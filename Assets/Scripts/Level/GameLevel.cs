using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevel : MonoBehaviour {

	public int levelNo;

	public static int totalStones; //ukupni broj kamenja na nivou
	public static int startingCoins; //ukupan broj coina na pocetku
	public static int[,] fieldAvailable; //matrica nivoa
	public static int[,] fieldBackground; //matrica pozadine
	public static int waveNumber; //promjenljiva u koju ucitavamo redni broj talasa neprijatelja


	EnemyWave[] enemyWaves; //niz enemyWaveova - svaki sledeci bi trebalo da bude jaci
    public List<Path> paths; //niz mogucih putanja kuda se mogu kretati enemyji

	//promjenljiva u kojoj je ucitana matrica 10 x 18 sa nulama i jedinicama koje oznacavaju da li se na polju
	//moze postaviti tower - detaljnije objasnjenje u Start()


	// Inicijalizacija nivoa
	void Start () {

		//citamo podatke za nivo
		totalStones = Levels.GetTotalCoins(levelNo); 
		startingCoins = Levels.GetStartingCoins(levelNo);
		fieldAvailable = Levels.GetMatrix (levelNo);
		waveNumber = 1; //pocinje od prvog talasa

		//score manager
		ScoreManager.SetStones(totalStones);
		ScoreManager.SetCoins(startingCoins);

	}


	// Update is called once per frame
	void Update () {
	
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






