using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    EnemyType type;
    public float health;
    Vector3 position;//trenutna pozicija neprijatelja

    public float speed;//brzina kretanja neprijatelja na osnovu tipa
    float speedFactor;//za usporenje

    //Cemu sluzi ovo int path, da nije mozda trebalo u GDD-u da pise da je path tipa Path?
    int path;
    int waypoint = 0;//tacka na pathu do koje se Enemy krece pravolinijski

    public AudioClip hitAudio;
    public AudioClip stealAudio;//zvuk kad Enemy dodje do kamena
    public AudioClip dyingAudio;

    //Promjenljive koje sam dodao u odnosu na GDD
    float slowdownTime;//vrijeme koliko traje usporenje
    Path put;//ovo sam dodao radi testiranja, tj. da bi uzeo niz waypoint-a za put
    bool alive;//da li je neprijatelj umro
    public AudioSource audioSource;
    private Animator anim;//za Die animaciju
    public GameObject model;//izgled neprijatelja

    void Start()
    {
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
                float distanceFromWayPoint = Vector3.Distance(transform.position, put.waypoints[waypoint]);//rastojanje neprijatelja od waypoint-a ka kom se krece
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
        transform.position = Vector3.RotateTowards(transform.position, put.waypoints[waypoint], rotationSpeed * Time.deltaTime, 0.0f);
    }

    void UpdatePosition(float distance)
    {
        Vector3 newPosition;
        float presao;
        while (distance > 0)
        {
            newPosition = Vector3.MoveTowards(transform.position, put.waypoints[waypoint], speed * Time.deltaTime);
            presao = Vector3.Distance(transform.position, newPosition);
            distance -= presao;
            transform.position = newPosition;
            if (distance <= 0)
            {
                RotationToWaypoint();
                waypoint++;
                if (waypoint == put.waypoints.Count - 1)
                { //ako Enemy stigne do zadnjeg waypoint-a(to je kamenje)
                    //od ukupnog broja kamenja oduzmemo onoliko kamenja koliko neprijatelj moze da ponese,a onda unistimo neprijatelja,npr. nesto ovako
                    //totalStones -= minStones;
                    Destroy(gameObject);
                    alive = false;
                    break;
                }
            }
        }
    }
    //trenutno rastojanje izmedju neprijatelja i kamenja,
    //ovaj metod treba da se pozove u klasi Hero pri odabiranju neprijatelja ka kom treba ispaliti projektil
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
	//testiranje ljubomir da vidim da li radi
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

}
