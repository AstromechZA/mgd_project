using UnityEngine;
using System.Collections;

public class GunTower : MonoBehaviour {

	Transform turretBone;
	Transform rotatorRBone;
	Transform barrelRBone;
	Transform rotatorLBone;
	Transform barrelLBone;


	void Start () {
		mapBones ();

		_setTurretClockwise(90);
		_setGunElevation(45);
	}

	// map the correct gun tower bones
	private void mapBones () {
		// have to traverse it like a tree
		Transform a = transform.Find ("Armature");
		turretBone = a.Find("turret_0");
		rotatorRBone = turretBone.Find("rotator_R_0");
		rotatorLBone = turretBone.Find("rotator_L_0");
		barrelRBone = rotatorRBone.Find("barrel_R_0");
		barrelLBone = rotatorLBone.Find("barrel_L_0");
	}
	
	void Update () {
		_incrementTurretClockwise(1.0f);
		_incrementGunElevation(1.0f);
	}

	#region Turret Rotation
	// -------------------------------------------------------------------------------------------------------------

	private void _incrementTurretClockwise (float degrees) {
		turretBone.transform.Rotate (Vector3.left, degrees, Space.Self);
	}

	private void _setTurretClockwise (float degrees) {
		turretBone.localEulerAngles = Vector3.up * degrees;
	}

	// -------------------------------------------------------------------------------------------------------------
	#endregion

	#region Gun Elevation
	// -------------------------------------------------------------------------------------------------------------

	private void _incrementGunElevation (float degrees) {
		rotatorRBone.transform.Rotate (Vector3.left, degrees, Space.Self);
		rotatorLBone.transform.Rotate (Vector3.right, degrees, Space.Self);
	}

	private void _setGunElevation (float degrees) {
		rotatorRBone.localEulerAngles = new Vector3(-degrees,90,0);
		rotatorLBone.localEulerAngles = new Vector3(-degrees,90,180);
	}

	// -------------------------------------------------------------------------------------------------------------
	#endregion

}
