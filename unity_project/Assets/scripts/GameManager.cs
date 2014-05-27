using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	
	GameObject missileTower;
	GameObject gunTower;
	GameObject beamTower;
	public GameObject AIController;
	public GameObject Globals;
	public Texture2D creditIcon;
	public Texture2D healthIcon;
	public Texture2D waveIcon;
	public GUIStyle guiStyle;
	Vector3 missileTowerPos;
	Vector3 gunTowerPos;
	Vector3 beamTowerPos;
	Vector3 interfaceTopPos, interfaceBotPos;
	float missileTowerPosX;
	float missileTowerPosY;
	float gunTowerPosX;
	float gunTowerPosY;
	float beamTowerPosX;
	float beamTowerPosY;
	int missileTowerCost;
	int gunTowerCost;
	int beamTowerCost;
	Rect missileCreditRect, missileCostRect, gunCreditRect, gunCostRect, beamCreditRect, beamCostRect;
	Rect creditRect, creditCountRect, healthRect, healthCountRect, waveRect, waveCountRect;
	//Rect nextWaveRect, enemiesInWaveRect, enemiesSpawnedRect;
	Rect endGameHeaderRect, endGameRestartRect, endGameMainMenuRect;
	const int perkTreeButtonWidth = 200;
	Rect perkTreeProgressBarRect;
	Rect perkTreeProgressBarForeRect;
	Rect perkTreeButtonRect;
	Texture2D perkTreeProgressBarBack;
	Texture2D perkTreeProgressBarFore;
	string perkTreeButtonText = "";
	bool game_over = false;
	bool game_won = false;
	
	
	public GUIStyle endGameFont;
	
	void Start ()
	{
		// Only create these objects on the first run. Not when it was paused.
		if (!GameController.Instance.gameWasPaused) {
			
			Instantiate (AIController);
			
			// Create Globals
			Instantiate (Globals);
		}
		
		missileTower = GameObject.Find ("Missile Tower Builder");
		gunTower = GameObject.Find ("Gun Tower Builder");
		beamTower = GameObject.Find ("Beam Tower Builder");
		
		// Get the towers positions
		missileTowerPos = Camera.main.WorldToScreenPoint (missileTower.transform.position);
		gunTowerPos = Camera.main.WorldToScreenPoint (gunTower.transform.position);
		beamTowerPos = Camera.main.WorldToScreenPoint (beamTower.transform.position);
		interfaceTopPos = Camera.main.WorldToScreenPoint (GameObject.Find ("Interface top").transform.position);
		interfaceBotPos = Camera.main.WorldToScreenPoint (GameObject.Find ("Interface bottom").transform.position);
		
		missileTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		gunTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		beamTowerCost = missileTower.GetComponent<TowerProperties> ().cost;
		
		missileTowerPosX = missileTowerPos.x;
		missileTowerPosY = missileTowerPos.y;
		gunTowerPosX = gunTowerPos.x;
		gunTowerPosY = gunTowerPos.y;
		beamTowerPosX = beamTowerPos.x;
		beamTowerPosY = beamTowerPos.y;
		
		// Draw Tower coins and prices
		missileCreditRect = new Rect (missileTowerPosX + 7, Screen.height - missileTowerPosY + 7, 20, 20);
		missileCostRect = new Rect (missileTowerPosX + 13, Screen.height - missileTowerPosY + 10, 20, 20);
		gunCreditRect = new Rect (gunTowerPosX + 7, Screen.height - gunTowerPosY + 7, 20, 20);
		gunCostRect = new Rect (gunTowerPosX + 13, Screen.height - gunTowerPosY + 10, 20, 20);
		beamCreditRect = new Rect (beamTowerPosX + 7, Screen.height - beamTowerPosY + 7, 20, 20);
		beamCostRect = new Rect (beamTowerPosX + 13, Screen.height - beamTowerPosY + 10, 20, 20);
		
		// Draw Credits, health and waves
		creditRect = new Rect (Screen.width - 120, Screen.height - interfaceTopPos.y - 10, 20, 20);
		creditCountRect = new Rect (Screen.width - 95, Screen.height - interfaceTopPos.y - 7, 100, 20);
		healthRect = new Rect (Screen.width - 60, Screen.height - interfaceTopPos.y - 10, 20, 20);
		healthCountRect = new Rect (Screen.width - 35, Screen.height - interfaceTopPos.y - 7, 100, 20);
		waveRect = new Rect (20, Screen.height - interfaceTopPos.y - 10, 20, 20);
		waveCountRect = new Rect (45, Screen.height - interfaceTopPos.y - 7, 100, 20);	
		
		//nextWaveRect = new Rect (200, Screen.height - interfaceTopPos.y - 7, 100, 20);
		//enemiesInWaveRect = new Rect (400, Screen.height - interfaceTopPos.y - 7, 100, 20);
		//enemiesSpawnedRect = new Rect (600, Screen.height - interfaceTopPos.y - 7, 100, 20);
		
		
		
		// End Game Rectangles
		endGameHeaderRect = new Rect ((Screen.width / 2) - 50, Screen.height / 2 - 150, 100, 60);
		endGameRestartRect = new Rect ((Screen.width / 2) - 75, Screen.height / 2 - 50, 150, 60);
		endGameMainMenuRect = new Rect ((Screen.width / 2) - 75, Screen.height / 2 + 25, 150, 60);
		
		perkTreeButtonRect = new Rect (
			(Screen.width - perkTreeButtonWidth) / 2,
			Screen.height - interfaceBotPos.y - 10,
			perkTreeButtonWidth,
			30
			);
		
		perkTreeProgressBarRect = new Rect (
			(Screen.width - perkTreeButtonWidth) / 2,
			Screen.height - interfaceBotPos.y - 20,
			perkTreeButtonWidth,
			20
			);
		perkTreeProgressBarForeRect = new Rect (
			perkTreeProgressBarRect.x,
			perkTreeProgressBarRect.y,
			0,
			perkTreeProgressBarRect.height
			);
		perkTreeProgressBarBack = TextureFactory.RGBTexture (30, 30, 30);
		perkTreeProgressBarFore = TextureFactory.RGBTexture (200, 255, 0);
		
		// Resume Game if it was paused
		resumeGame ();
	}
	
	private void resumeGame ()
	{
		GameObject.Find ("soundtrack_menu").audio.Pause ();
		if (GameController.Instance.gameWasPaused)
			GameObject.Find ("soundtrack_level").audio.Play ();
		
		Time.timeScale = 1;
	}
	
	private void pauseGame ()
	{
		GameObject.Find ("soundtrack_level").audio.Pause ();
		GameObject.Find ("soundtrack_menu").audio.Play ();
		Time.timeScale = 0;
		GameController.Instance.gameWasPaused = true;
	}
	
	private void gameOver ()
	{
		pauseGame ();
		GameObject.Find ("game_over").audio.Play ();
		game_over = true;
	}
	
	private void gameWon(){
		AchievementController.Instance.gamesWon++;
		
		// Win without citadel being damanged (cleanslate)
		if (GameController.Instance.startingCitadelLives == GameController.Instance.citadelLives){
			AchievementController.Instance.cleanSlate++;
		}
		// Perfectionist
		if (AchievementController.Instance.uniqueTargetAbilitiesUsed == 0) {
			AchievementController.Instance.perfectionist++;
		}
		// Triple play
		else if (AchievementController.Instance.uniqueTargetAbilitiesUsed == 3){
			AchievementController.Instance.triplePlay++;
		}
		
		pauseGame ();
		game_won = true;
	}
	
	void Update ()
	{
		
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GameObject.Find ("menu_back").audio.Play ();
			// Set timePlayed to the new totalTimePlayed (includes time played in this scene and previous time played)
			AchievementController.Instance.timePlayed = AchievementController.Instance.totalTimePlayed;
			// Pause the game
			pauseGame ();
			// Go to Main Menu
			Application.LoadLevel ("menu");
		}
		
		if (Input.GetKeyDown (KeyCode.O)) { 
			PerkController.Instance.AddExperience (2);
		}
		// GAME LOST
		if (GameController.Instance.citadelLives <= 0) {
			if (!game_over){
				endGameFont.normal.textColor = Color.red; 
				gameOver ();
			}
		}
		// GAME WON
		if (GameController.Instance.currentWave == GameController.Instance.numberOfWaves && GameController.Instance.enemiesInWave == GameController.Instance.enemiesSpawned) {
			// Ensure that no enemies still exist
			if (GameObject.FindGameObjectsWithTag("Enemy").Length==0){
				if (!game_won){
					endGameFont.normal.textColor = Color.green; 
					gameWon();
				}
			}
		}
		
		perkTreeButtonText = "Perk Tree [ " + PerkController.Instance.GetPoints () + " ]";
		perkTreeProgressBarForeRect.width = perkTreeProgressBarRect.width * PerkController.Instance.GetExperienceProgress ();
		
		
		if (!(GameObject.Find ("soundtrack_level").audio.isPlaying) && Time.timeScale == 1) 
			GameObject.Find ("soundtrack_level").audio.PlayDelayed (4.0F);
		
	}
	
	void OnGUI ()
	{
		
		// Draw Tower coins and prices
		GUI.DrawTexture (missileCreditRect, creditIcon);
		GUI.Label (missileCostRect, missileTowerCost.ToString (), guiStyle);
		GUI.DrawTexture (gunCreditRect, creditIcon);
		GUI.Label (gunCostRect, gunTowerCost.ToString (), guiStyle);
		GUI.DrawTexture (beamCreditRect, creditIcon);
		GUI.Label (beamCostRect, beamTowerCost.ToString (), guiStyle);
		
		// Draw Credits, health and waves
		GUI.DrawTexture (creditRect, creditIcon);
		GUI.Label (creditCountRect, GameController.Instance.citadelCredits.ToString (), guiStyle);
		GUI.DrawTexture (healthRect, healthIcon);
		GUI.Label (healthCountRect, GameController.Instance.citadelLives.ToString (), guiStyle);
		GUI.DrawTexture (waveRect, waveIcon);
		
		GUI.Label (waveCountRect, "WAVE " + GameController.Instance.currentWave.ToString () + "/" + GameController.Instance.numberOfWaves.ToString (), guiStyle);
		//GUI.Label (nextWaveRect, "Next wave: " + ((int)GameController.Instance.timeTillNextWave).ToString (), guiStyle);
		//GUI.Label (enemiesInWaveRect, "Enemies in wave: " + GameController.Instance.enemiesInWave.ToString (), guiStyle);
		//GUI.Label (enemiesSpawnedRect, "Enemies spawned: " + GameController.Instance.enemiesSpawned.ToString (), guiStyle);
		
		GUI.DrawTexture (perkTreeProgressBarRect, perkTreeProgressBarBack);
		GUI.DrawTexture (perkTreeProgressBarForeRect, perkTreeProgressBarFore);
		if (GUI.Button (perkTreeButtonRect, perkTreeButtonText)) {
			GameObject.Find ("menu_select").audio.Play ();
			pauseGame ();
			Application.LoadLevel ("perktree");
		}
		
		if (game_over || game_won) {
			
			if (game_over){
				GUI.Label(endGameHeaderRect, "GAME OVER", endGameFont);
			}
			else{
				GUI.Label(endGameHeaderRect, "GAME WON", endGameFont);
			}
			
			if (GUI.Button (endGameRestartRect, "Restart")) {
				GameController.Instance.ResetGameParameters ();
				GameController.Instance.DestroyAllObjectsWithTag ("Instantiable Object");
				GameController.Instance.DestroyAllObjectsWithTag ("Enemy");
				GameObject.Find ("Creep Spawn Point").GetComponent<CreepSpawner> ().Reset ();
				game_over = false;
				GameObject.Find ("soundtrack_level").audio.Stop ();
				GameObject.Find ("soundtrack_level").audio.PlayDelayed (4.0F);
				Application.LoadLevel ("gridtest");
			}
			
			if (GUI.Button (endGameMainMenuRect, "Main Menu")) {
				GameObject.Find ("soundtrack_level").audio.Stop ();
				GameController.Instance.DestroyAllObjectsWithTag ("Instantiable Object");
				GameController.Instance.DestroyAllObjectsWithTag ("Enemy");
				Time.timeScale = 1;
				Application.LoadLevel ("menu");
			}
		}
	}
}