using UnityEngine;
using System.Collections;

public class CloseListener : MonoBehaviour {


	void Update () {
		// apparently Android back button is mapped to keycode escape
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
	}
}
