using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWave : MonoBehaviour 
{
	public string[] enemyTypeNames;
	public Path[] path;
	public int[] count;
	public float[] spawnDelay;
	public float[] spawnInterval;

	public EnemyType[] enemyTypes;
	private float[] spawnTime;  //niz koji sadrzi posle kog vremena treba da se spawnuju neprijatelji
	private float timer;		//broji vrijeme
	private int[] cnt; 

    private GameObject enemyParent;

	/*
	 * Postavljamo niz koji sadrzi sve 0 koje predstavljaju da EnemyType na toj poziciji u svom nizu nije krenuo sa Spawnovanjem
	 * Kada dodje njegovo vrijeme, pokrece se Coroutine, koji Spawnuje neprijatelje na pocetak svog puta, u odredjenim intervalima,
	 * do odredjenog broja neprijatelja koji su predvidjeni da se Spawnuju u toj grupi
	 * Kada se odredjena grupa neprijatelja Spawnuje, postavlja se 1 na toj poziciji u nizu cnt, i to oznacava da se vec jednom pozvao Coroutine
	 * za Spawnovanje te grupe neprijatelja, i da smo sa njom zavrsili
	 */
	void Start () 
	{
        /*
        enemyParent = GameObject.Find("Enemies");
        if (enemyParent == null)//ako u hijerarhiji nema GameObject-a Enemies, kreiraj ga
        {
            //ovdje kreiramo GameObject sa nazivom Enemies i to je enemyParent
            enemyParent = new GameObject("Enemies");
        } */

        cnt = new int[spawnDelay.Length];
        spawnTime = new float[spawnDelay.Length];
        timer = 0;
        
		for (int i = 0; i < spawnDelay.Length; i++) 
		{
			cnt [i] = 0;
			if (i == 0) 
				spawnTime [0] = spawnDelay [0];
			else if (i > 0)
				spawnTime [i] = spawnTime [i - 1] + spawnDelay [i];
		}

		//AssignEnemyTypes(enemyTypes, enemyTypeNames); //ne znam da li je trebalo u startu da se dodijele atributi nizu enemyTypes preko imena
	}
	/*
	 * U Update postavljamo timer koji mjeri vrijeme predjeno
	 * Mi smo napravili niz koji oznacava u kojoj sekundi bi trebala koja grupa da krene sa Spawnovanjem
	 * Ako imamo niz spawnDelay tipa {0,4,2,3,0}, sto znaci da se prva grupa Spawnuje odmah, sledeca za 4 sekunde, pa za 2 sekunde sledeca i tako
	 * Postavljamo novi niz spawnTime koji u ovom slucaju bi trebao da glasi {0,4,6,9,9} sto znaci da ce prva grupa krenuti sa Spawnovanjem odmah,
	 * druga grupa ce krenuti u 4. sekundi, treca u 6., cetvrta u 9. i peta takodje u 9. sekundi
	 * 
	 * Krecemo se kroz niz i gledamo koji je clan niza cije je vrijeme za Spawnovanje proslo i koji se jos nije Spawnovao
	 * gledajuci u niz cnt[] koji na pocetku ima sve 0
	 */
	void Update () 
	{
		timer += Time.deltaTime;

		for (int j = 0; j < spawnTime.Length; j++) 
		{
			if (timer >= spawnTime [j] && cnt[j]==0) 
			{
				StartCoroutine (SpawnEnemy(enemyTypes[j], count[j], spawnInterval[j], path[j]));
				cnt[j] = 1;
			}
		}
	}

	/*	Na osnovu niza enemyTypeNames treba dodijeliti vrijednosti atributima enemyTypes upotrebom metode GetByName*/
    /*
    public void AssignEnemyTypes(EnemyType[] enemyTypes, string[] enemyTypeNames)
	{
        
		for (int i = 0; i < enemyTypeNames.Length; i++) 
		{
			//enemyTypes [i] = EnemyTypes.GetByName (enemyTypeNames[i]);  //Ovdje bi vjerovatno trebao da se napravi konstruktor SetEnemyType
           
			enemyTypes[i].name = FindObjectOfType<EnemyTypes>().GetByName(enemyTypeNames[i]).name;
            enemyTypes[i].defaultSpeed = FindObjectOfType<EnemyTypes>().GetByName(enemyTypeNames[i]).defaultSpeed;
            enemyTypes[i].initialHealth = FindObjectOfType<EnemyTypes>().GetByName(enemyTypeNames[i]).initialHealth;
            enemyTypes[i].slowdownFactor = FindObjectOfType<EnemyTypes>().GetByName(enemyTypeNames[i]).slowdownFactor;
            enemyTypes[i].reward = FindObjectOfType<EnemyTypes>().GetByName(enemyTypeNames[i]).reward;
            enemyTypes[i].minStones = FindObjectOfType<EnemyTypes>().GetByName(enemyTypeNames[i]).minStones;
            enemyTypes[i].maxStones = FindObjectOfType<EnemyTypes>().GetByName(enemyTypeNames[i]).maxStones;
            enemyTypes[i].model = Instantiate(FindObjectOfType<EnemyTypes>().GetByName(enemyTypeNames[i]).model) as GameObject;

		}
	}
	*/ 

	/*
	 * Postavlja se Coroutine za Spawnovanje neprijatelja kojem se kao parametri predaju tip neprijatelja, broj koliko takvih neprijatelja
	 * treba da se Spawnuje, koji je interval po kojima trebaju da se Spawnuju neprijatelji, kao i njihova putanja koja im je dodijeljena
	 * za njihovo kretanje
	 */
    
	IEnumerator SpawnEnemy(EnemyType enemyType, int count, float spawnInterval, Path path)
	{
        int cnt = 0;
		while(cnt < count)
		{
            GameObject enemy = Instantiate(enemyType, path.wayPoints[0], Quaternion.identity) as GameObject;
            //enemy.transform.parent = enemyParent.transform;//ovo uveo zbog sredjivanja Unity hijerarhije,smjestamo sve neprijatelje u Enemies GameObject
			cnt++;
			yield return new WaitForSeconds(spawnInterval);
		}
	}

}
