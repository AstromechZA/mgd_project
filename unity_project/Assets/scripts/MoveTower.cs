using UnityEngine;
using System.Collections;
using Pathfinding;

public class MoveTower : MonoBehaviour {
	public int move_cost = 1;
	private TowerProperties towerProperties;
	private GameObject placementVisualiser;
	bool canBuildHere = true;
	bool enoughCredits = false;
	private Vector3 screenPoint;
	private Vector3 objectOriginalPosition;
	bool moveLeeway = false;
	bool sellTower = false;

	MissileTowerController missileTowerController;
	GraphUpdateObject oldGraphUpdateObj;
	
	void Awake () {
		// Tower properties contain the cost of this tower.
		towerProperties = GetComponent<TowerProperties> ();
		
		// Capture builder's location for calculation later.
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
	}
	
	void OnMouseDown () {
		
		// Set moveLeeway to that of the tower properties
		moveLeeway = GetComponent<TowerProperties>().HasMoveLeeway;
		missileTowerController = GetComponent<MissileTowerController>();

		// Generate the old collider bounds in case we do move.
		oldGraphUpdateObj = new GraphUpdateObject(collider.bounds);
		
		if (GameController.Instance.citadelCredits - move_cost >= 0 || moveLeeway) {
			enoughCredits = true;
			// Create Placement Visualiser at the same position and rotation as the tower (use y-position that is inbetween game plane and Huds)
			// Load placement visualisation from Inspector
			placementVisualiser = towerProperties.placementVisualiser;
			placementVisualiser = (GameObject)Instantiate (placementVisualiser, new Vector3 (gameObject.transform.position.x, (float)0.05, gameObject.transform.position.z), gameObject.transform.rotation);
			
			canBuildHere = true;
			objectOriginalPosition = gameObject.transform.position;
			transform.position = MapManager.Instance.SnapToGrid(gameObject.transform.position);

			// Disable tower shooting
			GetComponent<BaseTowerController>().enabled = false;

			// Remove tower from occupancy grid at original position.
			MapManager.Instance.SetOccupancyForPosition(objectOriginalPosition, false);
		}
	}
	
	void OnMouseDrag (){

		if (enoughCredits || moveLeeway) {
			Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint (currentScreenPoint);

			transform.position = currentWorldPoint;

			// Move the current missile with the Missile tower
			if (missileTowerController){
				if (missileTowerController.currentMissile){
					missileTowerController.currentMissile.transform.position = transform.position;
				}
			}
			
			// Update the Visualisers status (Red if cant build - Grey if buildable)
			updatePlacementVisualiserStatus();

			if (!sellTower){
				transform.position = MapManager.Instance.SnapToGrid(currentWorldPoint);
			}

			// Update the position of the Placement Visualiser, use the towers position with y-position that is inbetween game plane and Huds)
			placementVisualiser.transform.position = new Vector3 (transform.position.x, (float)0.05, transform.position.z);
		}
	}
	
	void OnMouseUp () {
		// If the tower is not over the HUD
		if (!sellTower){
			if (enoughCredits || moveLeeway) {

				// Tower placed on unbuilable area
				if (MapManager.Instance.PlacementQuery (transform.position) != Vector4.zero) {

					AudioSource.PlayClipAtPoint (towerProperties.build_error, Camera.main.transform.position);

					// Move object back to original position
					transform.position = objectOriginalPosition;
					
					// Move current Missile (Missile Tower)
					if (missileTowerController){
						if (missileTowerController.currentMissile){
							missileTowerController.currentMissile.transform.position = objectOriginalPosition;
						}
					}

					// Register tower on occupancy grid at old position.
					MapManager.Instance.SetOccupancyForPosition (objectOriginalPosition, true);
				}
				// Tower placed on buildable area
				else{
					AudioSource.PlayClipAtPoint (towerProperties.build_sound, Camera.main.transform.position);

					// Register tower on occupancy grid at new position.
					MapManager.Instance.SetOccupancyForPosition (transform.position, true);
					
					// New path finding. Update grid at old and new collider positions.
					PathObstacle po = GetComponent<PathObstacle> ();
					po.UpdateGraphForObject ();
					po.UpdateGraphForObject(oldGraphUpdateObj);
					
					// Only decrement if there is no more moveLeeway
					if (!moveLeeway){
						// Decrement the towers cost from total credits
						GameController.Instance.citadelCredits -= move_cost;
					}
				}

				// Enable the shooting for the tower.
				GetComponent<BaseTowerController>().enabled = true;
			}
		}
		// Handle tower selling
		else{

			// Play sell sound
			GameObject.Find ("tower_sell").audio.Play ();

			// Move current Missile (Missile Tower)
			if (missileTowerController){
				if (missileTowerController.currentMissile){
					Destroy(missileTowerController.currentMissile);
				}
			}
			
			//Increment the citadels credits by (half of the towers value)
			if (!moveLeeway){
				GameController.Instance.citadelCredits += towerProperties.cost*0.5;
			}
			// (All of the towers value)
			else{
				GameController.Instance.citadelCredits += towerProperties.cost;
			}

			PathObstacle po = GetComponent<PathObstacle>();
			po.UpdateGraphForObject(oldGraphUpdateObj);

			//Destroy Tower
			Destroy (gameObject);
		}

		// Destroy the Placement Visualiser
		Destroy (placementVisualiser);
	}
	
	private void updatePlacementVisualiserStatus(){
		try
		{
			// Check to see the Placement visualisers status (Buildable or Not)
			if (MapManager.Instance.PlacementQuery(transform.position) != Vector4.zero) {
				if (canBuildHere){
					// Change Placement Visualisers material to placement_lines_red
					foreach (Renderer child in placementVisualiser.GetComponentsInChildren<Renderer>())
						child.material = towerProperties.placement_lines_red;
					canBuildHere = false;
				}
			} else {
				if (!canBuildHere){
					// Change Placement Visualisers material to placement_lines
					foreach (Renderer child in placementVisualiser.GetComponentsInChildren<Renderer>())
						child.material = towerProperties.placement_lines;
					canBuildHere = true;
				}
			}
			sellTower = false;
		}
		catch(System.Exception e){
			sellTower = true;
		}
	}
}
