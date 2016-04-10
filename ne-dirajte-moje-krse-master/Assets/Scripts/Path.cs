using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour 
{
	public Color pathColor = Color.green;					//Boja puta
	public List<Vector3> wayPoints = new List<Vector3>();	//Lista Vector3 koja predstavlja tacke puta

	//Za crtanje puta u Gizmosu
	void OnDrawGizmos()
	{
		Gizmos.color = pathColor;								//Postavljanje boje za crtanje puta
		
		for (int i = 0; i < wayPoints.Count; i++) 				//Prolazak kroz sve Vector3 u listi
		{
			if(i > 0)											//Provjera da li postoje vise od dva Vector3
				Gizmos.DrawLine(wayPoints[i-1],wayPoints[i]);	//Crtanje pravih linija izmedju dva susjedna Vector3(tacke)
		}
	}
}
