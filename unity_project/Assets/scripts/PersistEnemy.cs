using UnityEngine;
using System.Collections;

public class PersistEnemy : MonoBehaviour {
	void Awake() {
		gameObject.tag = "Enemy";
		DontDestroyOnLoad(transform.gameObject);
	}
}