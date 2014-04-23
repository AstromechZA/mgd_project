using UnityEngine;
using System.Collections;

public class LandscapeScreenOrientation : MonoBehaviour {
	void Start() {
		//Set the screen orientation to landscape
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
}
