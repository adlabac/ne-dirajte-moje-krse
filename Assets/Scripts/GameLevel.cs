using UnityEngine;
using System.Collections;


public class GameLevel : MonoBehaviour {


	EnemyWave[] enemyWaves; //niz enemyWaveova - svaki sledeci bi trebalo da bude jaci
	Path[] paths; //niz mogucih putanja kuda se mogu kretati enemyji

	public int totalStones; //ukupni broj kamenja na nivou
	public int startingCoins; //ukupan broj coina na pocetku
	public int totalWaves; //!!!! NISAM SIGURAN DA OVO JE OVO POTREBNO
	//broj protivnickih napada mozemo izvuci iz duzine niza enemyWaves

	public int waveNumber; //promjenljiva u koju ucitavamo redni broj talasa neprijatelja

	public GameObject spotPrefab; //prefab za colider za polje na nivou
	private GameObject spot;

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
		//ali ubuduce, ovu proceduru mozemo koristiti za ucitavanje
	}



	// Use this for initialization
	void Start () {
		GameObject stones;
		GameObject coins;

		//************** 1. INICIJALIZACIJA ZA SCORE MANAGER ********************
		//oba ova dijela idu preko ScoreManagera
		//u ovom dijelu bi trebalo inicijalizovati broj kamenja u odnosu na pocetni totalStones
		stones = GameObject.Find ("Stones");
		//u ovom dijelu bi trebalo inicijalizovati broj novcica u odnosu na pocetni startingCoins
		coins = GameObject.Find ("Coins");
		//prvi naredni red je zakomentarisan, jer nemamo niz
		//totalWaves = enemyWaves.Length;
		waveNumber = 1;
		//nakon ovoga - u score manageru prikazati i broj talasa ("Waves - 1/6")

		//************** 2. INICIJALIZACIJA POLJA NA MAPI ********************
		//ucitavati iz matrice i u zavisnosti od toga postavljati nevidljive collidere
		//		1 - tower polje
		//		0 - nedostupno polje (path ili zabranjena polja)
		//ideja je da se level ucitava preko matrice 18 x 10 koja se sastoji od nula i jedinica
		//za svaku jedinicu znaci da se moze postaviti tower

		LoadMatrix(); // prvo ucitavamo matricu


		//Debug.Log(stones.name);


		//petlja za crtanje colidera na dostpunim poljima
		//kad saznamo rezoluciju igracemo se sa brojkama ispod
		//umjesto ovog dijela, imamo jedan veliki collider i placeTower ce ici preko pozicije
		/*
		for (int i = 0; i < fieldAvailable.GetLength(0); i++) {
			for (int j = 0; j < fieldAvailable.GetLength(1); j++) {
				if (fieldAvailable [i,j] == 1) { //samo ako je jedinica
					Instantiate (spotPrefab, new Vector3 (-8.26F + j * 0.97F, 4.36F - i * 0.97F, 0), Quaternion.identity);
				}
			}
		}
		*/
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
