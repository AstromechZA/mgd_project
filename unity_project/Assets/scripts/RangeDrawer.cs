using UnityEngine;
using System.Collections;

public class RangeDrawer : MonoBehaviour {

	GunTowerController turretController;
	Transform turretTransform;

	public float maxRange = 50;

	float maxRangeSqr;

	// Use this for initialization
	void Start () {
		turretController = GetComponent<GunTowerController> ();
		Transform a = transform.Find ("Armature");
		turretTransform = a.Find("turret_0");
	}
	
	// Update is called once per frame
	void Update () {
		if (turretController.lookTowards != Vector3.zero) {
			Vector3 startPoint = new Vector3 (transform.position.x, turretTransform.position.y, transform.position.z);
			Vector3 endPoint = turretController.lookTowards;
			Debug.DrawLine (startPoint, endPoint, Color.green);
		}
	}
}
