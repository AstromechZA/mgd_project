using UnityEngine;
using System.Collections;

public class AbstractCreep : MonoBehaviour {

	public float startingHealth;

	// -----------------------
	private float healthValue;

	void Awake() {
		healthValue = startingHealth;
	}

	void Start () {
		NearestCreepFinder.Instance.register(this);
	}

	public void hit(float damage) {
		healthValue -= damage;
		if( damage < 0) {
			Destroy(gameObject);
		}
	}

	public void OnDestroy() {
		Debug.Log("Deregister creep");
		NearestCreepFinder.Instance.deregister(this);
	}
}
