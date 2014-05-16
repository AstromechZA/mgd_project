using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GUIStyle GUIStyleMenuBackground;

	void Start(){
		// Load texture into background of style
		GUIStyleMenuBackground.normal.background = (Texture2D)Resources.Load("Achievements/bases/notifier_background_2");
	}

	void OnGUI(){
		// Create background Texture
		GUI.Box(new Rect(0,0,Screen.width, Screen.height),"",GUIStyleMenuBackground);

		// If the game is paused
		if (Time.timeScale == 0) {
			if (GUI.Button (new Rect ((Screen.width / 2) - 40, 107, 130, 30), "Start New Game")) {
				// Ensure there are not already instantiated objects in the scene
				destroyAllInstantiatedObjects();
				Time.timeScale=1;
				Application.LoadLevel ("gridtest");
			} else if (GUI.Button (new Rect ((Screen.width / 2) - 25, 177, 100, 30), "Resume Game")) {
				Application.LoadLevel ("gridtest");	
			}
			else if (GUI.Button (new Rect ((Screen.width/2)-25,247,100,30), "Achievements")) {
				Application.LoadLevel ("achievements");
			}
			else if (GUI.Button (new Rect ((Screen.width/2)-15,317,80,30), "Exit Game")) {
				Application.Quit();
			}
		} else {
			if (GUI.Button (new Rect ((Screen.width / 2) - 40, 107, 130, 30), "Start New Game")) {
				// Ensure there are not already instantiated objects in the scene
				destroyAllInstantiatedObjects();
				Time.timeScale=1;
				Application.LoadLevel ("gridtest");
			}
			else if (GUI.Button (new Rect ((Screen.width/2)-25,177,100,30), "Achievements")) {
				Application.LoadLevel ("achievements");
			}
			else if (GUI.Button (new Rect ((Screen.width/2)-15,247,80,30), "Exit Game")) {
				Application.Quit();
			}
		}
	}

	private void destroyAllInstantiatedObjects(){
		// Get an array of all the instantiated objects
		GameObject[] instantiatedObjects = GameObject.FindGameObjectsWithTag("Instantiable Object");
		// Destroy them
		if (instantiatedObjects != null) {
			foreach (GameObject instantiatedObject in instantiatedObjects) {
				Destroy (instantiatedObject);
			}
		}
	}


}
