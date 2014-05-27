using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController> {
	
	public double citadelCredits;
	public int citadelLives;
	public int numberOfWaves;
	public int currentWave;
	
	public float timeTillNextWave;
	public float lengthOfWave;
	public float currentDurationOfWave;
	
	public int enemiesInWave;
	public int enemiesSpawned;
	
	public GUIStyle guiStyle;
	
	public bool gameWasPaused = false;

	public bool nextWaveSpawnerActive = false;
	public bool spawnNextWaveEarly = false;

	void Awake(){
		ResetGameParameters();
	}

	public void ResetGameParameters(){
		citadelCredits = 50;
		citadelLives = 100;
		numberOfWaves = 10;
		currentWave = 0;
		enemiesSpawned = 0;
		enemiesInWave = 2;
		gameWasPaused = false;
		Time.timeScale = 1;
		currentDurationOfWave = 0;

		// Wave timers
		lengthOfWave = 5.0f;
		timeTillNextWave = 0.0f;

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