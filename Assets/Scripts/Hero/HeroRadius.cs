using UnityEngine;
using System.Collections;

public class HeroRadius : MonoBehaviour {


	private Quaternion rotRadius;

	GameObject projectileParent;//ovdje se cuvaju svi projektili koji se spawnuju

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
	void OnMouseUp (){

	}



	void Awake()
	{
		rotRadius = transform.rotation;
	}


	void LateUpdate()
	{
		transform.rotation = rotRadius;
	}

    
    /*
	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("Collide");
	}*/


}
