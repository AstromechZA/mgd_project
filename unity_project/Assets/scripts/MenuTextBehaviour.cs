using UnityEngine;
using System.Collections;

public class MenuTextBehaviour : MonoBehaviour {

	void OnMouseDown () {
		renderer.material.color = Color.green;
	}

	void OnMouseUp () {
		renderer.material.color = Color.red;

		// Play the game (go to scene_level)
		if (GameObject.Find ("Menu/Play_Game_text") == this.gameObject) {
			Application.LoadLevel("scene_level");
		}
		// Handle Achievements
		else if (GameObject.Find ("Menu/Achievement_text") == this.gameObject) {

		}
		// Exit game
		else{
			Application.Quit();
		}
	}
}
