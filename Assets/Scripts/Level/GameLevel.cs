using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevel : MonoBehaviour {

	public static int levelNo = 1;

	public static int totalStones; //ukupni broj kamenja na nivou
	public static int startingCoins; //ukupan broj coina na pocetku
	public static int[,] fieldAvailable; //matrica nivoa
	public static int waveNumber; //promjenljiva u koju ucitavamo redni broj talasa neprijatelja

	public GameObject fieldMenuPrefab;
	public GameObject fieldMenuPrefab2;
	private GameObject spriteBckg;
	private Vector3 placePoint;

	public float levelWidth;
	public float levelHeight;
	public float fieldHeight;
	public float fieldWidth;
	public int row;
	public int col;


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

		//crtanje pozadine
		DrawBackground ();

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


	public void DrawBackground(){
		GameObject bckgImage = GameObject.Find ("Level Background");

		//duzina i sirina pozadine
		levelWidth = bckgImage.GetComponent<SpriteRenderer> ().bounds.size.x;
		levelHeight = bckgImage.GetComponent<SpriteRenderer> ().bounds.size.y;

		row = fieldAvailable.GetLength (0); //broj vrsta
		col = fieldAvailable.GetLength (1); //broj kolona

		//racunamo sirinu i visinu polja - ne mora biti uvijek sirina=visina (sada jeste)
		fieldHeight = levelHeight / row;
		fieldWidth = levelWidth / col;

		Vector3 coord = new Vector3 (levelWidth / 2, levelHeight / 2, 0); //vektor za dodavanje


		for (int i = 0; i < row; i++) {



			for (int j = 0; j < col; j++) {

				placePoint = new Vector3 (j * fieldHeight + fieldHeight / 2,  
					i * fieldWidth + fieldWidth / 2, 0.5f); //pravimo pocetnu tacku u nasem koord sistemu
				placePoint -= coord; //oduzimamo vektor da bi dobili prave koordinate
				//postavljamo tower na mjestu unutar odgovoarajuceg kvadratica

				//otvaranje menija sa herojima
				GameObject menuParent = GameObject.Find ("Background");
				//ako u hijerarhiji nema GameObject-a HeroMenus, kreiraj ga
				if (menuParent == null) {
					//ovdje kreiramo GameObject sa nazivom HeroMenus
					menuParent = new GameObject ("Background");
				}
					
				Vector3 placePointMenu = placePoint + new Vector3 (0, 0, -1);

				if (fieldAvailable [i, j] == 1) {
					spriteBckg = (GameObject)Instantiate (fieldMenuPrefab, placePointMenu, Quaternion.identity);
					spriteBckg.transform.parent = menuParent.transform;
				} else if (fieldAvailable [i, j] == 0) {
					spriteBckg = (GameObject)Instantiate (fieldMenuPrefab2, placePointMenu, Quaternion.identity);
					spriteBckg.transform.parent = menuParent.transform;
				}
			}			
		}
	}
		

}






