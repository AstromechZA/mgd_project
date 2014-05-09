using UnityEngine;
using System.Collections;

public class BuildTower : MonoBehaviour {

	private TowerProperties towerProperties;

	private Vector3 screenPoint;
	
	void Awake () {
		// Tower properties contain the cost of this tower.
		towerProperties = GetComponent<TowerProperties> ();

		// Capture builder's location for calculation later.
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
	}

	void OnMouseDown(){
		Instantiate (gameObject);
	}

	void OnMouseDrag(){
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentWorldPoint = Camera.main.ScreenToWorldPoint (currentScreenPoint);

		transform.position = MapManager.Instance.SnapToGrid(currentWorldPoint);
	}

	void OnMouseUp(){
		if (MapManager.Instance.PlacementQuery (transform.position) != Vector4.zero) {
			Destroy (gameObject);
		} else {
			// Register tower on occupancy grid to stop overlaps.
			MapManager.Instance.SetOccupancyForPosition(transform.position, true);

			// New path finding.
			PathObstacle po = GetComponent<PathObstacle>();
			po.UpdateGraphForObject();

			GetComponent<BaseTowerController>().enabled = true;

			// Remove this build script from the new tower.
			Destroy (this);
		}
	}
}
