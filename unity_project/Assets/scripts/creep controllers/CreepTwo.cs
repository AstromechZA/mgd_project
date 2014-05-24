using UnityEngine;
using System.Collections;

public class CreepTwo : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		this.transform.FindChild ("model/Cube").renderer.material.color = Color.red;
	}
}
