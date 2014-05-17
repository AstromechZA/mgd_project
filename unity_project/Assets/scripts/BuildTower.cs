﻿using UnityEngine;
using System.Collections;

public class BuildTower : MonoBehaviour {

	private TowerProperties towerProperties;

	private Vector3 screenPoint;

	// Material of the placement_lines
	public Material placement_lines; 
	public Material placement_lines_red; 

	// Placement Visualiser
	public GameObject placementVisualiser;
	bool canBuildHere = false;
	
	void Awake () {
		// Tower properties contain the cost of this tower.
		towerProperties = GetComponent<TowerProperties> ();

		// Capture builder's location for calculation later.
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
	}

	void OnMouseDown(){
		Instantiate (gameObject);

		// Create Placement Visualiser at the same position and rotation as the tower (use y-position that is inbetween game plane and Huds)
		// Load placement visualisation from Inspector
		placementVisualiser = (GameObject)Instantiate(placementVisualiser, new Vector3(gameObject.transform.position.x, (float)0.05, gameObject.transform.position.z), gameObject.transform.rotation);
	}

	void OnMouseDrag(){
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint (currentScreenPoint);

		transform.position = MapManager.Instance.SnapToGrid(currentWorldPoint);

		// Check to see the Placement visualisers status (Buildable or Not)
		if (MapManager.Instance.PlacementQuery(transform.position) != Vector4.zero) {
			if (canBuildHere){
				// Change Placement Visualisers material to placement_lines_red
				foreach (Renderer child in placementVisualiser.GetComponentsInChildren<Renderer>())
					child.material = placement_lines_red;
				canBuildHere = false;
			}
		} else {
			if (!canBuildHere){
				// Change Placement Visualisers material to placement_lines
				foreach (Renderer child in placementVisualiser.GetComponentsInChildren<Renderer>())
					child.material = placement_lines;
				canBuildHere = true;
			}
		}
		// Update the position of the Placement Visualiser, use the towers position with y-position that is inbetween game plane and Huds)
		placementVisualiser.transform.position = new Vector3 (transform.position.x, (float)0.05, transform.position.z);
	}

	void OnMouseUp(){
		if (MapManager.Instance.PlacementQuery (transform.position) != Vector4.zero) {
			Destroy(gameObject);
			// Destroy the Placement Visualiser
			Destroy(placementVisualiser);
		} else {
			// Register tower on occupancy grid to stop overlaps.
			MapManager.Instance.SetOccupancyForPosition(transform.position, true);

			// New path finding.
			PathObstacle po = GetComponent<PathObstacle>();
			po.UpdateGraphForObject();

			GetComponent<BaseTowerController>().enabled = true;

			// Remove this build script from the new tower.
			Destroy (this);

			// Destroy the Placement Visualiser
			Destroy(placementVisualiser);

			// Increment the number of towers built
			AchievementController.achievementController.towersBuilt++;
		}
	}
}
