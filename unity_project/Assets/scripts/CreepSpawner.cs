using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepSpawner : MonoBehaviour {

	public AudioClip sound_countdown;
	public AudioClip sound_ready;
	public AudioClip sound_go;
	private bool played_countdown = false;
	private bool played_ready = false;
	public GameObject citadelObject = null;
	
	// Different types of enemies
	public enum EnemyTypes
	{
		Easy,
		Medium,
		Hard,
		Boss
	}

	// Store Boss enemies bounty value and starting health value
	public int bountyValue;
	public float startingHealth;

	// Enemy Prefabs
	public GameObject easyEnemy;
	public GameObject mediumEnemy;
	public GameObject hardEnemy;
	
	public GameObject groupOfEnemies;
	
	public GameObject bossEnemy;
	
	private Dictionary<EnemyTypes, GameObject> enemies = new Dictionary<EnemyTypes, GameObject>(4);
	
	// Enemies
	private int totalEnemies;
	private int numEnemies = 0;
	private int spawnedEnemies = 0;
	
	// States
	private bool wave = false;
	public bool spawn = true;
	
	// Wave timers
	private float lengthOfWave;
	private float currentDurationOfWave;
	
	//Waves
	private int totalWaves;
	private int numWaves;
	
	// Spawn Interval
	public float spawnInterval = 1f;
	private float timeToSpawn = 0f;
	
	private float chanceToSpawnCluster = 0.1f;
	
	// Boss enemy spawned after a number of enemies have been spawned (randomly selected between 70% of the enemies and the total enemies)
	private int bossEnemySpawnPosition = 0;
	private bool bossSpawned = false;

	public GameObject waveSpawner;


	public void Reset()
	{
		numWaves = GameController.Instance.currentWave;
		totalWaves = GameController.Instance.numberOfWaves;
		totalEnemies = GameController.Instance.enemiesInWave;
		spawnedEnemies = GameController.Instance.enemiesSpawned;
		lengthOfWave = GameController.Instance.lengthOfWave;
		currentDurationOfWave = GameController.Instance.currentDurationOfWave;
		bossEnemySpawnPosition = Random.Range((int)(totalEnemies*0.7f), totalEnemies);
	}

	void Start()
	{
		// Add different enemies and their associated difficulty level to the enemies dictionary
		enemies.Add(EnemyTypes.Easy, easyEnemy);
		enemies.Add(EnemyTypes.Medium, mediumEnemy);
		enemies.Add(EnemyTypes.Hard, hardEnemy);
		enemies.Add(EnemyTypes.Boss, bossEnemy);
		
		numWaves = GameController.Instance.currentWave;
		totalWaves = GameController.Instance.numberOfWaves;
		totalEnemies = GameController.Instance.enemiesInWave;
		spawnedEnemies = GameController.Instance.enemiesSpawned;
		lengthOfWave = GameController.Instance.lengthOfWave;
		currentDurationOfWave = GameController.Instance.currentDurationOfWave;
		bossEnemySpawnPosition = Random.Range((int)(totalEnemies*0.7f), totalEnemies);
	}
	
	void Update ()
	{
		// Update Game variables
		GameController.Instance.enemiesInWave = totalEnemies;
		GameController.Instance.enemiesSpawned = spawnedEnemies;
		GameController.Instance.currentWave = numWaves;
		GameController.Instance.numberOfWaves = totalWaves;
		GameController.Instance.lengthOfWave = lengthOfWave;
		GameController.Instance.currentDurationOfWave = currentDurationOfWave;
		GameController.Instance.timeTillNextWave = lengthOfWave-currentDurationOfWave;

		if (GameController.Instance.spawnNextWaveEarly && !played_countdown){
			played_countdown = true;
			AudioSource.PlayClipAtPoint (sound_go, Camera.main.transform.position);
			currentDurationOfWave = GameController.Instance.lengthOfWave;
			GameController.Instance.enemiesSpawned = 0;
			GameController.Instance.spawnNextWaveEarly = false;
		}
		else{

			GameController.Instance.currentDurationOfWave = currentDurationOfWave;
			GameController.Instance.timeTillNextWave = lengthOfWave-currentDurationOfWave;
		}

		if (GameController.Instance.currentWave == 0 && !played_ready && GameController.Instance.timeTillNextWave >= 24.0F ) {
			played_ready = true;
			AudioSource.PlayClipAtPoint (sound_ready, Camera.main.transform.position);
		}


		if (GameController.Instance.timeTillNextWave <= 4.00F && !played_countdown) {
			played_countdown = true;
			AudioSource.PlayClipAtPoint (sound_countdown, Camera.main.transform.position);
		}
		

		// Update the position of the Boss enemy (Total Enemies changes)
		bossEnemySpawnPosition = Random.Range((int)(totalEnemies*0.7f), totalEnemies);
		
		if(spawn)
		{
			if(numWaves <= totalWaves)
			{
				// Increases the timer to allow the timed waves to work
				currentDurationOfWave += Time.deltaTime;
				if (wave)
				{
					timeToSpawn -= Time.deltaTime;
									
					if (timeToSpawn <= 0f){
						if (citadelObject!=null){
							// Spawn Boss enemy
							if (bossEnemySpawnPosition==spawnedEnemies && bossSpawned==false){
								spawnSingleEnemy(EnemyTypes.Boss);
								bossSpawned = true;
							}
							// Spawn Cluster of enemies
							else if (chanceToSpawnCluster >= Random.Range(0f,1f)){
								spawnPackOfEnemies();
							}
							// Spawn single enemey
							else{
								spawnSingleEnemy(getEnemyDifficulty());
							}
						}
						timeToSpawn = spawnInterval;
					}
				}
				// checks if the time is equal to the time required for a new wave
				if (currentDurationOfWave >= lengthOfWave)
				{

					// Destroy the wave spawner if there is one
					if (GameObject.Find("WaveSpawner")){
						Destroy(GameObject.Find("WaveSpawner"));
						GameController.Instance.nextWaveSpawnerActive = false;
					}

					// Lets countdown sound be played again
					played_countdown = false;
					// Enables next wave
					wave = true;
					// sets the time back to zero
					currentDurationOfWave = 0.0f;
					// increases the number of waves
					numWaves++;
					//Ensure numEnemies is 0 (spawn right amount of enemies on new wave)
					numEnemies = 0;
					
					//Reset spawned enemies
					spawnedEnemies = 0;
					
					// Modify the chance to spawn a cluster (Increase to a point as waves increase)
					chanceToSpawnCluster = Mathf.Clamp (chanceToSpawnCluster,chanceToSpawnCluster*(1+numWaves/totalWaves), 0.6f);
					
					lengthOfWave += numWaves*10f;
					
					// Reset Boss
					bossSpawned = false;
					
					// Increase Bosses Health and bounty by 30 percent
					bountyValue = (int)(bountyValue*1.3f);
					startingHealth = startingHealth*1.3f;
					enemies[EnemyTypes.Boss].GetComponent<AbstractCreep>().bountyValue = bountyValue;
					enemies[EnemyTypes.Boss].GetComponent<AbstractCreep>().startingHealth = startingHealth;

				}
				if(numEnemies >= totalEnemies)
				{
					// Disable the wave
					wave = false;

					// Create Wave Spawner
					if (!GameController.Instance.spawnNextWaveEarly && !GameController.Instance.nextWaveSpawnerActive && numWaves < totalWaves){
						GameObject waveSpawner2 = Instantiate(waveSpawner) as GameObject;
						waveSpawner2.name = "WaveSpawner";
					}
				}
			}
			else
			{
				spawn = false;
			}
		}
	}
	
	// Spawn single enemy
	private void spawnSingleEnemy(EnemyTypes enemyLevel){
		GameObject Enemy = Instantiate(enemies[enemyLevel], gameObject.transform.position, Quaternion.identity) as GameObject;
		AstarAI creepAI = Enemy.GetComponent<AstarAI>();
		creepAI.navigateToTarget(citadelObject);
		numEnemies++;
		spawnedEnemies++;
	}
	
	// Use this for a Prefab which contains multiple enemies
	private void spawnEnemyCluster(EnemyTypes enemyLevel)
	{
		GameObject Enemy = Instantiate(enemies[enemyLevel]) as GameObject;
		foreach (Transform child in Enemy.transform)
		{
			AstarAI creepAI = child.GetComponent<AstarAI>();
			creepAI.navigateToTarget(citadelObject);
			//child is your child transform
			
			numEnemies++;
			spawnedEnemies++;
		}
		// Account for the extra 4 enemies
		totalEnemies += 4;
	}
	
	// Spawn a pack of enemies
	// Formation:
	//		*
	//	*	*	*
	//		*
	private void spawnPackOfEnemies(){
		Vector3 spawnPos = gameObject.transform.position;
		
		GameObject Enemy1 = Instantiate (enemies[getEnemyDifficulty()], spawnPos, Quaternion.identity) as GameObject;
		AstarAI creepAI1 = Enemy1.GetComponent<AstarAI> ();
		creepAI1.navigateToTarget(citadelObject);
		
		GameObject Enemy2 = (GameObject)Instantiate (enemies[getEnemyDifficulty()], new Vector3(spawnPos.x+10, spawnPos.y, spawnPos.z), Quaternion.identity);
		AstarAI creepAI2 = Enemy2.GetComponent<AstarAI> ();
		creepAI2.navigateToTarget(citadelObject);
		
		GameObject Enemy3 = (GameObject)Instantiate (enemies[getEnemyDifficulty()], new Vector3(spawnPos.x-16, spawnPos.y, spawnPos.z), Quaternion.identity);
		AstarAI creepAI3 = Enemy3.GetComponent<AstarAI> ();
		creepAI3.navigateToTarget(citadelObject);
		
		GameObject Enemy4 = (GameObject)Instantiate (enemies[getEnemyDifficulty()], new Vector3(spawnPos.x, spawnPos.y, spawnPos.z+15), Quaternion.identity);
		AstarAI creepAI4 = Enemy4.GetComponent<AstarAI> ();
		creepAI4.navigateToTarget(citadelObject);
		
		GameObject Enemy5 = (GameObject)Instantiate (enemies[getEnemyDifficulty()], new Vector3(spawnPos.x, spawnPos.y, spawnPos.z-3), Quaternion.identity);
		AstarAI creepAI5 = Enemy5.GetComponent<AstarAI> ();
		creepAI5.navigateToTarget(citadelObject);
		
		// Increase the total number of enemies spawned and the number of spawned enemies
		numEnemies+=5;
		spawnedEnemies+=5;
		
		// Account for the extra 4 enemies
		totalEnemies += 4;
	}
	
	// Get a semi-randomly generated Enemy Difficulty determined by the current and number of waves
	private EnemyTypes getEnemyDifficulty(){
		
		int range = Random.Range(numWaves, numWaves+1);
		float range2 = Random.Range(0f,1f);
		
		// 20% difficulty offset
		float difficultyOffset = 0.2f;
		
		if (range > totalWaves*0.8) {
			if (range2 < difficultyOffset){
				return EnemyTypes.Medium;
			}
			else{
				return EnemyTypes.Hard;
			}
			
		} else if (range > totalWaves*0.5) {
			if(range2 < difficultyOffset){
				return EnemyTypes.Hard;
			}
			else{
				return EnemyTypes.Medium;
			}
		} else {
			if (range2 < difficultyOffset){
				return EnemyTypes.Medium;
			}
			else{
				return EnemyTypes.Easy;
			}
		}
	}
	
}
