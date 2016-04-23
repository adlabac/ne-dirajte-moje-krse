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
    public float radius;//u inspektoru podesimo radijus
    public Color radiusColor;//inicijalna boja radijusa
    public List<Level> levels;
    //-------dodatne promjenjive za FemaleHero
    bool isWailing;
	private Animator anim;
    float wailingTimer;
    //-----------------------------------
	Transform heroRadius;

    //Inicijalizacija
    void Start()
    {
        enemies = new List<Enemy>();//u pocetku nema neprijatelja koje enemy moze da dohvati
        //levels = new Level[levels.Length-1];
        //Kasnije ce biti azurirano
		heroRadius = transform.Find("HeroRadius");	//stavili smo da bismo povecavali sliku sivog okolo heroja prilikom povecavanja range


		if (gameObject.tag == "Heroes" ) 
		{
			audioSource = GetComponent<AudioSource> ();
			PlayAudio (spawnAudio);
			//racunamo poluprecnik na osnovu nacrtanog prefaba (sprite za hero) i takav nam postaje circle collider
			radius = heroRadius.GetComponent<SpriteRenderer> ().bounds.size.x / 2;
            anim = GetComponent<Animator>();
        }

        if (gameObject.name.Contains("FemaleHero")) 
		{
            isWailing = false;
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
            if (gameObject.name.Contains("FemaleHero"))
			{
                if (wailingTimer <= GetWailingRate())
                {
                    wailingTimer += Time.deltaTime;
                }
                else {
                    wailingTimer = 0;
                }
                if (!isWailing) {		//ako prije nismo pozivali Invoke brojac je na 0
					InvokeRepeating ("Shout", 0, GetWailingRate()); //ako su u blizini neprijatelji pozivaj na 3 sekunde Shout, sa malim zakasnjenjem od 0.3sek
                    isWailing = true;			//postavljamo brojac na 1 dok svi protivnici ne izadju iz kruga zene(da ne bi vise puta pozivali InvokeRepeating)
                    
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
				CancelInvoke ();    //kada citav wave neprijatelja izadje iz kruga zene, zaustavi InvokeRepeating
                isWailing = false;			//postavljamo brojac na 0 kako bi opet prilikom upada neprijatelja novog u krug zene, pozvali InvokeRepeating
                wailingTimer = 0;
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
                if (wailingTimer <= GetSlowDownDuration()) {
                    enemies[enemies.Count - 1].Slowdown(GetSlowDownFactor(), GetSlowDownDuration() - wailingTimer);
                }
                
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
				enemies [i].Slowdown (GetSlowDownFactor(), GetSlowDownDuration()); //usporavanje neprijatelja svih u dometu sa slowDownFactor za vrijeme od slowDownDuration
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

    //Geteri i seteri

    public int GetPrice()
    {
        return levels[currentLevel].cost;
    }

    public int GetSellPrice()
    {
        return levels[currentLevel].costSell;
    }

    public float GetFireRate()
    {
        return levels[currentLevel].fireRate;
    }

    public float GetWailingRate()
    {
        return levels[currentLevel].wailingRate;
    }

    public float GetSlowDownFactor()
    {
        return levels[currentLevel].slowDownFactor;
    }

    public float GetSlowDownDuration()
    {
        return levels[currentLevel].slowDownDuration;
    }

    public void setLevel(int lev)
    {
        currentLevel = lev;
    }
	//---------------
	public float GetRange()
	{
		return levels [currentLevel].range;
	}
	//---------------

    public string GetNextLevel()
    {
        if (currentLevel < 4)
		{							
			heroRadius.localScale += new Vector3 (GetRange(), GetRange(), 0);		//uvecavamo sliku sivog oko heroja sa svakim levelom 
			radius = heroRadius.GetComponent<SpriteRenderer> ().bounds.size.x / 2;	//pa cemo prema njoj radius heroja povecavat(kontrolisat)
            currentLevel += 1;
            if (gameObject.tag == "Heroes" && !gameObject.name.Contains("FemaleHero"))
                InvokeRepeating("Shoot", 0.0F, GetFireRate());
            else if (gameObject.name.Contains("FemaleHero")) //za sad deaktivirano
                InvokeRepeating("Shout", 0, GetWailingRate());
        }

        if (currentLevel == 4)
            return "max";
        else
            return currentLevel.ToString();
    }
}
