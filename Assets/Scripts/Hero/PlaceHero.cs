using UnityEngine;
using System.Collections;

public class PlaceHero : MonoBehaviour {

	public GameObject heroPrefab;
	private GameObject hero;
	public float levelWidth;
	public float levelHeight;
	public float fieldHeight;
	public float fieldWidth;
	public int row;
	public int col; 

	// Use this for initialization
	void Start () {
		//trazimo sirinu i visinu pozadinske slike

	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseUp (){
		Vector3 placePoint = new Vector3 (transform.parent.position.x, transform.parent.position.y, -1f);
		//postavljamo tower na mjestu unutar odgovarajuceg kvadratica
		hero = (GameObject)Instantiate (heroPrefab, placePoint, Quaternion.identity);
        if (heroPrefab.tag == "Heroes")
        {
            hero.transform.Find("HeroRadius").gameObject.SetActive(false);
        }
        int heroPrice = hero.GetComponent<Hero>().GetPrice();//cijena heroja
		hero.GetComponent<Hero> ().setLevel(1);

		if (heroPrice <= ScoreManager.GetCoins ()) { //ako je cijena manja od preostalih novcica
			ScoreManager.SetCoins (ScoreManager.GetCoins () - heroPrice); //podesi broj coina
			GameObject fieldManager = GameObject.Find ("Field Manager");
			hero.transform.parent = fieldManager.transform;
			//GameLevel.SetField (rowClicked, colClicked, 0); //update matrice - zauzeto polje					
		} else //inace unisti objekat
			Destroy (hero);

		if (heroPrefab.tag == "Heroes") {
			GameLevel.setHeroRadiusesInactive ("Heroes", "HeroRadius", "HeroMenus");
		}
	}


}
