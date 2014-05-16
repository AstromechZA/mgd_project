using UnityEngine;
using System.Collections;

public class Persist : MonoBehaviour {
	void Awake() {
		gameObject.tag = "Instantiable Object";
		DontDestroyOnLoad(transform.gameObject);
	}
}