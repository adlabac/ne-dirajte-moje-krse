using UnityEngine;
using System.Collections;

public class FemaleRadius : MonoBehaviour 
{

	private Quaternion rotRadius;

	void Awake()
	{
		rotRadius = transform.rotation;
	}


	void LateUpdate()
	{
		transform.rotation = rotRadius;
	}
		
}
