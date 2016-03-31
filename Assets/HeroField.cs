using UnityEngine;
using System.Collections;

public class HeroField : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnMouseUp (){

		//trazimo child od kliknutog heroja
		GameObject visibleRadius = transform.parent.FindChild("HeroRadius").gameObject;
		//GameObject visibleRadius = transform.Find ("HeroRadius").gameObject;
		GameObject[] heroes;

		//ako se vidi radijus, onda se samo ugasi
		if (visibleRadius.active==true)
			visibleRadius.active=false;
		//ako se ne vidi, bitno je da se svim drugima ugasi i da se ovdje upali
		else{
			//nadji sve heroje
			heroes = GameObject.FindGameObjectsWithTag ("Heroes");
			//svakom ugasi radius - bice samo jedan ustvari
			foreach (GameObject hero in heroes) {
				hero.transform.Find ("HeroRadius").gameObject.active = false;
			}
			visibleRadius.active=true;
		}

	}
}
