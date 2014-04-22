using UnityEngine;
using System.Collections;

// from http://unityshorttutorials.blogspot.com/2013/11/drag-and-drop-in-unity.html
public class BuildTower1 : MonoBehaviour {

	public int buildcost = 3;

	// Placement Visualiser Prefab
	//GameObject placementVisualiserPrefab = null;
	// Placement Visualiser
	GameObject placementVisualiser;

	private Vector3 screenPoint;
	void OnMouseDown () {
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		Instantiate (gameObject);

		// Create Placement Visualiser at the same position and rotation as the tower (use y-position of Placement Visualiser)
		// Load placement visualisation from Resources folder
		placementVisualiser = (GameObject)Instantiate(Resources.Load("PlacementVisualisation"), new Vector3(gameObject.transform.position.x, (float)-2.031304, gameObject.transform.position.z), gameObject.transform.rotation);
	}

	void OnMouseDrag () {
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
		transform.position = currentPos;

		gameObject.GetComponent<NavMeshObstacle> ().enabled = false; // can't block while dragging	

		// Update the position of the Placement Visualiser, use the towers position with the Placement Visualisers y-position)
		placementVisualiser.transform.position = new Vector3 (currentPos.x, (float)-2.031304, currentPos.z);
	}

	void OnMouseUp () {
		Player.energy -= buildcost; // cost to build
		Destroy (this); 
		gameObject.GetComponent<Tower> ().enabled = true;
		gameObject.AddComponent ("MoveTower");

		gameObject.GetComponent<NavMeshObstacle> ().enabled = true; // allow blocking again

		// Destroy the Placement Visualiser
		Destroy(placementVisualiser);
	}
}
