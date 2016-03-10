using UnityEngine;
using System.Collections;
//Osnovni:
//Start() - Inicijalizazija parametara potrebnih za Enemy-a
//Update() - definisanje ponasanja Enemy-a za svaki frame
//UpdatePosition() - azuriranje pozicije Enemy-a
//PlayAudio(AudioClip clip) - pokretanje odgovarajuceg zvuka

//Dodatni metodi:
//Death() - Enemy vise nije ziv,treba da se pokrene animacija umiranja i odgovarajuci zvuk,a onda se unisti objekat(posle izvrsenja animacija i zvuka)
//RotationToWaypoint() - rotiranje neprijatelja ka waypointu, nisam bas siguran da li ispravno radi, treba malo dodatnog testiranja
//GetDistanceFromRocks() - trenutno rastojanje izmedju neprijatelja i kamenja
//TakeDamage(float value) - metod kojim treba smanjiti HP Enemy-u u zavisnosti od value(tj. damage-a projektila)
//Slowdown(float factor, float time) - metod kojim se definise koliko treba smanjiti brzinu Enemy-a i koliko to usporenje treba da traje
//SetEnemyParams() - podesavanje pocetnih vrijednosti za atribute Enemy-a 

//Komentari:
//Metod SetEnemyParams() nedovrsen zbog nedostatka konkretnih tipova neprijatelja

public class Enemy : MonoBehaviour
{
    EnemyType type;//tip neprijatelja
    public float health = 100f;//HP neprijatelja, ovo treba dodatno razmotriti, postoji i klasa EnemyHealth, ovaj atribut u ovom obliku vjerovatno treba maci
    Vector3 position;//trenutna pozicija neprijatelja
    public float speed = 3f;//brzina kretanja neprijatelja na osnovu tipa
    float speedFactor;//faktor koji utice na usporenje
    int pathIndex = 0;//pathIndex je indeks Patha iz klase GameLevel
    int waypoint = 0;//tacka na pathu do koje se Enemy krece pravolinijski

    public AudioClip hitAudio;
    public AudioClip stealAudio;//zvuk kad Enemy dodje do kamena
    public AudioClip dyingAudio;

    //Promjenljive koje sam dodao u odnosu na GDD
    float slowdownTime;//vrijeme koliko traje usporenje
    Path path;//ovo sam dodao radi testiranja, tj. da bi uzeo niz waypoint-a za put, za sad posmatramo kao da imamo samo jedan tip puta
    bool alive;//da li je neprijatelj umro, ovo mora postojati zbog odlozenog unistenja objekta - ovim sprecavama da se Update() izvsava i nakon umiranja Enemy-a
    public AudioSource audioSource;//tu se mijenjaju AudioClip-ovi(pomocu metoda PlayAudio(AudioClip clip) u zavisnosti od situacije
    private Animator anim;//za Die animaciju
    public GameObject model;//izgled neprijatelja
    bool isSlowedDown;//da li je Enemy usporen
    bool canSteal; //da li Enemy moze da pokupi kamen

    void Start()
    {
        path = FindObjectOfType<GameLevel>().paths[pathIndex];
        //U zavisnosti od GameLevela biram index puta !
        //treba ispravno podesiti i tip neprijatelja , naknadno ce biti odradjeno
        //SetEnemyParams();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        alive = true;
        canSteal = true;
        isSlowedDown = false;
    }

    void Update()
    {
        if (alive && canSteal)
        {
            if (health <= 0) //kad neprijatelj treba da umre
            {
                Death();
            }
            else {
                if (isSlowedDown)
                {
                    //Debug.Log(slowdownTime);
                    if (slowdownTime <= 0) //ako vise nije usporen
                    {
                        isSlowedDown = false;
                        //speed = type.defaultSpeed;//ako nije usporen moramo mu vratiti default speed
                        speed = 3f;//vrati se na pocetnu brzinu
                    }
                    else
                    {
                        slowdownTime -= Time.deltaTime;//ovdje podesavamo koliko ce dugo biti usporavan
                    }
                }
                float distanceFromWayPoint = Vector3.Distance(transform.position, path.wayPoints[waypoint]);//rastojanje neprijatelja od waypoint-a ka kom se krece
                UpdatePosition(distanceFromWayPoint);
            }
        }
    }

    //u slucaju umiranja neprijatelja
    void Death()
    {
        //ScoreManager.AddCoins(type.reward);//pri umiranju neprijatelja treba povecati coins,kad budu definisani tipovi neprijatelja ovaj komenar maci
        ScoreManager.AddCoins(50);
        alive = false;
        canSteal = false;
        //anim.SetTrigger("Die");//za animaciju triger

        PlayAudio(dyingAudio);
        gameObject.GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, 2f);//iz slicnog razloga kao i kod klase Projectile, odlozeno unistenje objekta 
        //odlozeno unistenje da bi se animacija i zvuk izvrsili do kraja
    }
    //Rotacija ka waypoint-u
    void RotationToWaypoint()
    {
        Vector3 moveDirection = gameObject.transform.position - path.wayPoints[waypoint];
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void UpdatePosition(float distance)
    {
        Vector3 newPosition;
        float presao;
        if (distance > 0)
        {
            newPosition = Vector3.MoveTowards(transform.position, path.wayPoints[waypoint], speed*Time.deltaTime);//koliko se Enemy pomjeri od trenutne do sledece pozicije ka waypointu
            presao = Vector3.Distance(transform.position, newPosition);//koliko je presao od pocetne pozicije pa do nove pozicije
            distance -= presao;//distanca do waypointa se smanjuje
            transform.position = newPosition;//tek onda pomjerimo Enemy-a
            if (distance <= 0)//provjerimo da li je stigao do waypointa
            {
                if (waypoint == path.wayPoints.Count - 1) //ako Enemy stigne do zadnjeg waypoint-a(to je kamenje)
                {
                    //od ukupnog broja kamenja oduzmemo onoliko kamenja koliko neprijatelj moze da ponese,a onda unistimo neprijatelja
                    //ScoreManager.RemoveStones(Random.Range(type.minStones, type.maxStones + 1));//[min,max) zato sam stavio +1a
                    ScoreManager.RemoveStones(Random.Range(1, 4 + 1));
                    PlayAudio(stealAudio);
                    Destroy(gameObject,2f);//drugi arg. podesavamo u zavisno od trajanja zvuka stealAudio
                    canSteal = false;
                    alive = false;
                    gameObject.GetComponent<Renderer>().enabled = false;//Enemy mora da nestane 
                     
                }
                if (alive) { //da izmjegnemo error
                    waypoint++;//neprijatelj se krece ka novom waypoint-u
                    RotationToWaypoint();//rotiramo neprijatelja ka tom novom waypointu
                }
            }
        }
    }

    //ovaj metod treba da se pozove u klasi Hero pri odabiranju neprijatelja ka kom treba ispaliti projektil
    public float GetDistanceFromRocks()
    {
        int lastWaypoint = path.wayPoints.Count - 1;
        return Vector3.Distance(transform.position, path.wayPoints[lastWaypoint]);//rastojanje neprijatelja od zadnjeg waypointa koji predstavlja kamenje
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
        if (!isSlowedDown) { //mislim da ako je neprijatelj usporen, a ako ga pogodi projektil koji usporava, nema potrebe da ga dodatno usporimo jer bi rizikovali da speed dodje na 0
            speed -= speedFactor;
        }
        isSlowedDown = true;
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
        type = GetComponent<EnemyTypes>().GetByName("NazivNeprijatelja");//moguce da ce trebati u unity-u za Enemy dodati i komponentu EnemyTypes 
    }

}
