using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	// Start with no experience
	public static int experience = 0;
	public static int castleHealth = 100;
	void Start() {
		//Set the screen orientation to landscape
		Screen.orientation = ScreenOrientation.LandscapeLeft;
}
	void OnGUI() {	
		// Draw the players experience and the castles health
		GUI.Label(new Rect(5, 0, 400, 200), "XP: " + experience);
		GUI.Label(new Rect(0, 40, 400, 200), "Castle Health: " + castleHealth);
	}
}
