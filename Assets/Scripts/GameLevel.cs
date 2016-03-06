using UnityEngine;
using System.Collections;


public class GameLevel : MonoBehaviour {



	public GameObject heroPrefab;
	private GameObject hero;

	public static int rowNum = 10; //broj redova matrice levela
	public static int colNum = 18; //broj kolona matrice levela
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
		GameObject stones;
		GameObject coins;

		//************** 1. INICIJALIZACIJA ZA SCORE MANAGER ********************
		//oba ova dijela idu preko ScoreManagera
		//u ovom dijelu bi trebalo inicijalizovati broj kamenja u odnosu na pocetni totalStones
		stones = GameObject.Find ("Stones");
		totalStones = 20; //citati iz fajla/baze
		//u ovom dijelu bi trebalo inicijalizovati broj novcica u odnosu na pocetni startingCoins
		coins = GameObject.Find ("Coins");
		startingCoins = 100; //citati iz fajla/baze
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





		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);



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






/*

using UnityEngine;
using System.Collections;

public class PlaceTower : MonoBehaviour {




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//funkcija koja provjerava da li na polju vec postoji tower
	private bool canPlaceTower(int i, int j) {
		return GameLevel.IsAvailable (i, j);
	}


	//procedura koja obradjuje dogadjaj klikom na polje na kojem
	//je podeseno da se mogu postavljati toweri u toku levela
	void OnMouseUp () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		//imamo tacnu poziciju clicka/toucha tj. dodira sa backgroundom
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		//ako je doslo do hita sa backgroundom
		if (hit) {
			//1.ispitati da li moze da se postavi tower i ako moze postaviti - OK
			//2.postaviti ga tacno u odredjenom kvadraticu - OK
			//3.update filedAvailable - OK
			//4.hijerarhija

			//trazimo sirinu i visinu pozadinske slike
			GameObject bckgImage = GameObject.Find ("Background");
			float levelWidth = bckgImage.GetComponent<SpriteRenderer> ().bounds.size.x;
			float levelHeight = bckgImage.GetComponent<SpriteRenderer> ().bounds.size.y;

			//sad cemo da napravimo vektor kojim postavljamo koordinate sistema 
			//u donji lijevi ugao, a ne u centru ekrana
			Vector2 coord = new Vector2 (levelWidth/2, levelHeight/2); //vektor za dodavanje
			Vector2 hitPoint = hit.point + coord; //tacka dodira u novom koordinatnom sistemu

			//sada racunamo vrstu i kolonu (da bi radili sa matricom)
			int row = GameLevel.GetMatrixRows(); //broj vrsta
			int col = GameLevel.GetMatrixCols(); //broj kolona
			int rowClicked = Mathf.FloorToInt(hitPoint.y / levelHeight * row); //broj vrste - klik
			int colClicked = Mathf.FloorToInt(hitPoint.x / levelWidth * col); //broj kolone - klik

			//racunamo sirinu i visinu polja - ne mora biti uvijek sirina=visina (sada jeste)
			float fieldHeight = levelHeight / row;
			float fieldWidth = levelWidth / col;


			//Debug.Log (rowClicked + " " + colClicked + " " +
			//GameLevel.GetMatrixCols);
		
			//prvo ispitujemo je li polje available
			if (canPlaceTower (rowClicked, colClicked)) {
				Vector2 placePoint = new Vector2 (colClicked * fieldHeight + fieldHeight/2,  
					rowClicked * fieldWidth + fieldWidth/2); //pravimo pocetnu tacku u nasem koord sistemu
				placePoint -= coord; //oduzimamo vektor da bi dobili prave koordinate
				//postavljamo tower na mjestu unutar odgovoarajuceg kvadratica
				tower = (GameObject)Instantiate(towerPrefab, placePoint , Quaternion.identity);
				GameObject fieldManager = GameObject.Find ("FieldManager");
				tower.transform.parent = fieldManager.transform;
				GameLevel.SetField (rowClicked, colClicked, 0); //update matrice
			}

		}




	}
}







*/
