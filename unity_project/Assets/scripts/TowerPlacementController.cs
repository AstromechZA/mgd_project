using UnityEngine;
using System.Collections;

public class TowerPlacementController : Singleton<TowerPlacementController> {

	private GameObject currentlySelectedTower;
	public GameObject CurrentlySelectedTower {
		get {
			return currentlySelectedTower;
		}
		set {
			currentlySelectedTower = value;
			MoveTowerSelectedBackground();
			Debug.Log("Selected tower builder type: " + value.name);
		}
	}

	private void MoveTowerSelectedBackground(){
		GameObject backgroundPlane = GameObject.Find ("Selected Tower Background");
		Vector3 newPosition = new Vector3 (currentlySelectedTower.transform.position.x, 0.5f, currentlySelectedTower.transform.position.z);
		backgroundPlane.transform.position = newPosition;
	}
}
