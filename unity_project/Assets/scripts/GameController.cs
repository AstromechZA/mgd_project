using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController> {
	
	public int citadelCredits;
	public int citadelLives;
	public int numberOfWaves;
	public int currentWave;
	
	public GUIStyle guiStyle;
	
	void Start(){
		ResetGameParameters();
	}
	
	public void ResetGameParameters(){
		citadelCredits = 50;
		citadelLives = 20;
		numberOfWaves = 10;
		currentWave = 1;
	}	
}