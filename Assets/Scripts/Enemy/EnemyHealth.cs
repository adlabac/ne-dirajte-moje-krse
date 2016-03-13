using UnityEngine;
using System.Collections;

/*

Osnovni metodi:
Start() - Služi za inicijalizaciju health bara.
Update() - Ažuriranje health bara.

**/

public class EnemyHealth : MonoBehaviour {

	public float maxHealth = 100; // Maksimalni HP neprijatelja
	public float currentHealth = 100; // Trenutni HP neprijatelja
	private float healthBar; //  Health bar

	void Start () {
		healthBar = gameObject.transform.localScale.x; 	
	}
	
	void Update () {
		Vector3 tmpHealthBar = gameObject.transform.localScale; // ovo ce nam trebati da bi iznad svakog neprijatelja vizuelno mogli da vidimo trenutni HP
		tmpHealthBar.x = currentHealth / maxHealth * healthBar;
		gameObject.transform.localScale = tmpHealthBar;	
	}
}
