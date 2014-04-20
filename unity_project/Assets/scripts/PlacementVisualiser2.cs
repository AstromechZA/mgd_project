using UnityEngine;
using System.Collections;

public class PlacementVisualiser2 : MonoBehaviour {

	private Vector3 screenPoint;
	void OnMouseDown () {
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		Instantiate (gameObject);
	}

	void OnMouseDrag () {
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
		transform.position = currentPos;
	}

	void OnMouseUp () {
		Player.experience--; // cost to build
		Destroy (this); 
		gameObject.GetComponent<Tower>().enabled = true;
		gameObject.AddComponent("MoveTower");
		}
}
