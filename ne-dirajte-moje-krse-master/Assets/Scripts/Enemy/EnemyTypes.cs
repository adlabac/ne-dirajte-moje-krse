/*
 * 
 * @author: Nikola Darmanovic
 *
*/

using UnityEngine;
using System.Collections.Generic; //Obavezno ukljucivanje Generic-a ovdje da ne bi imali problem sa razlicitim vrstama elemenata liste

public class EnemyTypes : MonoBehaviour {


    public List<EnemyType> enemyType; //Lista koja sadrzi elemente tipa EnemyType
   
    //Test Mode
    /*
    EnemyType a1 = new EnemyType();
    EnemyType a2 = new EnemyType();
    EnemyType r = new EnemyType();
    */
    
    void Start () {

        //Test Metode EnemyType GetByName(string name):
        /*
        a1.name = "darman";
        a2.name = "andrej";
        enemyType.Add(a1);
        enemyType.Add(a2);
        
        r = GetByName("darman");
        Debug.Log(r.name);
        */

	}
	
	void Update () {
	    
	}

    //Vraca element tipa EnemyType kojeg zovemo samo po imenu
    public EnemyType GetByName(string name)
    {
        return enemyType.Find(x => x.name == name);
    }
    
}
