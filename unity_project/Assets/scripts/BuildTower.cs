using UnityEngine;
using System.Collections;

public class BuildTower : MonoBehaviour {
	
	private TowerProperties towerProperties;
	
	private Vector3 screenPoint;
	public AudioClip build_sound;
	public AudioClip build_error;
	
	// Material of the placement_lines
	public Material placement_lines; 
	public Material placement_lines_red; 
	
	// Placement Visualiser
	public GameObject placementVisualiser;
	bool canBuildHere = false;
	
	bool enoughCredits = false;
	
	void Awake () {
		// Tower properties contain the cost of this tower.
		towerProperties = GetComponent<TowerProperties> ();
		
		// Capture builder's location for calculation later.
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
	}
	
	void OnMouseDown(){
		// Only instantiate new tower if the player has enough credits
		if ( GameController.Instance.citadelCredits - GetComponent<TowerProperties> ().cost >= 0){
			enoughCredits = true;
			
			Instantiate (gameObject);
			
			// Create Placement Visualiser at the same position and rotation as the tower (use y-position that is inbetween game plane and Huds)
			// Load placement visualisation from Inspector
			placementVisualiser = (GameObject)Instantiate(placementVisualiser, new Vector3(gameObject.transform.position.x, (float)0.05, gameObject.transform.position.z), gameObject.transform.rotation);
			
			canBuildHere = true;
			transform.position = MapManager.Instance.SnapToGrid(gameObject.transform.position);
			
			// Update the Visualisers status (Red if cant build - Grey if buildable)
			updatePlacementVisualiserStatus();
		}
	}
	
	void OnMouseDrag(){
		if (enoughCredits){
			Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint (currentScreenPoint);
			
			transform.position = MapManager.Instance.SnapToGrid(currentWorldPoint);
			// Update the Visualisers status (Red if cant build - Grey if buildable)
			updatePlacementVisualiserStatus();
			// Update the position of the Placement Visualiser, use the towers position with y-position that is inbetween game plane and Huds)
			placementVisualiser.transform.position = new Vector3 (transform.position.x, (float)0.05, transform.position.z);
		}
	}
	
	void OnMouseUp(){
		if (enoughCredits){
			if (MapManager.Instance.PlacementQuery (transform.position) != Vector4.zero) {
				AudioSource.PlayClipAtPoint (build_error, Camera.main.transform.position);
				Destroy (gameObject);
				// Destroy the Placement Visualiser
				Destroy (placementVisualiser);
			} else {
				// Register tower on occupancy grid to stop overlaps.
				
				AudioSource.PlayClipAtPoint (build_sound, Camera.main.transform.position);
				MapManager.Instance.SetOccupancyForPosition (transform.position, true);
				
				// New path finding.
				PathObstacle po = GetComponent<PathObstacle> ();
				po.UpdateGraphForObject ();
				
				GetComponent<BaseTowerController> ().enabled = true;
				
				// Remove this build script from the new tower.
				Destroy (this);
				
				// Destroy the Placement Visualiser
				Destroy (placementVisualiser);
				
				// Increment the number of towers built
				AchievementController.Instance.towersBuilt++;
				// Decrement the towers cost from total credits
				GameController.Instance.citadelCredits -= GetComponent<TowerProperties>().cost;
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
