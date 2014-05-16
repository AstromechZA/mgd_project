using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public GameObject sonarTower;
	public GameObject missileTower;
	public GameObject gunTower;
	public GameObject beamTower;

	void Start()
	{
		// If the game has not yet been paused (Create the HUD towers)
		// First time Game Scene run or 'Start new Game'
		if (Time.timeScale == 1) {
			Instantiate (sonarTower);
			Instantiate (missileTower);
			Instantiate (gunTower);
			Instantiate (beamTower);
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

	void OnGUI(){
		if (GUI.Button (new Rect(5, 5, 80, 30), "Back")) {
			// Set timePlayed to the new totalTimePlayed (includes time played in this scene and previous time played)
			AchievementController.achievementController.timePlayed = AchievementController.achievementController.totalTimePlayed;
			// Pause the game
			pauseGame();
			// Go to Main Menu
			Application.LoadLevel ("menu");
		}
	}
}
