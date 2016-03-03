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
//Da li Level da bude klasa,razmotriti.
//U metodu Start() - potrebno analizirati InvokeRepeating("Shoot", 0.0F, GetLevel().fireRate);
//Metod SetLevel() - potrebna analiza
//Kreirati u Unity hijerarhiji objekat koji sadrzi sve Heroje koji se nalaze na mapi

public class Level : MonoBehaviour
{
    public int cost;
    public GameObject model;
    public Projectile projectile;
    public float fireRate;
}

public class Hero : MonoBehaviour
{
    int currentLevel = 1;//trenutni level heroja
    Level[] levels;//niz levela za heroja
    List<Enemy> enemies; //Svi neprijatelji u dometu
    float shootTimer;
    private GameObject projectileParent;//ovdje se cuvaju svi projektili koji se spawnuju
    public AudioSource audioSource;
    public AudioClip spawnAudio;
    public AudioClip enemySpottedAudio;

    //Inicijalizacija
    void Start()
    {
        enemies = null;//u pocetku nema neprijatelja koje enemy moze da dohvati
        levels = new Level[levels.Length-1];
        //Kasnije ce biti azurirano
        audioSource = GetComponent<AudioSource>();
        PlayAudio(spawnAudio);
        projectileParent = GameObject.Find("Projectiles");
        if (projectileParent == null)//ako u hijerarhiji nema GameObject-a Projectiles, kreiraj ga
        {
            //ovdje kreiramo GameObject sa nazivom Projectiles i to je projectileParent
            projectileParent = new GameObject("Projectiles");
        }
        //Na osnovu trenutnog upgrade levela heroja, odredjujemo fireRate i pozivamo na svakih fireRate sekundi metod za ispaljivanje projektila
        InvokeRepeating("Shoot", 0.0F,GetLevel().fireRate);
    }

    //Update se vrsi jednom po frejmu
    void Update()
    {
        //Kasnije ce biti azurirano
    }

    Level GetLevel()
    {
        return levels[currentLevel];
    }

    Level GetMaxLevel()
    {
        return levels[levels.Length - 1];
    }
    //Potrebna dodatna analiza ovog metoda
    void SetLevel(int levelIndex)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[levelIndex].model != null)
            {
                if (i == levelIndex)
                {
                    levels[i].model.SetActive(true);//Aktivira objekat,tj. prikazuje njegov model
                }
                else
                {
                    levels[i].model.SetActive(false);
                }
            }
        }
    }

    //Napomena: Svaki heroj ima coolider koji predstavlja domet(poluprecnik) u kom on moze da ispali projektil
    void OnTriggerEnter2D(Collider2D other) // ovo other je objekat koji ima kolider i nalazi se u dometu kolidera Heroja
    {
        if (other.CompareTag("Enemy"))//ako objekat other ima Tag sa nazivom Enemy(Unity-u za Enemy treba postaviti da ima tag Enemy)
        {
            if (enemies.Count == 0) //Pustamo zvuk ako je lista neprijatelja prazna, tj. ulazi prvi neprijatelj u domet
            {
                PlayAudio(enemySpottedAudio);
            }
            enemies.Add(other.gameObject.GetComponent<Enemy>());//dodamo u listu enemies neprijatelja koji je usao u domet heroja
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        enemies.Remove(other.gameObject.GetComponent<Enemy>());//brisemo iz liste enemies neprijatelja koji je izasao iz dometa heroja
    }

    void PlayAudio(AudioClip clip)
    {
        //Debug.Log("sound");
        audioSource.clip = clip;
        audioSource.Play();
    }

    void Shoot()
    {
        if (enemies.Count > 0) //ako ima neprijatelja u dometu Heroja
        {
            //u newProjectile se cuva clone objekta projectile
            GameObject newProjectile = Instantiate(this.GetLevel().projectile.model) as GameObject;//kreiramo projektil koji trebamo da ispalimo ka neprijatelju koji je najblizi kamenju
            //parent od newProjectile je projectileParent  
            newProjectile.transform.parent = projectileParent.transform;//ovo uveo zbog sredjivanja Unity hijerarhije
            newProjectile.AddComponent<Projectile>().FireProjectile(ChooseTarget(), ChooseTarget().transform.position);//kako je newProjectile GameObject, moram da mu dodam komponentu Projectile da bi mogla da se pozove metoda FireProjectile
        }
    }

    Enemy ChooseTarget () { 
        //izaberemo onog neprijatelja iz liste neprijatelja koji je najblize cilju (kamenju)
        Enemy nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        foreach (Enemy enemy in enemies)
        {
            float dist = enemy.GetDistanceFromRocks();
            if (dist < minDistance) {
                nearestEnemy = enemy;
                minDistance = dist;
            }
        }
        return nearestEnemy;
    }

}
