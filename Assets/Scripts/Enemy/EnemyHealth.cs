using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/*

Osnovni metodi:
Start() - Služi za inicijalizaciju health bara.
Update() - Ažuriranje health bara.
SetHealthBar(float myHealth) - metod kojim se modifikuje HealthBar

**/

public class EnemyHealth : MonoBehaviour {

	public float maxHealth = 100f; // Maksimalni HP neprijatelja
	public float currentHealth = 100f; // Trenutni HP neprijatelja
    public GameObject healthBar;
    private float normalizedHealth;

	void Start () {
        maxHealth = transform.gameObject.GetComponent<EnemyType>().initialHealth;
        currentHealth = transform.gameObject.GetComponent<Enemy>().health;
	}
	
	void Update () {
        if (currentHealth > 0) {
            currentHealth = transform.gameObject.GetComponent<Enemy>().health;
            normalizedHealth = currentHealth / maxHealth;
            SetHealthBar(normalizedHealth);
        }
        
	}

    void SetHealthBar(float myHealth)
    {
        if (healthBar != null) {
            healthBar.transform.localScale = new Vector3(Mathf.Clamp(myHealth, 0f, 1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        }
    }
}
