/*
 * 
 * @author: Nikola Darmanovic
 * 
*/


using UnityEngine;
using System.Collections;
/*
 *  Klasa koja opisuje tipove neprijatelja
 * */

[System.Serializable]
public class EnemyType : MonoBehaviour {

    
    public string name;          //Naziv neprijatelja
    public GameObject model;     //Kreirati model ? Konstruktor??? Geteri i seteri?
    public float defaultSpeed;   //Defaultna brzina      
    public float initialHealth;  //Pocetni helti
    public float slowdownFactor; //Koliko usporava u slucaju kletve
    public int reward;    //Broj perpera koji donosi svaki ubijeni enemy  
    public int minStones; //Koliko najmanje kamenja moze enemy da odnese
    public int maxStones; //Koliko najvise kamenja moze enemy da odnese
    
    
	void Start () {
	}
	
	void Update () {
	    
	}
    
}



