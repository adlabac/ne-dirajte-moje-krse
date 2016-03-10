using UnityEngine;
using System.Collections;


public class GameLevel : MonoBehaviour {


	public static int totalStones; //ukupni broj kamenja na nivou
	public static int startingCoins; //ukupan broj coina na pocetku
	public static int waveNumber; //promjenljiva u koju ucitavamo redni broj talasa neprijatelja

	EnemyWave[] enemyWaves; //niz enemyWaveova - svaki sledeci bi trebalo da bude jaci
	Path[] paths; //niz mogucih putanja kuda se mogu kretati enemyji


	//promjenljiva u kojoj je ucitana matrica 10 x 18 sa nulama i jedinicama koje oznacavaju da li se na polju
	//moze postaviti tower - detaljnije objasnjenje u Start()
	public static int[,] fieldAvailable = {
		{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
		{1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1},
		{1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1},
		{1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1},
		{1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1},
		{1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0},
		{0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0},
		{1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0},
		{1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0}
	};


	//procedura za inicijalizaciju matrice nivoa
	void LoadMatrix(){
		//inicijalizacija se vrsi direktno pri deklaraciji
		//ali ubuduce, ovu proceduru mozemo koristiti za ucitavanje iz baze ili fajla
	}



	// Use this for initialization
	void Start () {


		LoadMatrix(); // prvo ucitavamo matricu

		//************** 1. INICIJALIZACIJA ZA SCORE MANAGER ********************
		//oba ova dijela idu preko ScoreManagera

		//u ovom dijelu bi trebalo inicijalizovati broj kamenja u odnosu na pocetni totalStones
		totalStones = 20; //citati iz fajla/baze
		ScoreManager.SetStones(totalStones);


		//u ovom dijelu bi trebalo inicijalizovati broj novcica u odnosu na pocetni startingCoins
		startingCoins = 180; //citati iz fajla/baze
		ScoreManager.SetCoins(startingCoins);


		//prvi naredni red je zakomentarisan, jer nemamo niz
		//totalWaves = enemyWaves.Length;
		waveNumber = 1;
		//nakon ovoga - u score manageru prikazati i broj talasa ("Waves - 1/6")

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
}






