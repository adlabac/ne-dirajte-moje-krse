using UnityEngine;
using System.Collections;

public class Levels : MonoBehaviour {

	public static int[,] fieldAvailableEmpty = {
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
	};



	//***************************************************************************
	//****************************** LEVEL 1 ************************************
	//***************************************************************************
	private static int[,] fieldAvailable01 = {
		{0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
		{1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1},
		{1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1},
		{1, 1, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1},
		{1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1},
		{1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0},
		{0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 0},
		{1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0},
		{1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0}
	};
		
	private static int totalStones01= 20; //ukupni broj kamenja na nivou
	private static int startingCoins01 = 5000; //ukupan broj coina na pocetku


	public static EnemyWave wave01;

	private static EnemyWave[] waves01 = {new EnemyWave()};

	//***************************************************************************
	//*************************** END LEVEL 1 ***********************************
	//***************************************************************************

		

	//funkcija koja vraca broj pocetnih novcica - argument je broj nivoa
	public static int GetStartingCoins(int level)
	{
		if (level == 1)
			return startingCoins01;
		else
			return 0;
	}

	//funkcija koja vraca broj startnih kamenja - argument je broj nivoa
	public static int GetTotalCoins(int level)
	{
		if (level == 1)
			return totalStones01;
		else
			return 0;
	}
		
	//funkcija koja vraca matricu nivoa - argument je broj nivoa
	public static int[,] GetMatrix(int level)
	{
		if (level == 1)
			return fieldAvailable01;
		else
			return fieldAvailableEmpty;

	}
		

	public static int GetMatrixRows(int level)
	{
		if (level == 1)
			return fieldAvailable01.GetLength(0);
		else
			return 0;
	}
		
	public static int GetMatrixCols(int level)
	{
		if (level == 1)
			return fieldAvailable01.GetLength(1);
		else
			return 0;
	}


	public static EnemyWave[] GetWaves(int level){

		wave01 = new EnemyWave ();
		wave01.enemyTypeNames = new string[] {"test","asdasd"};
		wave01.pathNo = new int[] {0,0};
		wave01.count = new int[] {6,2};
		wave01.spawnDelay = new float[] {1,1};
		wave01.spawnInterval = new float[] {1,1};
		wave01.enemyTypesNo = new int[] {0,1};

		waves01 = new EnemyWave[] { wave01};




		if (level == 1)
			return waves01;
		else
			return waves01;
	}



}
