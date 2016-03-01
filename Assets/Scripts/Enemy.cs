using UnityEngine;
using System.Collections;
//Osnovni:
//Start() - Pokupimo komponente AudioSource i Animator iz Unity-a koje su zakacena za neprijatleja i stavimo da je Enemy na pocetku ziv 
//Update() - definisanje ponasanja Enemy-a za svaki frame
//UpdatePosition() - azuriranje pozicije Enemy-a, ovaj metod treba jos nadogradjivati jer treba srediti i situaciji sta ce se desiti kada je Enemy usporen i koliko treba da traje to usporenje
//PlayAudio(AudioClip clip) - pokretanje odgovarajuceg zvuka

//Dodatni metodi:
//Death() - Enemy vise nije ziv,treba da se pokrene animacija umiranja i odgovarajuci zvuk,a onda se unisti objekat(posle izvrsenja animacija i zvuka)
//RotationToWaypoint() - rotiranje neprijatelja ka waypointu, nisam bas siguran da li ispravno radi, treba malo dodatnog testiranja
//GetDistanceFromRocks() - trenutno rastojanje izmedju neprijatelja i kamenja
//TakeDamage(float value) - metod kojim treba smanjiti HP Enemy-u u zavisnosti od value(tj. damage-a projektila)
//Slowdown(float factor, float time) - metod kojim se definise koliko treba smanjiti brzinu Enemy-a i koliko to usporenje treba da traje  

//Komentari:
//Linije 74-75 (to mozda ne treba odraditi u ovoj klasi), slicno za Linije 104-105
//Metod RotationToWaypoint() - dodatno testiranje
//Metod UpdatePostion() - provjeriti ponasanje ovog metoda kada je putanja kosa
//Metodi TakeDamage i Slowdown - jos stvari treba odraditi kod ovih metoda
//Sta bi jos trebalo dodati?
public class Enemy : MonoBehaviour
{
    EnemyType type;//tip neprijatelja
    public float health;//HP neprijatelja, ovo treba dodatno razmotriti, postoji i klasa EnemyHealth, ovaj atribut u ovom obliku vjerovatno treba maci
    Vector3 position;//trenutna pozicija neprijatelja

    public float speed;//brzina kretanja neprijatelja na osnovu tipa
    float speedFactor;//za usporenje

    int pathIndex = 0;//pathIndex je indeks Patha iz klase GameLevel, neka je to za sad jedan put sa indeksom 0.
    int waypoint = 0;//tacka na pathu do koje se Enemy krece pravolinijski


    public AudioClip hitAudio;
    public AudioClip stealAudio;//zvuk kad Enemy dodje do kamena
    public AudioClip dyingAudio;

    //Promjenljive koje sam dodao u odnosu na GDD
    float slowdownTime;//vrijeme koliko traje usporenje
    Path[] path;//ovo sam dodao radi testiranja, tj. da bi uzeo niz waypoint-a za put, za sad posmatramo kao da imamo samo jedan tip puta
    bool alive;//da li je neprijatelj umro, ovo mora postojati zbog odlozenog unistenja objekta - ovim sprecavama da se Update() izvsava i nakon umiranja Enemy-a
    public AudioSource audioSource;//tu se mijenjaju AudioClip-ovi(pomocu metoda PlayAudio(AudioClip clip) u zavisnosti od situacije
    private Animator anim;//za Die animaciju
    public GameObject model;//izgled neprijatelja
    int reward; // nagrada kada projektil unisti Enemy-a

    void Start()
    {
        //treba podesiti i tip neprijatelja , naknadno ce biti odradjeno
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        alive = true;
    }

    void Update()
    {
        if (alive)
        {
            if (health <= 0) //kad neprijatelj treba da umre
            {
                Death();
            }
            else {
                float distanceFromWayPoint = Vector3.Distance(transform.position, path[pathIndex].wayPoints[waypoint]);//rastojanje neprijatelja od waypoint-a ka kom se krece
                UpdatePosition(distanceFromWayPoint);
            }
        }
    }

    //u slucaju umiranja neprijatelja
    void Death()
    {
        //pri umiranju neprijatelja treba povecati coins, npr.
        //coins += reward;
        alive = false;
        anim.SetTrigger("Die");//za animaciju triger
        PlayAudio(dyingAudio);
        Destroy(gameObject, 2f);//iz slicnog razloga odlozeno unistenje objekta kao i kod klase Projectile
        //odlozeno unistenje da bi se animacija i zvuk izvrsili do kraja
    }
    void RotationToWaypoint()
    {
        float rotationSpeed = 2f;//brzina rotiranja
        transform.position = Vector3.RotateTowards(transform.position, path[pathIndex].wayPoints[waypoint], rotationSpeed * Time.deltaTime, 0.0f);
    }

    void UpdatePosition(float distance)
    {
        Vector3 newPosition;
        float presao;
        while (distance > 0)
        {
            newPosition = Vector3.MoveTowards(transform.position, path[pathIndex].wayPoints[waypoint], speed * Time.deltaTime);//koliko se Enemy pomjeri od trenutne do sledece pozicije ka waypointu
            presao = Vector3.Distance(transform.position, newPosition);//koliko je presao od pocetne pozicije pa do nove pozicije
            distance -= presao;//distanca do waypointa se smanjuje
            transform.position = newPosition;//tek onda pomjerimo Enemy-a
            if (distance <= 0)//provjerimo da li je stigao do waypointa
            {
                waypoint++;//neprijatelj se krece ka novom waypoint-u
                RotationToWaypoint();//rotiramo neprijatelja ka tom novom waypointu
                if (waypoint == path[pathIndex].wayPoints.Count - 1) //ako Enemy stigne do zadnjeg waypoint-a(to je kamenje)
                { 
                    //od ukupnog broja kamenja oduzmemo onoliko kamenja koliko neprijatelj moze da ponese,a onda unistimo neprijatelja,npr. nesto ovako
                    //totalStones -= amountOfStonesEnemyTake;
                    Destroy(gameObject);
                    alive = false;
                    break; // moramo prekinuti izvrsavanje petlje jer Enemy vise nije ziv
                }
            }
        }
    }

    //ovaj metod treba da se pozove u klasi Hero pri odabiranju neprijatelja ka kom treba ispaliti projektil
    public float GetDistanceFromRocks()
    {
        int lastWaypoint = path[pathIndex].wayPoints.Count - 1;
        return Vector3.Distance(transform.position, path[pathIndex].wayPoints[lastWaypoint]);//rastojanje neprijatelja od zadnjeg waypointa koji predstavlja kamenje
    }
    //Odrediti float value pomocu metoda GetDamage(float distance) kada Hero izabere neprijatelja, a pozvati ovaj metod kada se sudare neprijatelj i projektil
    public void TakeDamage(float value)
    {
        health -= value;
        PlayAudio(hitAudio);
    }
    //slicno kao i za prethodni metod
    public void Slowdown(float factor, float time)
    {
        speedFactor = factor;
        slowdownTime = time;
    }

    void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    //Ovaj metod mozda treba prevesti u konstruktor
    void SetEnemyParams() {
        health = type.initialHealth;
        speed = type.defaultSpeed;
        speedFactor = type.slowdownFactor;
        reward = type.reward;
    }

}
