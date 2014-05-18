using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public int citadelCredits;
	public int citadelLives;
	public int numberOfWaves;
	public int currentWave;
	
	public GUIStyle guiStyle;
	
	// Create Singleton
	private static GameController gameController1;
	public static GameController gameController
	{
		get{return gameController1;}
	}
	
	// Keep this gameobject active from scene to scene 
	void Awake(){
		if (gameController1 == null)
		{
			gameController1 = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (gameController1 != this)
		{
			// Singleton already exists (destroy new one)
			Destroy(this);
		}
	}
	
	void Start(){
		setGameParameters();
	}
	
	public void setGameParameters(){
		citadelCredits = 50;
		citadelLives = 20;
		numberOfWaves = 10;
		currentWave = 1;
	}
	
}