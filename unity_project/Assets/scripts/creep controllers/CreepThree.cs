using UnityEngine;
using System.Collections;

public class CreepThree : MonoBehaviour {
	// Use this for initialization
	void Awake () {
		this.transform.FindChild ("model/Cube").renderer.material.color = Color.magenta;
	}
}
