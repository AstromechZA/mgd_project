using UnityEngine;
using System.Collections;

public class MoveTower : MonoBehaviour {
	public int move_cost = 1;

	private TowerProperties towerProperties;

	private AudioClip build_sound;
	private AudioClip build_error;

	// Placement Visualiser
	private GameObject placementVisualiser;
	bool canBuildHere = false;

	// Material of the placement_lines
	private Material placement_lines; 
	private Material placement_lines_red; 

	bool enoughCredits = false;
	
	private Vector3 screenPoint;
	private Vector3 objectOriginalPosition;

	bool moveLeeway = false;

	void Awake () {
		// Tower properties contain the cost of this tower.
		towerProperties = GetComponent<TowerProperties> ();

		build_sound = towerProperties.build_sound;
		build_error = towerProperties.build_error;
		placement_lines = towerProperties.placement_lines;
		placement_lines_red = towerProperties.placement_lines_red;

		// Capture builder's location for calculation later.
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
	}

	void OnMouseDown () {

		// Set moveLeeway to that of the tower properties
		moveLeeway = GetComponent<TowerProperties> ().moveLeeway;


		if (GameController.Instance.citadelCredits - move_cost >= 0 || moveLeeway) {
			enoughCredits = true;
			// Create Placement Visualiser at the same position and rotation as the tower (use y-position that is inbetween game plane and Huds)
			// Load placement visualisation from Inspector
			placementVisualiser = towerProperties.placementVisualiser;
			placementVisualiser = (GameObject)Instantiate (placementVisualiser, new Vector3 (gameObject.transform.position.x, (float)0.05, gameObject.transform.position.z), gameObject.transform.rotation);
		
			canBuildHere = true;
			objectOriginalPosition = gameObject.transform.position;
			transform.position = MapManager.Instance.SnapToGrid(gameObject.transform.position);
			
			// Update the Visualisers status (Red if cant build - Grey if buildable)
			updatePlacementVisualiserStatus();
		}
	}
	
	void OnMouseDrag (){
		if (enoughCredits || moveLeeway) {
			Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint (currentScreenPoint);
			transform.position = MapManager.Instance.SnapToGrid(currentWorldPoint);

			// Move the current missile with the Missile tower
			if (GetComponent<MissileTowerController> ()){
				GetComponent<MissileTowerController> ().currentMissile.transform.position = MapManager.Instance.SnapToGrid(currentWorldPoint);
			}
			// Disable tower shooting
			GetComponent<BaseTowerController>().enabled = false;
			// Remove tower from occupancy grid
			MapManager.Instance.SetOccupancyForPosition(objectOriginalPosition, false);

			// Update the Visualisers status (Red if cant build - Grey if buildable)
			updatePlacementVisualiserStatus();
			// Update the position of the Placement Visualiser, use the towers position with y-position that is inbetween game plane and Huds)
			placementVisualiser.transform.position = new Vector3 (transform.position.x, (float)0.05, transform.position.z);
		}
	}
	
	void OnMouseUp () {
		if (enoughCredits || moveLeeway) {
			if (MapManager.Instance.PlacementQuery (transform.position) != Vector4.zero) {
				AudioSource.PlayClipAtPoint (build_error, Camera.main.transform.position);		
				// Move object back to original position
				transform.position = objectOriginalPosition;

				// Deal with current Missile (Missile Tower)
				GetComponent<MissileTowerController> ().currentMissile.transform.position = objectOriginalPosition;

				// Destroy the Placement Visualiser
				Destroy (placementVisualiser);
			}
			else{
				// Register tower on occupancy grid to stop overlaps.
				
				AudioSource.PlayClipAtPoint (build_sound, Camera.main.transform.position);
				MapManager.Instance.SetOccupancyForPosition (transform.position, true);

				// New path finding.
				PathObstacle po = GetComponent<PathObstacle> ();
				po.UpdateGraphForObject ();

				// Enable the shooting for the tower.
				GetComponent<BaseTowerController>().enabled = true;
				
				// Destroy the Placement Visualiser
				Destroy (placementVisualiser);

				// Only decrement if there is no more moveLeeway
				if (!moveLeeway){
					// Decrement the towers cost from total credits
					GameController.Instance.citadelCredits -= move_cost;
				}
			}
		}
	}

	private void updatePlacementVisualiserStatus(){
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
	}
}
