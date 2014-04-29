using UnityEngine;
using System.Collections;

public class SonarTowerController : MonoBehaviour {
		
	public float rotationRate = 90.0f;

	private Transform turretBone;

	void Start () {
		// have to traverse it like a tree
		Transform a = transform.Find ("Armature");
		turretBone = a.Find("Bone");
	}

	void Update () {
		turretBone.rotation *= Quaternion.Euler(rotationRate * Time.deltaTime, 0, 0);
	}
}
