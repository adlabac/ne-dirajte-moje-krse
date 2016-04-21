using UnityEngine;
using System.Collections;

public class HeroField : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnMouseUp ()
	{
		if (transform.parent.gameObject.tag == "Heroes") 
		{
			//trazimo child od kliknutog heroja
			GameObject visibleRadius = transform.parent.FindChild ("HeroRadius").gameObject;
			//GameObject visibleRadius = transform.Find ("HeroRadius").gameObject;

			//ako se vidi radijus, onda se samo ugasi
			if (visibleRadius.activeSelf == true)
				visibleRadius.SetActive (false);
			//ako se ne vidi, bitno je da se svim drugima ugasi i da se ovdje upali
			else {
				GameLevel.setHeroRadiusesInactive ("Heroes","HeroRadius","HeroMenus");
				visibleRadius.SetActive (true);
			}
		} 
		if (transform.parent.gameObject.tag == "FemaleHeroes") 
		{
			//trazimo child od kliknutog heroja
			GameObject visibleRadius = transform.parent.FindChild("FemaleHeroRadius").gameObject;

			//ako se vidi radijus, onda se samo ugasi
			if (visibleRadius.activeSelf == true)
				visibleRadius.SetActive (false);
			//ako se ne vidi, bitno je da se svim drugima ugasi i da se ovdje upali
			else {
				GameLevel.setHeroRadiusesInactive ("FemaleHeroes","FemaleRadius","HeroMenus");
				visibleRadius.SetActive (true);
			}
		}

	}
}
