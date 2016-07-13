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


	public EnemyWave wave01;
	public EnemyWave wave02;

	EnemyWave[] enemyWaves; //niz enemyWaveova - svaki sledeci bi trebalo da bude jaci


	private float[] spawnTime;  //niz koji sadrzi posle kog vremena treba da se spawnuju neprijatelji
	private float timer;		//broji vrijeme
	private int[] cnt; 
    

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



		EnemyWave w = enemyWaves[0];
		cnt = new int[w.spawnDelay.Length];
		spawnTime = new float[w.spawnDelay.Length];
		timer = 0;

		for (int i = 0; i < w.spawnDelay.Length; i++) 
		{
			cnt [i] = 0;
			if (i == 0) 
				spawnTime [0] = w.spawnDelay [0];
			else if (i > 0)
				spawnTime [i] = spawnTime [i - 1] + w.spawnDelay [i];
		}

	
	}


	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		Debug.Log ("Dujo" + enemies [0].name);

		for (int j = 0; j < spawnTime.Length; j++) 
		{
			if (timer >= spawnTime [j] && cnt[j]==0) 
			{
				StartCoroutine (SpawnEnemy(enemies[1], 3, spawnTime[j], paths[0]));
				cnt[j] = 1;
			}
		}

		//waveText.text = waveNum.ToString() + " | " + count.Length.ToString();//prikaz broja wave-a	
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






