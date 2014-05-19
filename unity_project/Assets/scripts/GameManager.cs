using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject sonarTower;
	public GameObject missileTower;
	public GameObject gunTower;
	public GameObject beamTower;
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

	bool isPaused = false;
		
	void Start()
	{
		// Only create these objects on the first run. Not when it was paused.
		if (!isPaused) {
			Instantiate(AIController);

			// Create Globals
			Instantiate (Globals);

			// Reset Game Parameters
			GameController.Instance.ResetGameParameters();
		}

		// Get the towers positions
		sonarTowerPos = Camera.main.WorldToScreenPoint(sonarTower.transform.position);
		missileTowerPos = Camera.main.WorldToScreenPoint(missileTower.transform.position);
		gunTowerPos = Camera.main.WorldToScreenPoint(gunTower.transform.position);
		beamTowerPos = Camera.main.WorldToScreenPoint(beamTower.transform.position);
		
		// Resume Game if it was paused
		resumeGame();
	}
	
	private void resumeGame(){
		Time.timeScale = 1;
		isPaused = false;
	}
	
	private void pauseGame(){
		Time.timeScale = 0;
		isPaused = true;
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

		if (GameController.Instance.citadelLives == 0) {
			Debug.Log("Add Lose Game stuff here");
		}
	}
	
	void OnGUI(){

		// Draw Tower coins and prices
		GUI.DrawTexture(new Rect(sonarTowerPos.x+7,Screen.height-sonarTowerPos.y+7,20,20), creditIcon);
		GUI.Label(new Rect(sonarTowerPos.x+13,Screen.height-sonarTowerPos.y+10,20,20), sonarTower.GetComponent<TowerProperties> ().cost.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(missileTowerPos.x+7,Screen.height-missileTowerPos.y+7,20,20), creditIcon);
		GUI.Label(new Rect(missileTowerPos.x+13,Screen.height-missileTowerPos.y+10,20,20), missileTower.GetComponent<TowerProperties> ().cost.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(gunTowerPos.x+7,Screen.height-gunTowerPos.y+7,20,20), creditIcon);
		GUI.Label(new Rect(gunTowerPos.x+13,Screen.height-gunTowerPos.y+10,20,20), gunTower.GetComponent<TowerProperties> ().cost.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(beamTowerPos.x+7,Screen.height-beamTowerPos.y+7,20,20), creditIcon);
		GUI.Label(new Rect(beamTowerPos.x+13,Screen.height-beamTowerPos.y+10,20,20), beamTower.GetComponent<TowerProperties> ().cost.ToString(), guiStyle);

		// Draw Credits, health and waves
		GUI.DrawTexture(new Rect(Screen.width-170,10,20,20), creditIcon);
		GUI.Label(new Rect(Screen.width-145,13,100,20), GameController.Instance.citadelCredits.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(Screen.width-110,10,20,20), healthIcon);
		GUI.Label(new Rect(Screen.width-85,13,100,20), GameController.Instance.citadelLives.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(15,10,20,20), waveIcon);
		GUI.Label(new Rect(40,13,100,20), "WAVE "+GameController.Instance.currentWave.ToString()+"/"+GameController.Instance.numberOfWaves.ToString(), guiStyle);	
	}
}
