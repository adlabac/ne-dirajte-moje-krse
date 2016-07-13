using UnityEngine;
using System.Collections;

public class Levels {


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


	private static EnemyType e01;
	private static EnemyType e02;




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
	 
	//wave 1
	private static string[] names = { "tefds", "tesr3"};
	private static Path[] paths = { };
	private static int[] cnt = { 2, 1 };
	private static float[] del = { 0.4f, 0.5f };
	private static float[] inter = { 1, 1 };
	private static EnemyType[] et = {};

	//private static EnemyWave w01 = new EnemyWave (names, paths, cnt, del, inter, et);

	//private static EnemyWave[] waves01 = {w01};




	/*

	public EnemyWave (string[] v_names, Path[] v_path, int[] v_count, float[] v_delay,
		float[] v_spawn, EnemyType[] v_enemies)
	{
		enemyTypeNames = v_names;	
		path = v_path;
		count = v_count;
		spawnDelay = v_delay;
		spawnInterval = v_spawn;
		enemyTypes = v_enemies;
	}

	*/


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

	//funkcija koja vraca brojstartnih kamenja - argument je broj nivoa
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


}
