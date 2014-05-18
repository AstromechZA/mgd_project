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
		
	void Start()
	{
		// First time Game Scene run or 'Start new Game'
		if (Time.timeScale == 1) {

			//Create the HUD towers
			Instantiate (sonarTower);
			Instantiate (missileTower);
			Instantiate (gunTower);
			Instantiate (beamTower);
			Instantiate (AIController);
			// Create Globals
			Instantiate (Globals);

			// Reset Game Parameters
			GameController.gameController.setGameParameters();
		}
		
		// Resume Game if it was paused
		resumeGame();
	}
	
	private void resumeGame(){
		Time.timeScale = 1;
	}
	
	private void pauseGame(){
		Time.timeScale = 0;
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) { 
			// Set timePlayed to the new totalTimePlayed (includes time played in this scene and previous time played)
			AchievementController.achievementController.timePlayed = AchievementController.achievementController.totalTimePlayed;
			// Pause the game
			GameObject.Find ("menu_back").audio.Play ();
			pauseGame();
			// Go to Main Menu
			Application.LoadLevel ("menu");
		}

		if (GameController.gameController.citadelLives == 0) {
			Debug.Log("Add Lose Game stuff here");
		}
	}
	
	void OnGUI(){
		GUI.DrawTexture(new Rect(Screen.width-170,10,20,20), creditIcon);
		GUI.Label(new Rect(Screen.width-145,13,100,20), GameController.gameController.citadelCredits.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(Screen.width-110,10,20,20), healthIcon);
		GUI.Label(new Rect(Screen.width-85,13,100,20), GameController.gameController.citadelLives.ToString(), guiStyle);
		GUI.DrawTexture(new Rect(15,10,20,20), waveIcon);
		GUI.Label(new Rect(40,13,100,20), GameController.gameController.currentWave.ToString()+"/"+GameController.gameController.numberOfWaves.ToString(), guiStyle);	
	}
}
