using UnityEngine;
using System.Collections;

public class GunTower : MonoBehaviour {

	// bones
	Transform turretBone;
	Transform rotatorRBone;
	Transform barrelRBone;
	Transform rotatorLBone;
	Transform barrelLBone;

	// barrel
	float progressL = 0.0f; // barrel starts off ready
	float progressR = 0.0f; // barrel starts off ready

	/* Turret Rotation orientation
	 * 90
	 * 
	 * Z
	 * |       45
	 * |
	 * |
	 * |
	 * +----------X  0
	 */ 

	void Start () {
		mapBones ();

		_setTurretAngle(0);
		_setGunElevation(0);
		_setBarrelLProgress(0.8f);
		_setBarrelRProgress(0.0f);
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
		if(Input.GetKey(KeyCode.LeftArrow)) _incrementTurretAngle(-2);
		if(Input.GetKey(KeyCode.RightArrow)) _incrementTurretAngle(+2);
		if(Input.GetKey(KeyCode.UpArrow)) _incrementGunElevation(+2);
		if(Input.GetKey(KeyCode.DownArrow)) _incrementGunElevation(-2);
	}

	#region Turret Rotation
	// -------------------------------------------------------------------------------------------------------------

	private void _incrementTurretAngle (float degrees) {
		turretBone.transform.Rotate (Vector3.left, degrees, Space.Self);
	}

	private void _setTurretAngle (float degrees) {
		turretBone.localEulerAngles = new Vector3(90-degrees, 90, 0);
	}

	private float _getTurretAngle () {
		return 90-turretBone.localEulerAngles.x;
	}

	// -------------------------------------------------------------------------------------------------------------
	#endregion

	#region Gun Elevation
	// -------------------------------------------------------------------------------------------------------------

	private void _incrementGunElevation (float degrees) {
		rotatorRBone.transform.Rotate (Vector3.left, degrees, Space.Self);
		//rotatorLBone.transform.Rotate (Vector3.right, degrees, Space.Self);
	}

	private void _setGunElevation (float degrees) {
		rotatorRBone.localEulerAngles = new Vector3(-degrees, 90, 0);
		rotatorLBone.localEulerAngles = new Vector3(-degrees, 90, 180);
	}

	// -------------------------------------------------------------------------------------------------------------
	#endregion

	#region Gun Barrel
	// -------------------------------------------------------------------------------------------------------------

	private void _setBarrelLProgress (float percent) {
		barrelLBone.localScale = new Vector3(percent*0.3f + 0.7f, 1, 1);
	}

	private void _setBarrelRProgress (float percent) {
		barrelRBone.localScale = new Vector3(percent*0.3f + 0.7f, 1, 1);
	}

	// -------------------------------------------------------------------------------------------------------------
	#endregion

}
