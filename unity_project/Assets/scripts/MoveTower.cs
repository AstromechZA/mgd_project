using UnityEngine;
using System.Collections;

// from http://unityshorttutorials.blogspot.com/2013/11/drag-and-drop-in-unity.html
public class MoveTower : MonoBehaviour {

	public int move_cost = 1;
	

	// Placement Visualiser Prefab
	//GameObject placementVisualiserPrefab = null;
	// Placement Visualiser
	GameObject placementVisualiser;

	private Vector3 screenPoint;
	void OnMouseDown () {
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		// Create Placement Visualiser at the same position and rotation as the tower (use y-position of Placement Visualiser)
		// Need to load the prefab from the Resources folder
		placementVisualiser = (GameObject)Instantiate(Resources.Load("PlacementVisualisation"), new Vector3(gameObject.transform.position.x, (float)-2.031304, gameObject.transform.position.z), gameObject.transform.rotation);
	}
	
	void OnMouseDrag (){
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
		transform.position = currentPos;
		gameObject.GetComponent<Tower> ().enabled = false; // can't shoot while dragging
		gameObject.GetComponent<NavMeshObstacle> ().enabled = false; // can't block while dragging		

		// Update the position of the Placement Visualiser, use the towers position with the Placement Visualisers y-position)
		placementVisualiser.transform.position = new Vector3 (currentPos.x, (float)-2.031304, currentPos.z);
	}
	
	void OnMouseUp () {
		Player.energy -= move_cost; // cost to move
		gameObject.GetComponent<Tower>().enabled = true; // allow shooting again
		gameObject.GetComponent<NavMeshObstacle> ().enabled = true; // allow blocking again

		// Destroy the Placement Visualiser
		Destroy(placementVisualiser);
	}
}
