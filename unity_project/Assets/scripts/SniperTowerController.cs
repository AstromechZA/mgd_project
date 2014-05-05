using UnityEngine;
using System.Collections;

public class SniperTowerController : MonoBehaviour {
	
	// bones
	private Transform turretBone;
	private Transform barrelBone;
	
	private Quaternion baseQuat = new Quaternion(0, 0, -0.7f, 0.7f);

	void Start () {
		mapBones ();
		
		setTurretAngle(Random.Range(0, 360));
	}

	void Update () {
		incrementTurretAngle(0.5f);
	}

	// map the correct tower bones
	private void mapBones () {
		Transform a = transform.Find ("Armature");
		turretBone = a.Find("turret");
		barrelBone = turretBone.Find("gun");
	}

	private void setTurretAngle (float degrees) {
		turretBone.rotation = baseQuat * Quaternion.Euler(degrees, 0, 0);
	}

	private void incrementTurretAngle (float degrees) {
		turretBone.rotation *= Quaternion.Euler(degrees, 0, 0);
	}



}
