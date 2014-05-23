using UnityEngine;
using System.Collections;

public class TowerSelector : MonoBehaviour {

	void OnMouseDown() {
		TowerPlacementController.Instance.CurrentlySelectedTower = gameObject;
	}
}
