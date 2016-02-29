using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class ScoreManager : MonoBehaviour
{
	private static ScoreManager instance;
	
	private ScoreManager() {}


	private static int coins=0;
	private static int stonesRemaining=0;
	private static int enemyWave=0;

	public static void SetStones(int stonesToBeSet)
	{
		//postavimo broj kamenja
		stonesRemaining = stonesToBeSet;
	}
	
	public static int GetStones()
	{
		//vraca trenutni broj kamena
		return stonesRemaining;
	}
	
	public static void SetCoins(int coinsToBeSet)
	{
		//postavimo broj novcica
		coins = coinsToBeSet;
	}
	
	public static int GetCoins()
	{
		//vraca trenutni broj novcica
		return coins;
	}
	
	public static void AddCoins(int coinsToBeAdded)
	{
		//dodajmo (moze i negativno) broj novcica na trenutnu vrijednost
		coins += coinsToBeAdded;
	}
	
	public static void AddStones(int stonesToBeAdded)
	{
		//dodajmo (moze i negativno) broj kamena na trenutnu vrijednost
		stonesRemaining += stonesToBeAdded;
	}
	
	public static void NextWave()
	{
		//uvecajmo vrijednost protivnickog talasa za jedan
		enemyWave++;
	}
	
	public static int GetWave()
	{
		//vraca vrijednost trenutnog talasa
		return enemyWave;
	}
	
	public static ScoreManager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new ScoreManager();
			}
			return instance;
		}
	}





}


