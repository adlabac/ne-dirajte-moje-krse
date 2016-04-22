using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Osnovni:
//Start() - Pokupimo komponentu AudioSource 
//Update() - definisanje ponasanja Heroja za svaki frame
//GetLevel() - Vraca trenutni level Heroja
//GetMaxLevel() - Metod vraca maksimalni level upgrade-a Heroja
//SetLevel(int levelIndex) - Postavlja level upgrade-a na nivo zadat indeksom
//OnTriggerEnter2D(Collider2D other) - Kad neprijatelj udje u domet heroja(definisemo u Unity-u collider), dodajemo ga u listu neprijatelja u dometu
//OnTriggerExit2D(Collider2D other) - Kad neprijatelj izadje iz dometa heroja, brisemo ga iz liste neprijatelja u dometu 
//Shoot() - Metod koji, ako ima neprijatelja u dometu, izvrsava ispaljivanje projektila na najblizeg neprijatelja      
//PlayAudio(AudioClip clip) - pokretanje odgovarajuceg zvuka

//Dodatni metodi:
//ChooseTarget () - Heroji uvijek napadaju neprijatelja u dometu koji je najbliži cilju.Ako jedan neprijatelj pretekne onog koji je trenutno napadnut, napad će se preusmjeriti na njega.
//Shoot() - ispaljivanje projektila ka meti

//Komentari:
//U metodu Start() - potrebno analizirati InvokeRepeating("Shoot", 0.0F, GetLevel().fireRate);
//Metod SetLevel() - potrebna analiza
//Kreirati u Unity hijerarhiji objekat koji sadrzi sve Heroje koji se nalaze na mapi

public class Level
{
    public int cost; //promjenljiva u kojoj se cuva cijena heroja ili upgradea
	public int costSell; //prodaja towera
    public GameObject model;
    public Projectile projectile;
    public float fireRate;
	public float range;


	public Level(int c, int cs, float fr, float r){
		cost = c;
		costSell = cs;
		fireRate = fr;
		range = r;
	}


	public int GetCost(){
		return cost;
	}

	public int GetCostSell(){
		return costSell;
	}
}


public class Hero : MonoBehaviour
{
    int currentLevel = 0;//trenutni level heroja
    List<Enemy> enemies; //Svi neprijatelji u dometu
    float shootTimer;
    GameObject projectileParent;//ovdje se cuvaju svi projektili koji se spawnuju
    AudioSource audioSource;

    public Projectile projectile;
    public AudioClip spawnAudio;
    public AudioClip enemySpottedAudio;
    //public static int heroPrice = 50;
	//public static int heroSellPrice = 30;
	//public static int heroUp1Price = 20;
	//public static int heroUp2Price = 30;
	//public static int heroUp3Price = 50;
	//public int level = 1;
    public float radius;//u inspektoru podesimo radijus
    public Color radiusColor;//inicijalna boja radijusa

	//-------dodatne promjenjive za FemaleHero
	int brojac;

	private Animator anim;

	public float slowDownFactor;
	public float slowDownDuration;
	//-----------------------------------

	public Level[] levels = {
		new Level (70, 0, 0, 0), //nulti nivo - cijena gradjenja
		new Level (90, 90, 0.5f, 1f),
		new Level (100, 150, 0.4f, 1f),
		new Level (120, 200, 0.3f, 1f),
		new Level (0, 200, 0.3f, 1f),
	};

