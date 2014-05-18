using UnityEngine;
using System.Collections;

public class MenuTextBehaviour : MonoBehaviour {


	void OnMouseDown () {

		renderer.material.color = Color.green;

	}

	void OnMouseUp () {
		
		renderer.material.color = Color.red;

		GameObject.Find ("menu_select").audio.Play ();
		
		// Play the game (go to scene_level)
		if (GameObject.Find ("Menu Block/Play_Game_text") == this.gameObject) {
			// Game is paused
			if (Time.timeScale == 0){
				destroyAllObjectsWithTag("Instantiable Object");
				Time.timeScale=1;
				Application.LoadLevel("gridtest");
			}
			else{
				// Ensure there are not already instantiated objects in the scene
				destroyAllObjectsWithTag("Instantiable Object");
				Time.timeScale=1;
				Application.LoadLevel("gridtest");
			}
		}
		// Handle Achievements
		else if (GameObject.Find ("Menu Block/Achievement_text") == this.gameObject) {
			Application.LoadLevel ("achievements");
		}
		
		
		// Resume the Game
		else if(GameObject.Find ("Resume_Game_text(Clone)") == this.gameObject){
			
			Application.LoadLevel ("gridtest");	
		}
		// Exit game
		else{
			Application.Quit();
		}
	
	}


	private void destroyAllObjectsWithTag(string tag){
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
