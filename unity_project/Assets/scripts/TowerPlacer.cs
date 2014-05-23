using UnityEngine;
using System.Collections;

public class TowerPlacer : MonoBehaviour {

	// Public vars
	public GameObject placementVisualiser;

	// Private vars
	private Vector3 screenPoint;

	private GameObject currentTower;
	private GameObject currentPlacementVisualiser;
	
	private bool canBuildHere = false;

	void Awake() {
		// Capture builder's location for calculation later.
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
	}

	void OnMouseDown(){
		GameObject selectedTower = TowerPlacementController.Instance.CurrentlySelectedTower;

		if (selectedTower != null) {
			TowerProperties towerProperties = selectedTower.GetComponent<TowerProperties>();

			// Only instantiate new tower if the player has enough credits
			if ( GameController.Instance.citadelCredits - towerProperties.cost >= 0){
				canBuildHere = true;

				// Create a copy of the currently selected tower and place it under the mouse position.
				currentTower = (GameObject)Instantiate (selectedTower);
				currentTower.transform.position = MouseToGridPosition();
				currentTower.name = "Tower";

				// Create Placement Visualiser at the same position and rotation as the tower (use y-position that is inbetween game plane and Huds)
				currentPlacementVisualiser = (GameObject) Instantiate(placementVisualiser, new Vector3(currentTower.transform.position.x, 0.05f, currentTower.transform.position.z), currentTower.transform.rotation);
				
				// Update the Visualisers colour (Red if cant build - Grey if buildable)
				UpdatePlacementVisualiserStatus();
			}
		}
	}

	void OnMouseDrag(){
		if (currentTower != null){			
			currentTower.transform.position = MouseToGridPosition();

			// Move placement visualiser with the tower.
			currentPlacementVisualiser.transform.position = new Vector3 (currentTower.transform.position.x, 0.05f, currentTower.transform.position.z);
			UpdatePlacementVisualiserStatus();
		}
	}

	void OnMouseUp(){
		if (currentTower != null){
			TowerProperties towerProperties = currentTower.GetComponent<TowerProperties>();

			if (MapManager.Instance.PlacementQuery (currentTower.transform.position) != Vector4.zero) {
				AudioSource.PlayClipAtPoint (towerProperties.build_error, Camera.main.transform.position);

				Destroy (currentTower);
			} else {
				// Register tower on occupancy grid to stop overlaps.
				MapManager.Instance.SetOccupancyForPosition (currentTower.transform.position, true);

				AudioSource.PlayClipAtPoint (towerProperties.build_sound, Camera.main.transform.position);
				
				// New path finding.
				PathObstacle po = currentTower.GetComponent<PathObstacle> ();
				po.UpdateGraphForObject ();
				
				// Enable the shooting and moving controller for the tower and persist this object.
				currentTower.GetComponent<BaseTowerController>().enabled = true;
				currentTower.AddComponent<MoveTower>();
				currentTower.AddComponent<Persist>();

				Destroy (currentTower.GetComponent<TowerSelector>());

				
				// Increment the number of towers built
				AchievementController.Instance.towersBuilt++;
				// Decrement the towers cost from total credits
				GameController.Instance.citadelCredits -= towerProperties.cost;
				
				// Set the time the tower was built.
				towerProperties.placementTime = Time.time;
			}

			Destroy (currentPlacementVisualiser);
			currentPlacementVisualiser = null;
			currentTower = null;
		}
	}

	private Vector3 MouseToGridPosition(){
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint (currentScreenPoint);
		
		return MapManager.Instance.SnapToGrid(currentWorldPoint);
	}

	private void UpdatePlacementVisualiserStatus(){
		if (currentPlacementVisualiser != null && currentTower != null) {
			TowerProperties towerProperties = currentTower.GetComponent<TowerProperties> ();

			// Check to see the Placement visualisers status (Buildable or Not)
			if (MapManager.Instance.PlacementQuery (currentTower.transform.position) != Vector4.zero) {
				if (canBuildHere) {
					// Change Placement Visualisers material to placement_lines_red
					foreach (Renderer child in currentPlacementVisualiser.GetComponentsInChildren<Renderer>()) {
							child.material = towerProperties.placement_lines_red;
					}
					canBuildHere = false;
				}
			} else {
				if (!canBuildHere) {
					// Change Placement Visualisers material to placement_lines
					foreach (Renderer child in currentPlacementVisualiser.GetComponentsInChildren<Renderer>()) {
							child.material = towerProperties.placement_lines;
					}
					canBuildHere = true;
				}
			}
		}
	}
}
