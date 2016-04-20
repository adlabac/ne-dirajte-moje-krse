using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *Logika funkcionisanja kletve je da u ovom kontretnom primjeru imamo slowDownFactor za koji cemo umanjit brzinu kretanja neprijatelja i slowDownDuration 
 *samo vrijeme trajanje tog usporavanja(postavljeni su na 1.5 factor i 1.5 sekundi da traje u ovom primjeru)
 *U animaciji sam stavio da duzina trajanja animacija lelekanja traje 1.5 sekundi(koliko traje i slowDownDuration), smatram da te dvije promjenjive moraju
 *da budu iste
 *Preko brojaca i Update funckije sam omogucio da se InvokeRepeating pozove samo jednom dok konstantno u okolini zene heroja postoje neprijatelji
 *cim ne postoji vise neprijatelja u blizini, brojac se postavlja na 0 i zaustavlja se InvokeRepeating
 *Kako se brojac postavlja na 0, omogucava se da prilikom sledeceg wave koji dodje u blizini zene heroja, opet se pozove InvokeRepeating(Shout) na svake 3s
 *lelekanje animacija sam stavio da ima exit time i da se zavrsava na 1.5s
 */
public class FemaleHero : MonoBehaviour 
{
	int currentLevel = 0;//trenutni level heroja
	List<Enemy> enemies; //Svi neprijatelji u dometu

	int brojac; //sluzi za kontrolisanje InvokeRepeating u Update()

	private Animator anim;

	public float radius;//u inspektoru podesimo radijus
	public Color radiusColor;//inicijalna boja radijusa

	public float slowDownFactor;
	public float slowDownDuration;

	void Start () 
	{
		brojac = 0;
		enemies = new List<Enemy>();//u pocetku nema neprijatelja koje enemy moze da dohvati

		radius = transform.Find ("FemaleHeroRadius").GetComponent<SpriteRenderer> ().bounds.size.x / 2;

		anim = GetComponent<Animator> ();
		anim.SetBool ("lelekanje", false);

		//podesavamo radius collidera
		if (Mathf.Abs(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y)) != 0){
			GetComponent<CircleCollider2D>().radius = radius / Mathf.Abs(Mathf.Max(transform.lossyScale.x, transform.lossyScale.y));
		}
		else {
			GetComponent<CircleCollider2D>().radius = radius;
		}

		//InvokeRepeating("Shout", 0.0F, 2.0f);
	}
	

	void Update()
	{

		//Kasnije ce biti azurirano
		if (enemies.Count != 0)
		{
			if (brojac == 0) {		//ako prije nismo pozivali Invoke brojac je na 0
				InvokeRepeating ("Shout", 0.3f, 3.0f);	//ako su u blizini neprijatelji pozivaj na 3 sekunde Shout, sa malim zakasnjenjem od 0.3sek
				brojac++;			//postavljamo brojac na 1 dok svi protivnici ne izadju iz kruga zene(da ne bi vise puta pozivali InvokeRepeating)
			} else {
				anim.SetBool ("lelekanje", false);		//ako smo vec pozvali InvokeRepeating a protivnici su i dalje u blizini, postavi brojac na 1 da ne bi opet pozvali InvokeRepeating
			}
			Rotation();
		}
		if (enemies.Count == 0) 
		{
			radiusColor = Color.green;
			anim.SetBool ("lelekanje", false);
			CancelInvoke ();	//kada citav wave neprijatelja izadje iz kruga zene, zaustavi InvokeRepeating
			brojac = 0;			//postavljamo brojac na 0 kako bi opet prilikom upada neprijatelja novog u krug zene, pozvali InvokeRepeating
		}
	}

	void OnMouseUp ()
	{
		GameLevel.setFemaleHeroRadiusesInactive ();
	}


	Enemy ChooseTarget () { 
		//izaberemo onog neprijatelja iz liste neprijatelja koji je najblize cilju (kamenju)
		Enemy nearestEnemy = null;
		float minDistance = Mathf.Infinity;
		foreach (Enemy enemy in enemies)
		{
			if (enemy.tag == "Enemies") { 
				float dist = enemy.GetDistanceFromRocks();
				if (dist < minDistance)
				{
					nearestEnemy = enemy;
					minDistance = dist;
				}
			}
		}
		return nearestEnemy;
	}

	void Rotation()
	{
		if (ChooseTarget() != null) 
		{
			Vector3 moveDirection = gameObject.transform.position - ChooseTarget().transform.position;
			if (moveDirection != Vector3.zero)
			{
				float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 90f;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			}
		}

	}


	//Napomena: Svaki heroj ima coolider koji predstavlja domet(poluprecnik) u kom on moze da ispali projektil
	void OnTriggerEnter2D(Collider2D other) // ovo other je objekat koji ima kolider i nalazi se u dometu kolidera Heroja
	{
		if (other.CompareTag("Enemies"))//ako objekat other ima Tag sa nazivom Enemy(Unity-u za Enemy treba postaviti da ima tag Enemy)
		{
			radiusColor = Color.red;
			if (enemies.Count == 0) //Pustamo zvuk ako je lista neprijatelja prazna, tj. ulazi prvi neprijatelj u domet
			{
				//PlayAudio(enemySpottedAudio);
			}

			enemies.Add(other.gameObject.GetComponent<Enemy>());//dodamo u listu enemies neprijatelja koji je usao u domet heroja
			enemies[enemies.Count - 1].SetDetectedF(this);
		}
	}


	void OnTriggerExit2D(Collider2D other)
	{
		Enemy enemyLeftRadius = other.gameObject.GetComponent<Enemy>();
		enemyLeftRadius.UnsetDetectedF(this);
		enemies.Remove(enemyLeftRadius);//brisemo iz liste enemies neprijatelja koji je izasao iz dometa heroja
	}


	void Shout()
	{
		if (enemies.Count > 0) { //ako ima neprijatelja u dometu Heroja
			anim.SetBool ("lelekanje", true);
			for (int i = 0; i < enemies.Count; i++) {
				enemies [i].Slowdown (slowDownFactor, slowDownDuration); //usporavanje neprijatelja svih u dometu sa slowDownFactor za vrijeme od slowDownDuration
			}
		} 
	}



	void OnDrawGizmos() {
		Gizmos.color = radiusColor;
		Gizmos.DrawWireSphere(transform.position , radius);
	}

	public List<Enemy> GetEnemies() {
		return enemies;
	}

	public void RemoveEnemy(Enemy enemy) {
		enemies.Remove(enemy);
	}
}