    //Inicijalizacija
    void Start()
    {
        enemies = new List<Enemy>();//u pocetku nema neprijatelja koje enemy moze da dohvati
        //levels = new Level[levels.Length-1];
        //Kasnije ce biti azurirano

		if (gameObject.tag == "Heroes" ) 
		{
			audioSource = GetComponent<AudioSource> ();
			PlayAudio (spawnAudio);
		
			//racunamo poluprecnik na osnovu nacrtanog prefaba (sprite za hero) i takav nam postaje circle collider
			radius = transform.Find ("HeroRadius").GetComponent<SpriteRenderer> ().bounds.size.x / 2;
		}
        if (gameObject.name.Contains("FemaleHero")) 
		{
			/*float x = 3f;
			for(int cnt = 0; cnt < levels.Length; cnt++){
				levels [cnt].fireRate = x;
				x -= 0.1f;
			}*/
			brojac = 0;
			anim = GetComponent<Animator> ();
			anim.SetBool ("lelekanje", false);
		}

		//podesavamo radius collidera
		if (Mathf.Abs(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y)) != 0){
			GetComponent<CircleCollider2D>().radius = radius / Mathf.Abs(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y));
        }
        else {
			GetComponent<CircleCollider2D>().radius = radius;
        }

		if (gameObject.tag == "Heroes" && !gameObject.name.Contains("FemaleHero"))
		{
			//pravimo objekat zbog hijerarhije
			projectileParent = GameObject.Find ("Projectiles");
			if (projectileParent == null) {//ako u hijerarhiji nema GameObject-a Projectiles, kreiraj ga
				//ovdje kreiramo GameObject sa nazivom Projectiles i to je projectileParent
				projectileParent = new GameObject ("Projectiles");
			}
			//Na osnovu trenutnog upgrade levela heroja, odredjujemo fireRate i pozivamo na svakih fireRate sekundi metod za ispaljivanje projektila
			//InvokeRepeating("Shoot", 0.0F, GetLevel().fireRate);
			InvokeRepeating ("Shoot", 0.0F, GetFireRate ());
		}
    }


    //Update se vrsi jednom po frejmu
    void Update()
    {
			
        //Kasnije ce biti azurirano
        if (enemies.Count != 0)
        {
            Rotation();
            //Shoot();
			if(gameObject.name.Contains("FemaleHero"))
			{
				if (brojac == 0) {		//ako prije nismo pozivali Invoke brojac je na 0
					InvokeRepeating ("Shout", 0.3f, 3.0f);	//ako su u blizini neprijatelji pozivaj na 3 sekunde Shout, sa malim zakasnjenjem od 0.3sek
					brojac++;			//postavljamo brojac na 1 dok svi protivnici ne izadju iz kruga zene(da ne bi vise puta pozivali InvokeRepeating)
				} else {
					anim.SetBool ("lelekanje", false);		//ako smo vec pozvali InvokeRepeating a protivnici su i dalje u blizini, postavi brojac na 1 da ne bi opet pozvali InvokeRepeating
				}
			}
        }
		if (enemies.Count == 0) 
		{
			radiusColor = Color.green;
            if (gameObject.name.Contains("FemaleHero")) 
			{
				anim.SetBool ("lelekanje", false);
				CancelInvoke ();	//kada citav wave neprijatelja izadje iz kruga zene, zaustavi InvokeRepeating
				brojac = 0;			//postavljamo brojac na 0 kako bi opet prilikom upada neprijatelja novog u krug zene, pozvali InvokeRepeating
			}	
		}
    }
		

	//klik na cijeli collider - moguce je vidjeti njegov radius	
	void OnMouseUp ()
	{
		if (gameObject.tag == "Heroes") {
			GameLevel.setHeroRadiusesInactive ("Heroes", "HeroRadius", "HeroMenus");
		} 
	}


	Enemy ChooseTarget () { 
		//izaberemo onog neprijatelja iz liste neprijatelja koji je najblize cilju (kamenju)
		Enemy nearestEnemy = null;
		float minDistance = Mathf.Infinity;
		foreach (Enemy enemy in enemies)
		{
            if (enemy.tag == "Enemies") { 
                float dist = enemy.GetDistanceFromRocks();
                if (dist < minDistance)
                {
                    nearestEnemy = enemy;
                    minDistance = dist;
                }
            }
		}
		return nearestEnemy;
	}
	public int GetPrice()
	{
		return levels[currentLevel].GetCost();
	}

	public int GetSellPrice()
	{
		return levels[currentLevel].GetCostSell();
	}

	public float GetFireRate()
	{
		return levels [currentLevel].fireRate;
	}

	public void setLevel(int lev)
	{
		currentLevel = lev;
	}


	public string GetNextLevel()
	{
		if (currentLevel < 4) {
			currentLevel += 1;
			if(gameObject.tag == "Heroes" && !gameObject.name.Contains("FemaleHero"))
				InvokeRepeating ("Shoot", 0.0F, GetFireRate ());
            else if (gameObject.name.Contains("FemaleHero"))
				InvokeRepeating ("Shout", 0.3F, GetFireRate ());
		}

		if (currentLevel==4)
			return "max";
		else
			return currentLevel.ToString ();
	}

	void Rotation()
	{
        if (ChooseTarget() != null) 
        {
            Vector3 moveDirection = gameObject.transform.position - ChooseTarget().transform.position;
            if (moveDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 90f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
		
	}

	//Napomena: Svaki heroj ima coolider koji predstavlja domet(poluprecnik) u kom on moze da ispali projektil
	void OnTriggerEnter2D(Collider2D other) // ovo other je objekat koji ima kolider i nalazi se u dometu kolidera Heroja
	{
		if (other.CompareTag("Enemies"))//ako objekat other ima Tag sa nazivom Enemy(Unity-u za Enemy treba postaviti da ima tag Enemy)
		{
            radiusColor = Color.red;
			if (enemies.Count == 0) //Pustamo zvuk ako je lista neprijatelja prazna, tj. ulazi prvi neprijatelj u domet
			{
				//PlayAudio(enemySpottedAudio);
			}
            
            enemies.Add(other.gameObject.GetComponent<Enemy>());//dodamo u listu enemies neprijatelja koji je usao u domet heroja
            enemies[enemies.Count - 1].SetDetected(this);
            if (this.gameObject.name.Contains("FemaleHero")) {
                enemies[enemies.Count - 1].Slowdown(slowDownFactor, slowDownDuration);
            }
        }
	}

	void OnTriggerExit2D(Collider2D other)
	{
        Enemy enemyLeftRadius = other.gameObject.GetComponent<Enemy>();
        enemyLeftRadius.UnsetDetected(this);
        enemies.Remove(enemyLeftRadius);//brisemo iz liste enemies neprijatelja koji je izasao iz dometa heroja
	}

	void PlayAudio(AudioClip clip)
	{
		audioSource.clip = clip;
		audioSource.Play();
	}

    void Shoot()
    {
        if (enemies.Count > 0) //ako ima neprijatelja u dometu Heroja
        {
			if (ChooseTarget() != null)
			{
            	GameObject newProjectile = Instantiate(projectile.model, transform.position, Quaternion.identity) as GameObject;
            	newProjectile.transform.parent = projectileParent.transform;//ovo uveo zbog sredjivanja Unity hijerarhije
				newProjectile.GetComponent<Projectile>().FireProjectile(ChooseTarget(), ChooseTarget().transform.position);//kako je newProjectile GameObject, moram da mu dodam komponentu Projectile da bi mogla da se pozove metoda FireProjectile
			}
		}
    }
		
	void Shout() //lelekanje zene
	{
		if (enemies.Count > 0) { //ako ima neprijatelja u dometu Heroja
			anim.SetBool ("lelekanje", true);
			for (int i = 0; i < enemies.Count; i++) {
				enemies [i].Slowdown (slowDownFactor, slowDownDuration); //usporavanje neprijatelja svih u dometu sa slowDownFactor za vrijeme od slowDownDuration
			}
		} 
	}

    void OnDrawGizmos() {
        Gizmos.color = radiusColor;
        Gizmos.DrawWireSphere(transform.position , radius);
    }

    public List<Enemy> GetEnemies() {
        return enemies;
    }

    public void RemoveEnemy(Enemy enemy) {
        enemies.Remove(enemy);
    }
}
