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

		Debug.Log ("Up click");
		GameObject heroParent = transform.parent.parent.gameObject;
		int heroUp1Price = heroParent.GetComponent<Hero> ().GetUp1Price();
		if (heroUp1Price <= ScoreManager.GetCoins ()) {
			ScoreManager.SetCoins(ScoreManager.GetCoins()-heroUp1Price); //podesi broj coina
			heroParent.transform.Find("LevelNumber").GetComponent<TextMesh>().text=
				heroParent.GetComponent<Hero>().GetNextLevel();
			GameLevel.setHeroRadiusesInactive ();
		}
	}
}
