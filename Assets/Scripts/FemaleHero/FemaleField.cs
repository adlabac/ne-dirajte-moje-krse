using UnityEngine;
using System.Collections;

public class FemaleField : MonoBehaviour 
{

	void OnMouseUp ()
	{

		//trazimo child od kliknutog heroja
		GameObject visibleRadius = transform.parent.FindChild("FemaleHeroRadius").gameObject;

		//ako se vidi radijus, onda se samo ugasi
		if (visibleRadius.activeSelf == true)
			visibleRadius.SetActive(false);
		//ako se ne vidi, bitno je da se svim drugima ugasi i da se ovdje upali
		else {
			//GameLevel.setHeroRadiusesInactive ();
			visibleRadius.SetActive (true);
		}

	}
}
