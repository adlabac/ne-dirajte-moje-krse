using UnityEngine;
using System.Collections;

public class ChooseHero : MonoBehaviour {

	public GameObject fieldMenuPrefab;
	private GameObject fieldMenu;
	//private Hero heroScript;

	public float levelWidth;
	public float levelHeight;
	public float fieldHeight;
	public float fieldWidth;
	public int row;
	public int col; 

	private Vector3 placePoint;

	// Use this for initialization
	void Start () {
		//trazimo sirinu i visinu pozadinske slike
		GameObject bckgImage = GameObject.Find ("Level Background");

		levelWidth = bckgImage.GetComponent<SpriteRenderer> ().bounds.size.x;
		levelHeight = bckgImage.GetComponent<SpriteRenderer> ().bounds.size.y;

		row = GameLevel.GetMatrixRows(); //broj vrsta
		col = GameLevel.GetMatrixCols(); //broj kolona

		//racunamo sirinu i visinu polja - ne mora biti uvijek sirina=visina (sada jeste)
		fieldHeight = levelHeight / row;
		fieldWidth = levelWidth / col;

	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseUp (){
		//trazimo poziciju klika na slici
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
		//ako je doslo do hita sa backgroundom
		if (hit) {
			
			//sad cemo da napravimo vektor kojim postavljamo koordinate sistema 
			//u donji lijevi ugao, a ne u centru ekrana
			Vector3 coord = new Vector3 (levelWidth/2, levelHeight/2,0); //vektor za dodavanje
			Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y,0) + coord; //tacka dodira u novom koordinatnom sistemu

			//sada racunamo vrstu i kolonu (da bi radili sa matricom)
			//sada imamo poziciju polja u matrici
			int rowClicked = Mathf.FloorToInt(hitPoint.y / levelHeight * row); //broj vrste - klik
			int colClicked = Mathf.FloorToInt(hitPoint.x / levelWidth * col); //broj kolone - klik
            GameLevel.setHeroRadiusesInactive ("Heroes", "HeroRadius", "HeroMenus");

			//ispitujemo je li polje available
			if (canPlaceTower (rowClicked, colClicked)) {
				//stavljamo kao z=-1f da bi radius bio u pozadini
				placePoint = new Vector3 (colClicked * fieldHeight + fieldHeight/2,  
					rowClicked * fieldWidth + fieldWidth/2, 0f); //pravimo pocetnu tacku u nasem koord sistemu
				placePoint -= coord; //oduzimamo vektor da bi dobili prave koordinate
				//postavljamo tower na mjestu unutar odgovoarajuceg kvadratica

				//otvaranje menija sa herojima
				GameObject menuParent = GameObject.Find("HeroMenus");
				//ako u hijerarhiji nema GameObject-a HeroMenus, kreiraj ga
				if (menuParent == null){
					//ovdje kreiramo GameObject sa nazivom HeroMenus
					menuParent = new GameObject("HeroMenus");
				}
				Vector3 placePointMenu = placePoint + new Vector3 (0, 0, -1f);
				fieldMenu = (GameObject)Instantiate (fieldMenuPrefab, placePointMenu, Quaternion.identity);
				fieldMenu.transform.parent = menuParent.transform;
			}

		}

	}

	//funkcija koja provjerava da li na polju vec postoji tower
	private bool canPlaceTower(int i, int j) {
		return GameLevel.IsAvailable (i, j);
	}


}
