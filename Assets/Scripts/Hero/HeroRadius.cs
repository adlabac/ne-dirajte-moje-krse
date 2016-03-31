using UnityEngine;
using System.Collections;

public class HeroRadius : MonoBehaviour {

	// Use this for initialization
	void Start () {

		float radius;

		//racunamo poluprecnik na osnovu nacrtanog prefaba (sprite za domet towera)
		radius = GetComponent<SpriteRenderer> ().bounds.size.x / 2; 
		//podesavamo radius collidera u zavisnosti od skaliranja
		if (Mathf.Abs(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y)) != 0){
			GetComponent<CircleCollider2D> ().radius = radius / Mathf.Abs (Mathf.Max (transform.lossyScale.x, transform.lossyScale.y));
		}
		else {
			GetComponent<CircleCollider2D>().radius = radius;
		}
			
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
	void OnMouseUp (){
		//transform.gameObject.active = false;
	}
    
    /*
	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("Collide");
	}*/


}
