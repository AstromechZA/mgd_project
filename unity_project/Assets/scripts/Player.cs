using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	// Start with no experience
	public static int experience = 0;
	public static int energy = 10;
	public static int castleHealth = 100;
	public static int perk_points = 0;
	void Start() {
		//Set the screen orientation to landscape
		Screen.orientation = ScreenOrientation.LandscapeLeft;
}
	void OnGUI() {	
		// Draw the players experience and the castles health
		GUI.Label(new Rect(Screen.width - 90, Screen.height - 50, 400, 200), "Perk Points: " + perk_points);
		GUI.Label(new Rect(Screen.width - 90, Screen.height - 35, 400, 200), "Energy: " + energy);
		GUI.Label(new Rect(Screen.width - 90, Screen.height - 20, 400, 200), "Health: " + castleHealth);
	}
}
