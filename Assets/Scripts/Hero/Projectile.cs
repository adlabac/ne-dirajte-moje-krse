using UnityEngine;
using System.Collections;

//Osnovni:
//Start() - Pokupimo komponentu AudioSource iz Unity-a  koja je zakacena za projektil i stavimo na projektil jos nije eksplodirao 
//Update() - definisanje ponasanja projektila za svaki frame
//UpdatePosition() - azuriranje pozicije projektila
//PlayAudio(AudioClip projectileAudio) - pokretanje odgovarajuceg zvuka

//Dodatni metodi:
//Explode() - metod koji provjerava da li su se sudarili projektil i neprijatelj
//GetDamage(float distance) - odredjuje damage na osnovu rastojanja projektila od neprijatelja
//GetSlowdown(float distance) - modifikuje se brzina neprijatelja na osnovu rastojanja projektila od neprijatelja
//GetSlowdownDuration(float distance) - definise se koliko usporenje traje na osnovu rastojanja izmedju suprostavljenih strana
//FireProjectile(Enemy enemy, Vector3 enemyPosition) - ovdje definisemo metu projektila


//Potrebni dodatni komentari: 
//Metod Update() - u samom metodu sam i opisao sta treba sve da se definise prije izvrsavanja koda u ovom metodu, da li je potrebno jos nesto
//Metod Explode() - mozda se moze pojednostaviti, detaljno procitati komentare koje sam naveo u ovoj funkciji
//Metod FireProjectile() - treba li se jos sta dodati kod ovog metoda
//Kreirati u Unity hijerarhiji objekat koji sadrzi sve Projektile koji se nalaze na mapi radi bolje preglednosti hijerarhije

public class Projectile : MonoBehaviour {
    Enemy target; //neprijatelj ka kom treba da bude ispaljen projektil
    Vector3 targetPosition; // pozicija neprijatelja
    public GameObject model;//prefab projektila

    public float minRadius;//na osnovu ova dva atributa odredjuje se damage projektila, za manji radijus damage je veci
    public float maxRadius;

    public float minDamage;//procjena na osnovu radiusa
    public float maxDamage;

    public float minSlowdown;//procjena na osnovu radiusa
    public float maxSlowdown;

    public float minSlowdownDuration;//procjena na osnovu radiusa
    public float maxSlowdownDuration;

    public float speed = 10f;//brzina kretanja projektila
    //za razliciti tipove oruzija ce biti razlicita brzina
    public float distanceFromHero;
    public AudioClip shotAudio;
    public AudioClip impactAudio;
    private AudioSource audioSource;
    bool notExplode;//samo za metod Explode(),koristi se da se zvuk impactAudio ne bi aktivirao vise puta ako aktiviramo odlozeno unisavanje projektila, 
    //treba razmotriti postojanje ove promjenljive,vjerovatno postoji laksi nacin da se rijesi ovaj problem

    //inicijalizacija
	void Start () {
        audioSource = this.GetComponent<AudioSource>();
        notExplode = true;
	}

	void Update () {

        if (notExplode) //provjera da li je projektil vec ekplodirao,jer ako jeste nema smisla raditi ista sa njim
        {
            if (Vector3.Distance(transform.position, targetPosition) < speed * Time.deltaTime)//uslov kojim se provjerava da li se desio sudar izmedju projektila i neprijatelja(target)
            {
                Explode();
            }
            else
            {
                if (target != null) //ako postoji meta onda je prati,ovo target je u stvari Enemy koji je odabran metodom ChooseEnemy u klasi Hero
                {
                    
                    targetPosition = target.transform.position;//pratimo poziciju neprijatelja,treba nam za UpdatePosition() metod
                    RotationToTarget();
                    UpdatePosition();//azuriramo poziciju projektila u odnosu na metu(target)
                }
            }
        }        
	}

    //Za pomjeranje projektila ka meti
    void UpdatePosition() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    //kad se sudare projektil i neprijatelj
    //potrebne izmjene
    void Explode() {
        //Debug.Log("Explode");
        target.TakeDamage(GetDamage(distanceFromHero));//distanceFromHero podesili pri pozivu funkcije FireProjectile unutar klase Hero
        target.Slowdown(GetSlowdown(distanceFromHero), GetSlowdownDuration(distanceFromHero));

        PlayAudio(impactAudio);
        gameObject.GetComponent<Renderer>().enabled = false;//treba da sakrije prikaz projektila jer isti treba da nestane pri sudaru, ali ne i da bude unisten
        notExplode = false;//znaci projektil jeste eksplodirao, pa Update() vise nista ne radi
        Destroy(gameObject,1.5f);//projektil bude unisten posle 1.5sec(ovo vrijeme podlozno modifikaciji), odlozeno unistenje projektila da bi se cuo zvuk pri udaru
        //kada bi se gameObject(tekuci projektil) odmah pri sudaru unistio, unistio bi i komponentu za zvuk, pa bi se zvuk pri udaru projektila odmah prekinuo ! 
    }

    void PlayAudio(AudioClip projectileAudio) {
        if (audioSource != null)
        {
            audioSource.clip = projectileAudio;
            audioSource.Play();
        }
    }

    //definisanje damage-a
    public float GetDamage(float distance)
    {
        if (distance <= minRadius)
        { //projektil je blizu neprijatelja
            return maxDamage;
        }
        else if (distance <= maxRadius)
        {
            return minDamage;
        }
        else {
            return 0; // ako je enemy izvan dometa projektila 
        }
    }

    //Usporavanje neprijatelja
    public float GetSlowdown(float distance) {
        if (distance<= minRadius)
        {
            return maxSlowdown; 
        }
        else if (distance <= maxRadius)
        {
            return minSlowdown;
        }
        else {
            return 0; // ako je enemy izvan dometa projektila 
        }
        
    }

    //Trajanje usporavanja
    public float GetSlowdownDuration(float distance) {
        if (distance <= minRadius )
        {
            return maxSlowdownDuration;
        }
        else if (distance <= maxRadius)
        {
            return minSlowdownDuration;
        }
        else {
            return 0;
        }
        
    }

    //ovaj metod se poziva kada se izabere target pomocu metoda Enemy ChooseTarget (List<Enemy> enemies) koji treba da bude definisan u klasi Hero
    //i treba ga pozvati u klasi Hero,pa je zato public

    //Ovaj metod treba jos doradjivati
    public void FireProjectile(Enemy enemy, Vector3 enemyPosition) {
        PlayAudio(shotAudio);
        target = enemy;
        //Debug.Log(target.transform.position);
        targetPosition = enemyPosition;
        //Debug.Log(targetPosition);
        distanceFromHero = Vector3.Distance(enemyPosition, transform.position);//kada se pozove ovaj metod, odredice rastojanje izmedju Heroja i Enemy-a
    }
    void RotationToTarget()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float step = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
