using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour {

	//ZA PODESAVANJE KROZ INSPEKTOR
	public int levelNo; //redni broj nivoa
	public GameObject[] enemies; //niz u kojem cuvamo neprijatelje
	public List<Path> paths; //niz mogucih putanja kuda se mogu kretati enemyji

	//PODACI KOJE IZVLACIMO IZ KLASE "Levels"
	public static int totalStones; //ukupni broj kamenja na nivou
	public static int startingCoins; //ukupan broj coina na pocetku
	public static int[,] fieldAvailable; //matrica nivoa
	public static int[,] fieldBackground; //matrica pozadine
	public static int waveNumber; //promjenljiva u koju ucitavamo redni broj talasa neprijatelja
	public static int waveCount;
	public static EnemyWave[] enemyWaves; //niz enemyWaveova - svaki sledeci bi trebalo da bude jaci


	private float[] spawnTime;  //niz koji sadrzi posle kog vremena treba da se spawnuju neprijatelji
	private float timer;		//broji vrijeme
	private int len;			//duzina svih podwaveova
	private int[] cnt; 			//niz u kom cuvamo da li su spawnovani svi podwaveovi
	private int[] subwaves;		//broj podwaveova po waveu
	private int wNow;			//trenutni wave
	private int swNow;			//trenutni subwave
    

	// Inicijalizacija nivoa
	void Start () {

		//citamo podatke za nivo iz "Levels"
		totalStones = Levels.GetTotalCoins(levelNo); //broj kamenja
		startingCoins = Levels.GetStartingCoins(levelNo); //pocetni broj novcica
		fieldAvailable = Levels.GetMatrix (levelNo); //matrica za postavljanje heroja
		enemyWaves = Levels.GetWaves (levelNo); //niz talasa neprijatelja
		waveNumber = 1; //pocinje od prvog talasa 
		waveCount = enemyWaves.GetLength(0); //broj talasa

		//score manager
		ScoreManager.SetStones(totalStones);
		ScoreManager.SetCoins(startingCoins);
		ScoreManager.SetWave(waveNumber,waveCount);

		//svi talasi neprijatelja
		/*
	 	 * Postavljamo niz koji sadrzi sve 0 koje predstavljaju da EnemyType na toj poziciji u svom nizu nije krenuo sa Spawnovanjem
		 * Kada dodje njegovo vrijeme, pokrece se Coroutine, koji Spawnuje neprijatelje na pocetak svog puta, u odredjenim intervalima,
		 * do odredjenog broja neprijatelja koji su predvidjeni da se Spawnuju u toj grupi
		 * Kada se odredjena grupa neprijatelja Spawnuje, postavlja se 1 na toj poziciji u nizu cnt, i to oznacava da se vec jednom pozvao Coroutine
		 * za Spawnovanje te grupe neprijatelja, i da smo sa njom zavrsili
		 */

		timer = 0;
		len = 0;
		subwaves = new int[enemyWaves.Length];
		swNow = 0;
		wNow = 0;

		//racunamo broj podtalasa po talasima
		for (int x = 0; x < enemyWaves.GetLength (0); x++) {
			EnemyWave w = enemyWaves[x];
			len += w.spawnDelay.Length;
			subwaves[x] = w.spawnDelay.Length;
		}


		cnt = new int[len]; //niz koji govori je li spawnovano
		spawnTime = new float[len]; //niz intervala izmedju
		int z=0;	


		for (int x = 0; x < enemyWaves.GetLength (0); x++) {
			for (int y = 0; y < subwaves [x]; y++) {
				cnt [z] = 0;
				if (x == 0 && y == 0)
					spawnTime [z] = enemyWaves [x].spawnDelay [y];
				else
					spawnTime [z] = spawnTime[z-1] + enemyWaves [x].spawnDelay [y];
				z++;
			}
		}
	
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
	void Update () {

		timer += Time.deltaTime;

		for (int j = 0; j < len; j++) 
		{
			if (timer >= spawnTime [j] && cnt[j]==0) 
			{
				Debug.Log ("j=" + j + " len=" + len + " st=" + spawnTime[j] + " podtalas=" + swNow);

				StartCoroutine (SpawnEnemy(enemies[enemyWaves[wNow].enemyTypesNo[swNow]], 1, spawnTime[j], paths[0]));
				cnt[j] = 1;

				if (swNow == subwaves [wNow]-1) {
					swNow = 0;
					wNow += 1;
					waveNumber = wNow + 1;
					if (waveNumber > waveCount)
						waveNumber = waveCount;
					ScoreManager.SetWave(waveNumber,waveCount);
				} else {
					swNow += 1;
				}
					


			}
		}

			
			
	}




	IEnumerator SpawnEnemy(GameObject enemyType, int count, float spawnInterval, Path path)
	{
		//waveNum++;
		int cnt = 0;
		while(cnt < count)
		{
			GameObject enemy = Instantiate(enemyType, path.wayPoints[0], Quaternion.identity) as GameObject;
			//enemy.transform.parent = enemyParent.transform;//ovo uveo zbog sredjivanja Unity hijerarhije,smjestamo sve neprijatelje u Enemies GameObject
			cnt++;
			yield return new WaitForSeconds(spawnInterval);
		}

	}




	public static int GetMatrixRows(){
		return fieldAvailable.GetLength(0);
	}

	public static int GetMatrixCols(){
		return fieldAvailable.GetLength(1);
	}
		
	public static void SetField(int i, int j, int value){
		fieldAvailable [i, j] = value;
	}

	public static bool IsAvailable(int i, int j){
		if (fieldAvailable [i, j] == 0)
			return false;
		else return true;
	}


	public static void setHeroRadiusesInactive(string tagHero, string radius, string menuHero){
		GameObject[] heroes;
		GameObject[] menus;

		heroes = GameObject.FindGameObjectsWithTag (tagHero);
		//svakom ugasi radius - bice samo jedan ustvari
		foreach (GameObject hero in heroes) {
			GameObject heroRad = hero.transform.Find (radius).gameObject;
			heroRad.SetActive (false);
			//heroRad.transform.position += new Vector3 (0, 0, 1);
		}

		menus = GameObject.FindGameObjectsWithTag (menuHero);
		foreach (GameObject menu in menus) {
			Destroy (menu);
		}
	}



}






