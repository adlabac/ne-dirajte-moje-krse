using UnityEngine;
using System.Collections;

public class HeroRadius : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp (){
		transform.gameObject.active = false;
	}
}
