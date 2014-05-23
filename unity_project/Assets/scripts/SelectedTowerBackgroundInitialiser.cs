using UnityEngine;
using System.Collections;

public class SelectedTowerBackgroundInitialiser : MonoBehaviour {

	// Sets the initially selected tower on game load.
	void Start () {
		TowerPlacementController.Instance.CurrentlySelectedTower = GameObject.Find ("Beam Tower Builder");
	}
}
