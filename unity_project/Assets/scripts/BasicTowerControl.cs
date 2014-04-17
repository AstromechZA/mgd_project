using UnityEngine;
using System.Collections;

public class BasicTowerControl : MonoBehaviour {

	Component towerTop;

	// Use this for initialization
	void Start () {

		towerTop = transform.Find("basic_tower_top");


	}
	
	// Update is called once per frame
	void Update () {
		towerTop.transform.Rotate (Vector3.up * Time.deltaTime * 100.0f);
	}
}
