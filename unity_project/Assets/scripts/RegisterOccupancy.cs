using UnityEngine;
using System.Collections;

/**
 * Sets the occupancy of cells for the building when initialized.
 */
public class RegisterOccupancy : MonoBehaviour {

	void Start () {
		MapManager.Instance.SetOccupancyForPosition (transform.position, true);
	}
}
