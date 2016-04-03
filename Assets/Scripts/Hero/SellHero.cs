using UnityEngine;
using System.Collections;

public class SellHero : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp(){
		Debug.Log ("Click sell");
		GameObject heroParent = transform.parent.parent.gameObject;
		int heroSellPrice = heroParent.GetComponent<Hero> ().GetSellPrice ();
		ScoreManager.SetCoins(ScoreManager.GetCoins()+heroSellPrice); //podesi broj coina
		Destroy(heroParent);
	}
}
