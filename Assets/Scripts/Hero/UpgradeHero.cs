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
		int heroUp1Price = heroParent.GetComponent<Hero> ().GetUp1Price();
		ScoreManager.SetCoins(ScoreManager.GetCoins()-heroUp1Price); //podesi broj coina
	}
}
