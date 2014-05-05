using UnityEngine;
using System.Collections;

public class SonarTowerController1 : MonoBehaviour {
	public float spinrate = 100;

	private Transform topBone;

	void Start () {
		mapBones ();
	}

	void Update () {
		topBone.rotation *= Quaternion.Euler(Time.deltaTime * spinrate, 0, 0);
	}

	private void mapBones () {
		Transform a = transform.Find ("Armature");
		topBone = a.Find("Bone");
	}
}
