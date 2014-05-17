using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	Transform player;
	// Use this for initialization
	void Start () {
		player = (GameObject.Find("Menu Block")).transform;
		transform.parent = player;
	}

}
