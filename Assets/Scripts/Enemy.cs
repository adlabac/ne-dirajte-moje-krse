using UnityEngine;
using System.Collections;
/*
 Napomene:
 * Metod CrashWithProjectile() nije dovrsen jer mislim da treba spojiti neke od klase koje smo radili da bi ovo dovrsio
 * Linije 146-150, bi li moglo ovo da radi ovako, ideja: enemy se krece (speed - speedFactor) * slowdownTime 
 */
public class Enemy : MonoBehaviour {
    EnemyType type; //za sad pod komentare da bi moglo da se kompajlira bez klase EnemyType
    public float health;
    Vector3 position;//trenutna pozicija neprijatelja
    
    public float speed;//brzina kretanja neprijatelja na osnovu tipa
    float speedFactor;//za usporenje
   
    //Cemu sluzi ovo int path?
    int path;
    int waypoint = 0;//tacka na pathu do koje se Enemy krece pravolinijski

    public AudioClip hitAudio;
    public AudioClip stealAudio;//zvuk kad Enemy dodje do kamena
    public AudioClip dyingAudio;

    //Promjenljive koje sam dodao u odnosu na GDD
    float slowdownTime;//vrijeme koliko traje usporenje
    Path put;//ovo sam dodao radi testiranja, tj. da bi uzeo niz waypoint-a za put
    bool notDead;//da li je neprijatelj umro
    bool isSlowedDown;
    public AudioSource audioSource;
    private Animator anim;//za Die animaciju
    public GameObject model;//izgled neprijatelja

    Projectile projektil;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        notDead = true;
        isSlowedDown = false;
    }

	void Update () {
        if(notDead){
            if (GetDistanceFromRocks() <= 0)//ako je Enemy stigao do kamenja
            {   //od ukupnog broja kamenja oduzmemo onoliko kamenja koliko neptijatelj moze da ponese,a onda unistimo neprijatelja,npr. nesto ovako
                //totalStones -= minStones;
                Destroy(gameObject);
                notDead = false;
            }
            if (projektil != null) { //ako se projektil krece ka neprijatelju
                float distanceFromProjectile = Vector3.Distance(projektil.transform.position, transform.position);
                if (distanceFromProjectile <= 0)
                {
                    CrashWithProjectile();
                }
                else {
                    isSlowedDown = false;
                }
            }
            if (health <= 0) //kad neprijatelj treba da umre
            {
                Death();
            }
            else { // azuraranje pozicije Enemy u zavisnosti od waypointa
                float distanceFromWayPoint = Vector3.Distance(transform.position, put.waypoints[waypoint]);
                if (distanceFromWayPoint <= 0) {
                    UpdateWaypoint();
                }
                if (notDead) {
                    UpdatePosition(); 
                }    
            }
        } 
	}
    //trenutno rastojanje izmedju neprijatelja i kamenja
    public float GetDistanceFromRocks()
    {
        int lastWaypoint = put.waypoints.Count - 1;
        return Vector3.Distance(transform.position, put.waypoints[lastWaypoint]);//rastojanje neprijatelja od zadnjeg waypointa koji predstavlja kamenje
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

    //Da bi ovaj metod mogao da se dovrsi potrebne su bar sledece klase: EnemyType, Hero i Projectile, ovo cu dovrsiti kad spojimo klase 
    void CrashWithProjectile()
    {
        //Napomena: Parametri za metode TakeDamage(float damage) i Slowdown(float factor,float time) treba definisati pri ispaljivanju projektila,
        //to jest kad Hero izabere metu,a tada treba odrediti vrijednosti za value,factor i time pomocu metoda GetDamage,GetSlowdown,GetSlowdownTime
        //a parametri sva 3 metoda su distanca izmedju heroja i neprijatelja u trenutku ispaljivanja projektila

        //PseudoKod
        /*
        TakeDamage(damage u trenutku ispljivanja projektila);
        //ako je speedFactor veci od 0, znaci da j
        if(speedFactor u trenutku ispljivanja projektila > 0 && type.name == "onaj koji moze biti usporen"){
            Slowdown(speedFactor u trenutku ispljivanja projektila, slowdownTime u trenutku ispljivanja projektila);
            isSlowedDown = true;
        }
        */
    }

    //u slucaju umiranja neprijatelja
    void Death()
    {
        //pri umiranju neprijatelja treba povecati coins, npr.
        //coins += reward;
        notDead = false;
        anim.SetTrigger("Die");//za animaciju triger
        PlayAudio(dyingAudio);
        Destroy(gameObject, 2f);//iz slicnog razloga odlozeno unistenje objekta kao i kod klase Projectile
        //odlozeno unistenje da bi se animacija i zvuk izvrsili do kraja
    }

    //kretanje ka waypointu
    void UpdateWaypoint()
    {
        // ako jos nisu stigli do zadnjeg waypoint-a koji predstavlja kamenje, ima smisla azurirati waypoint indeks
        if (waypoint < put.waypoints.Count - 1)
        {
            waypoint++;
            RotationToWaypoint();
        }
    }

    void RotationToWaypoint()
    {
        float rotationSpeed = 2f;//brzina rotiranja
        transform.position = Vector3.RotateTowards(transform.position, put.waypoints[waypoint], rotationSpeed * Time.deltaTime, 0.0f);
    }

    void UpdatePosition()
    {
        if (isSlowedDown)
        {
            //trebalo bi da se ovako azurira neprijatelja kada je usporen
            transform.position = Vector3.MoveTowards(transform.position, put.waypoints[waypoint], (speed - speedFactor) * slowdownTime);
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, put.waypoints[waypoint], speed * Time.deltaTime);//kretanje neprijatelja ka waypointu
        }
    }

    void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
