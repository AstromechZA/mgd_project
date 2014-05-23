using UnityEngine;
using System.Collections;

public class AbstractCreep : MonoBehaviour {

	public float startingHealth;
	public int bountyValue;

	// -----------------------
	private float healthValue;

	void Awake() {
		healthValue = startingHealth;
	}

	void Start () {
		NearestCreepFinder.Instance.Register(this);
	}

	public void Hit(float damage) {
		healthValue -= damage;
		if( damage < 0) {
			Destroy(gameObject);
		}
	}

	public void OnDestroy() {
		Debug.Log("Deregister creep");
		if (NearestCreepFinder.Instance != null) NearestCreepFinder.Instance.Deregister(this);
	}
}
