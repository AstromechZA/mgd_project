﻿using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController> {
	
	public double citadelCredits;
	public int citadelLives;
	public int startingCitadelLives;
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
		startingCitadelLives = citadelLives;
	}

	public void ResetGameParameters(){
		citadelCredits = 250;
		citadelLives = 20;
		numberOfWaves = 10;
		currentWave = 0;
		enemiesSpawned = 0;
		enemiesInWave = 7;
		gameWasPaused = false;
		Time.timeScale = 1;
		currentDurationOfWave = 0;

		// Wave timers
		lengthOfWave = 25.0f;
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