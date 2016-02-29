using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public int cost;
    public GameObject model;
    public Projectile projectile;
    public float fireRate;
}

public class Hero : MonoBehaviour
{
    public AudioClip spawnAudio;
    public AudioClip enemySpottedAudio;
    int currentLevel = 1;
    Level[] levels;
    List<Enemy> enemies; //Svi neprijatelji u dometu
    public AudioSource audioSource;

    //Inicijalizacija
    void Start()
    {
        PlayAudio(spawnAudio);
        //Na osnovu trenutnog upgrade levela heroja, odredjujemo fireRate i pozivamo na svakih fireRate sekundi metod za ispaljivanje projektila
        InvokeRepeating("Shoot", 0.0F, getLevel().fireRate);
    }

    //Metod koji, ako ima neprijatelja u dometu, izvrsava ispaljivanje projektila na najblizeg neprijatelja
    void Shoot()
    {
        if (enemies.Count > 0)
        {
            Projectile projectile = getLevel().projectile;
            //projectile.FireProjectile(ChooseTarget(), ChooseTarget().getPosition());    Fali metod za dobijanje Vector3 pozicije neprijatelja
        }
    }

    //Update se vrsi jednom po frejmu
    void Update()
    {

    }

    //Vraca trenutni nivo
    Level getLevel()
    {
        return levels[currentLevel];
    }

    //Metod vraca maksimalni nivo upgrade-a
    Level GetMaxLevel()
    {
        return levels[levels.Length - 1];
    }

    //Postavlja nivo upgrade-a na nivo zadat indeksom
    void setLevel(int levelIndex)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[levelIndex].model != null)
            {
                if (i == levelIndex)
                {
                    levels[i].model.SetActive(true);
                }
                else {
                    levels[i].model.SetActive(false);
                }
            }
        }
        //Na osnovu trenutnog upgrade levela heroja, odredjujemo fireRate i pozivamo na svakih fireRate sekundi metod za ispaljivanje projektila
        InvokeRepeating("Shoot", 0.0F, levels[levelIndex].fireRate);
    }

    //Kad neprijatelj udje u domet heroja, dodajemo ga u listu neprijatelja u dometu
    void OnTriggerEnter2D(Collider2D other)
    {
        //Provjeravamo da li je neprijatelj narusio zonu dometa heroja
        if (other.CompareTag("Enemy"))
        {
            //Pustamo zvuk ako je lista neprijatelja prazna, tj. ulazi prvi neprijatelj u domet
            if (enemies.Count == 0)
            {
                PlayAudio(enemySpottedAudio);
            }
            enemies.Add(other.gameObject.GetComponent<Enemy>());
        }
    }

    //Kad neprijatelj izadje iz dometa heroja, brisemo ga iz liste neprijatelja u dometu
    void OnTriggerExit2D(Collider2D other)
    {
        enemies.Remove(other.gameObject.GetComponent<Enemy>());
    }

    /*Heroji uvijek napadaju neprijatelja u dometu koji je najbliži cilju. 
    Ako jedan neprijatelj pretekne onog koji je trenutno napadnut, napad će se preusmjeriti na njega.
    List<Enemy> enemies - lista treba da sadrzi sve vidljive neprijatelje na mapi
    */
    Enemy ChooseTarget () { 
        //izaberemo onog neprijatelja iz liste neprijatelja koji je najblize cilju (krsima)
        Enemy nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        foreach (Enemy enemy in enemies)
        {
            /*float dist = enemy.getDistanceFromRocks();
            if (dist < minDistance) {
                nearestEnemy = enemy;
                minDistance = dist;
            }*/
        }
        return nearestEnemy;
    }

    //Zvuk nakon nekog dogadjaja
    void PlayAudio(AudioClip clip)
    {
        Debug.Log("sound");
        audioSource.clip = clip;
        audioSource.Play();
    }

}
