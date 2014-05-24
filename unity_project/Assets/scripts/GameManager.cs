using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	GameObject sonarTower;
	GameObject missileTower;
	GameObject gunTower;
	GameObject beamTower;
	public GameObject AIController;
	public GameObject Globals;
	
	public Texture2D creditIcon;
	public Texture2D healthIcon;
	public Texture2D waveIcon;
	
	public GUIStyle guiStyle;
	
	Vector3 sonarTowerPos;
	Vector3 missileTowerPos;
	Vector3 gunTowerPos;
	Vector3 beamTowerPos;
	Vector3 interfaceTopPos;
	
	float sonarTowerPosX;
	float sonarTowerPosY;
	float missileTowerPosX;
	float missileTowerPosY;
	float gunTowerPosX;
	float gunTowerPosY;
	float beamTowerPosX;
	float beamTowerPosY;
	
	int sonarTowerCost;
	int missileTowerCost;
	int gunTowerCost;
	int beamTowerCost;
	
	Rect sonarCreditRect, sonarCostRect, missileCreditRect, missileCostRect, gunCreditRect, gunCostRect, beamCreditRect, beamCostRect;
	
	Rect creditRect, creditCountRect, healthRect, healthCountRect, waveRect, waveCountRect, nextWaveRect, enemiesInWaveRect, enemiesSpawnedRect;
	
	const int perkTreeButtonWidth = 200;
	Rect perkTreeProgressBarRect;
	Rect perkTreeProgressBarForeRect;
	Rect perkTreeButtonRect;
	Texture2D perkTreeProgressBarBack;
	Texture2D perkTreeProgressBarFore;
	string perkTreeButtonText = "";
	
	void Start()
	{
		// Only create these objects on the first run. Not when it was paused.
		if (!GameController.Instance.gameWasPaused) {
			
			Instantiate(AIController);
			
			// Create Globals
			Instantiate (Globals);
		}
		
		sonarTower = GameObject.Find ("Sonar Tower Builder");
		missileTower = GameObject.Find ("Missile Tower Builder");
		gunTower = GameObject.Find ("Gun Tower Builder");
		beamTower = GameObject.Find ("Beam Tower Builder");
		
		// Get the towers positions
		sonarTowerPos = Camera.main.WorldToScreenPoint(sonarTower.transform.position);
		missileTowerPos = Camera.main.WorldToScreenPoint(missileTower.transform.position);
		gunTowerPos = Camera.main.WorldToScreenPoint(gunTower.transform.position);
		beamTowerPos = Camera.main.WorldToScreenPoint(beamTower.transform.position);
		interfaceTopPos = Camera.main.WorldToScreenPoint (GameObject.Find ("Interface top").transform.position);
		
		sonarTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		missileTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		gunTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		beamTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		
		sonarTowerPosX = sonarTowerPos.x;
		sonarTowerPosY = sonarTowerPos.y;
		missileTowerPosX = missileTowerPos.x;
		missileTowerPosY = missileTowerPos.y;
		gunTowerPosX = gunTowerPos.x;
		gunTowerPosY = gunTowerPos.y;
		beamTowerPosX = beamTowerPos.x;
		beamTowerPosY = beamTowerPos.y;
		
		// Draw Tower coins and prices
		sonarCreditRect = new Rect (sonarTowerPosX + 7, Screen.height - sonarTowerPosY + 7, 20, 20);
		sonarCostRect = new Rect (sonarTowerPosX + 13, Screen.height - sonarTowerPosY + 10, 20, 20);
		missileCreditRect = new Rect (missileTowerPosX + 7, Screen.height - missileTowerPosY + 7, 20, 20);
		missileCostRect = new Rect (missileTowerPosX + 13, Screen.height - missileTowerPosY + 10, 20, 20);
		gunCreditRect = new Rect (gunTowerPosX + 7, Screen.height - gunTowerPosY + 7, 20, 20);
		gunCostRect = new Rect (gunTowerPosX + 13, Screen.height - gunTowerPosY + 10, 20, 20);
		beamCreditRect = new Rect (beamTowerPosX + 7, Screen.height - beamTowerPosY + 7, 20, 20);
		beamCostRect = new Rect (beamTowerPosX + 13, Screen.height - beamTowerPosY + 10, 20, 20);
		
		// Draw Credits, health and waves
		creditRect = new Rect (Screen.width - 170, Screen.height - interfaceTopPos.y - 10, 20, 20);
		creditCountRect = new Rect (Screen.width - 145, Screen.height - interfaceTopPos.y - 7, 100, 20);
		healthRect = new Rect (Screen.width - 110, Screen.height - interfaceTopPos.y - 10, 20, 20);
		healthCountRect = new Rect (Screen.width - 85, Screen.height - interfaceTopPos.y - 7, 100, 20);
		waveRect = new Rect (15, Screen.height - interfaceTopPos.y - 10, 20, 20);
		waveCountRect = new Rect (40, Screen.height - interfaceTopPos.y - 7, 100, 20);	
		
		nextWaveRect = new Rect (200, Screen.height - interfaceTopPos.y - 7, 100, 20);
		enemiesInWaveRect = new Rect (400, Screen.height - interfaceTopPos.y - 7, 100, 20);
		enemiesSpawnedRect = new Rect (600, Screen.height - interfaceTopPos.y - 7, 100, 20);
		
		
		perkTreeButtonRect = new Rect(
			(Screen.width - perkTreeButtonWidth)/2,
			Screen.height - 40,
			perkTreeButtonWidth,
			30
			);
		
		perkTreeProgressBarRect = new Rect(
			(Screen.width - perkTreeButtonWidth)/2,
			Screen.height - 60,
			perkTreeButtonWidth,
			20
			);
		perkTreeProgressBarForeRect = new Rect(
			perkTreeProgressBarRect.x,
			perkTreeProgressBarRect.y,
			0,
			perkTreeProgressBarRect.height
			);
		perkTreeProgressBarBack = TextureFactory.RGBTexture(30, 30, 30);
		perkTreeProgressBarFore = TextureFactory.RGBTexture(200, 255, 0);
		
		// Resume Game if it was paused
		resumeGame();
	}
	
	private void resumeGame(){
		Time.timeScale = 1;
	}
	
	private void pauseGame(){
		Time.timeScale = 0;
		GameController.Instance.gameWasPaused = true;
	}
	
	void Update(){
		
		if (Input.GetKeyDown(KeyCode.Escape)) { 
			// Set timePlayed to the new totalTimePlayed (includes time played in this scene and previous time played)
			AchievementController.Instance.timePlayed = AchievementController.Instance.totalTimePlayed;
			// Pause the game
			pauseGame();
			// Go to Main Menu
			Application.LoadLevel ("menu");
		}
		
		if (Input.GetKeyDown(KeyCode.O)) { 
			PerkController.Instance.AddExperience(2);
		}
		
		if (GameController.Instance.citadelLives == 0) {
			Debug.Log("Add Lose Game stuff here");
		}
		
		perkTreeButtonText = "Perk Tree [ " + PerkController.Instance.GetPoints() + " ]";
		perkTreeProgressBarForeRect.width = perkTreeProgressBarRect.width * PerkController.Instance.GetExperienceProgress();
	}
	
	void OnGUI(){
		
		// Draw Tower coins and prices
		GUI.DrawTexture(sonarCreditRect, creditIcon);
		GUI.Label(sonarCostRect, sonarTowerCost.ToString(), guiStyle);
		GUI.DrawTexture(missileCreditRect, creditIcon);
		GUI.Label(missileCostRect, missileTowerCost.ToString(), guiStyle);
		GUI.DrawTexture(gunCreditRect, creditIcon);
		GUI.Label(gunCostRect, gunTowerCost.ToString(), guiStyle);
		GUI.DrawTexture(beamCreditRect, creditIcon);
		GUI.Label(beamCostRect, beamTowerCost.ToString(), guiStyle);
		
		// Draw Credits, health and waves
		GUI.DrawTexture(creditRect, creditIcon);
		GUI.Label(creditCountRect, GameController.Instance.citadelCredits.ToString(), guiStyle);
		GUI.DrawTexture(healthRect, healthIcon);
		GUI.Label(healthCountRect, GameController.Instance.citadelLives.ToString(), guiStyle);
		GUI.DrawTexture(waveRect, waveIcon);
		
		GUI.Label(waveCountRect, "WAVE "+GameController.Instance.currentWave.ToString()+"/"+GameController.Instance.numberOfWaves.ToString(), guiStyle);
		GUI.Label(nextWaveRect, "Next wave: "+((int)GameController.Instance.timeTillnextWave).ToString(), guiStyle);
		GUI.Label(enemiesInWaveRect, "Enemies in wave: "+GameController.Instance.enemiesInWave.ToString(), guiStyle);
		GUI.Label(enemiesSpawnedRect, "Enemies spawned: "+GameController.Instance.enemiesSpawned.ToString(), guiStyle);
		
		GUI.DrawTexture(perkTreeProgressBarRect, perkTreeProgressBarBack);
		GUI.DrawTexture(perkTreeProgressBarForeRect, perkTreeProgressBarFore);
		if (GUI.Button(perkTreeButtonRect, perkTreeButtonText)) {
			pauseGame();
			Application.LoadLevel ("perktree");
		}
	}
}
