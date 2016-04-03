using UnityEngine;
using System.Collections;

public class UpgradeHero : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseUp(){
		GameObject heroParent = transform.parent.parent.gameObject;
		int heroUpgradePrice = heroParent.GetComponent<Hero> ().GetPrice();
		if (heroUpgradePrice <= ScoreManager.GetCoins ()) {
			ScoreManager.SetCoins(ScoreManager.GetCoins()-heroUpgradePrice); //podesi broj coina
			heroParent.transform.Find("LevelNumber").GetComponent<TextMesh>().text=
				heroParent.GetComponent<Hero>().GetNextLevel();
			GameLevel.setHeroRadiusesInactive ();
		}
	}
}
