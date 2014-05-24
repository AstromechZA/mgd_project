using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController> {
	
	public double citadelCredits;
	public int citadelLives;
	public int numberOfWaves;
	public int currentWave;
	
	public float timeTillnextWave;
	
	public int enemiesInWave;
	public int enemiesSpawned;
	
	public GUIStyle guiStyle;
	
	public bool gameWasPaused = false;
	
	void Awake(){
		ResetGameParameters();
	}
	
	public void ResetGameParameters(){
		citadelCredits = 50;
		citadelLives = 20;
		numberOfWaves = 10;
		currentWave = 1;
		gameWasPaused = false;
		Time.timeScale = 1;
		timeTillnextWave = 20;
	}	
	
	public void DestroyAllObjectsWithTag(string tag){
		// Get an array of all the instantiated objects
		GameObject[] instantiatedObjects = GameObject.FindGameObjectsWithTag(tag);
		// Destroy them
		if (instantiatedObjects != null) {
			foreach (GameObject instantiatedObject in instantiatedObjects) {
				Destroy (instantiatedObject);
			}
		}
	}
}