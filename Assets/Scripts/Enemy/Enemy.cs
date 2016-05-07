using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public class Enemy : MonoBehaviour
{
    EnemyType type;//tip neprijatelja
    Vector3 position;//trenutna pozicija neprijatelja
    float speedFactor;//faktor koji utice na usporenje
    int pathIndex = 0;//pathIndex je indeks Patha iz klase GameLevel
    int waypoint = 1;//tacka na pathu do koje se Enemy krece pravolinijski

    public float health;//HP neprijatelja
    public float speed;//brzina kretanja neprijatelja na osnovu tipa
    public AudioClip hitAudio;
    public AudioClip stealAudio;//zvuk kad Enemy dodje do kamena
    public AudioClip dyingAudio;

    //Promjenljive koje sam dodao u odnosu na GDD
    float slowdownTime;//vrijeme koliko traje usporenje
    Path path;//ovo sam dodao radi testiranja, tj. da bi uzeo niz waypoint-a za put, za sad posmatramo kao da imamo samo jedan tip puta
    bool alive;//da li je neprijatelj umro, ovo mora postojati zbog odlozenog unistenja objekta - ovim sprecavama da se Update() izvsava i nakon umiranja Enemy-a
    AudioSource audioSourceEnemy;//tu se mijenjaju AudioClip-ovi(pomocu metoda PlayAudio(AudioClip clip) u zavisnosti od situacije
    Animator anim;//za Die i Jump animaciju
    public bool isSlowedDown;//da li je Enemy usporen
    bool canSteal; //da li Enemy moze da pokupi kamen
    List<Hero> heroes;//lista heroja koje vidi neprijatelj

    public GameObject model;//izgled neprijatelja
    Vector3 offset;
    List<Vector3> newPath;

    //Promjenljive za skakavca
    public bool targetable = true;//da li heroj moze da puca ka neprijatelju
    public float distanceToJump;//predjeni put do skoka
    List<int> jumpsBetweenWaypoints;
    Vector3 startJumpPosition;
    public float jumpDistance;//ako je jumpDistance = 0 onda Enemy ne moze da skoci
    //Pomocne promjenljive za UpdatePosition
    int numJumpsBetweenWaypoints;//broj mogucih skokova izmedju 2 waypointa
    bool isFirstCallJump;

    void Start()
    {
        isFirstCallJump = false;
        
        jumpsBetweenWaypoints = new List<int>();
        heroes = new List<Hero>();
        //U zavisnosti od GameLevela biram index puta !
        path = FindObjectOfType<GameLevel>().paths[pathIndex];
        SetEnemyParams();
        anim = GetComponent<Animator>();
        audioSourceEnemy = this.GetComponent<AudioSource>();
        alive = true;
        canSteal = true;
        isSlowedDown = false;
        RotationToWaypoint();
        //Potrebno za offset path
        offset = new Vector3(Random.Range(-0.35f, 0.35f), Random.Range(-0.35f, 0.35f), 0);
        newPath = new List<Vector3>();
        for (int i = 0; i < path.wayPoints.Count; i++) {
            newPath.Add(path.wayPoints[i] + offset);
            if (i > 0)
            {
                float dis = Vector3.Distance(path.wayPoints[i] ,path.wayPoints[i - 1]);//distana izmedju 2 waypointa
                jumpsBetweenWaypoints.Add((int)Mathf.Floor(dis / (jumpDistance + distanceToJump))); 
            }
        }
        numJumpsBetweenWaypoints = jumpsBetweenWaypoints[waypoint - 1];
        GameObject enemiesGameObject = GameObject.Find("Enemies");
        this.transform.parent = enemiesGameObject.transform;
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
                    if (slowdownTime <= 0) //ako vise nije usporen
                    {
                        isSlowedDown = false;
                        speed = type.defaultSpeed;//ako nije usporen moramo mu vratiti default speed
                    }
                    else
                    {
                        slowdownTime -= Time.deltaTime;//ovdje podesavamo koliko ce dugo biti usporavan
                    }
                }
                float distanceFromWayPoint = Vector3.Distance(transform.position, newPath[waypoint]);//rastojanje neprijatelja od waypoint-a ka kom se krece
                UpdatePosition(distanceFromWayPoint);
            }
        }
    }

    //u slucaju umiranja neprijatelja
    void Death()
    {
        ScoreManager.AddCoins(type.reward);//pri umiranju neprijatelja treba povecati coins
        alive = false;
        canSteal = false;
        //anim.SetTrigger("Die");//za animaciju triger
        PlayAudio(dyingAudio);
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;//i ovo sam morao da dodam jer kad postavim heroja onamo gdje je enemy stradao,
        //a nije proslo dvije sekunde, kako je ostao kolider, heroj ce ga detektovat
        foreach (Hero hero in heroes.ToList()) //bez ovog dijela .ToList() javlja gresku
        {
            hero.RemoveEnemy(this);
        }

        Destroy(gameObject,2f);//iz slicnog razloga kao i kod klase Projectile, odlozeno unistenje objekta 
        //odlozeno unistenje da bi se animacija i zvuk izvrsili do kraja
    }
    //Rotacija ka waypoint-u
    void RotationToWaypoint()
    {
        foreach (Transform child in transform) {
            if (child.tag == "EnemyModel") {
                Vector3 moveDirection = child.position - path.wayPoints[waypoint];
                if (moveDirection != Vector3.zero)
                {
                    float angle;
                    angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 90f;
                    child.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }
    }

    void UpdatePosition(float distance)
    {
        
        Vector3 newPosition;
        float presao;
        Vector3 pom = newPath[waypoint - 1];
        if (distance > 0)
        {

            newPosition = Vector3.MoveTowards(transform.position, newPath[waypoint], speed * Time.deltaTime);//koliko se Enemy pomjeri od trenutne do sledece pozicije ka waypointu
            presao = Vector3.Distance(transform.position, newPosition);//koliko je presao od pocetne pozicije pa do nove pozicije
            distance -= presao;//distanca do waypointa se smanjuje
            transform.position = newPosition;//tek onda pomjerimo Enemy-a


            if(this.jumpDistance != 0)//da li enemy moze da skoci
            {
                if((Vector3.Distance(transform.position,pom)) > distanceToJump && numJumpsBetweenWaypoints > 0 && isFirstCallJump == false)//pocetak skoka
                {
                    Jump();
                    isFirstCallJump = true;
                }

                if(this.jumpDistance < Vector3.Distance(transform.position, startJumpPosition) && !targetable) //uslov za kraj skoka
                {
                    Debug.Log("Kraj skoka");
                    pom = transform.position;
                    targetable = true;
                    numJumpsBetweenWaypoints--;
                    isFirstCallJump = false;
                }
            }


            if (distance <= 0)//provjerimo da li je stigao do waypointa
            {

                if (waypoint == path.wayPoints.Count - 1) //ako Enemy stigne do zadnjeg waypoint-a(to je kamenje)
                {
                    //od ukupnog broja kamenja oduzmemo onoliko kamenja koliko neprijatelj moze da ponese,a onda unistimo neprijatelja
                    ScoreManager.RemoveStones(Random.Range(type.minStones, type.maxStones + 1));//[min,max) zato sam stavio +1
                    PlayAudio(stealAudio);
                    Destroy(gameObject,2f);//drugi arg. podesavamo u zavisno od trajanja zvuka stealAudio
                    canSteal = false;
                    alive = false;
                    foreach (Transform child in gameObject.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    foreach (Hero hero in heroes.ToList())
                    {
                        hero.RemoveEnemy(this);
                    }

                    gameObject.GetComponent<Renderer>().enabled = false;//Enemy mora da nestane 
                }
                if (alive) { //da izmjegnemo error
                    waypoint++;//neprijatelj se krece ka novom waypoint-u
                    numJumpsBetweenWaypoints = jumpsBetweenWaypoints[waypoint - 1];
                    RotationToWaypoint();//rotiramo neprijatelja ka tom novom waypointu
                }
            }
        }
    }

    //ovaj metod treba da se pozove u klasi Hero pri odabiranju neprijatelja ka kom treba ispaliti projektil
    public float GetDistanceFromRocks()
    {
        int lastWaypoint = path.wayPoints.Count - 1;
        float distanceFromRocks = Vector3.Distance(transform.position,path.wayPoints[waypoint]);//rastojanje od tekuce pozicije neprijatelja do waypointa
        int waypointIndex = waypoint ;
        while (waypointIndex < lastWaypoint) //sabereme rastojanja izmjedju svih preostalih waypointa
        {
            distanceFromRocks += Vector3.Distance(path.wayPoints[waypointIndex], path.wayPoints[waypointIndex + 1]);
            waypointIndex++;
        }
        return distanceFromRocks;
    }
    //Odrediti float value pomocu metoda GetDamage(float distance) kada Hero izabere neprijatelja, a pozvati ovaj metod kada se sudare neprijatelj i projektil
    public void TakeDamage(float value)
    {
        if (alive) {
            health -= value;
            PlayAudio(hitAudio);
		} 
    }
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
        if (audioSourceEnemy != null) {
            audioSourceEnemy.clip = clip;
            audioSourceEnemy.Play();
        }
    }
    void SetEnemyParams() {
        type = GetComponent<EnemyType>();
        health = type.initialHealth;
        speed = type.defaultSpeed;
        speedFactor = type.slowdownFactor;
    }

    public void SetDetected(Hero hero) {
        heroes.Add(hero);
    }

    public void UnsetDetected(Hero hero) {
        heroes.Remove(hero);
    }

    void Jump()
    {
        startJumpPosition = transform.position;
        //anim.SetTrigger("Jump");//za animaciju triger
        targetable = false;
        Debug.Log("Jump");
    }

}
