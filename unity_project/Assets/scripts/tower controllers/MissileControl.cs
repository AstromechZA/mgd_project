using UnityEngine;
using System.Collections;

public class MissileControl : MonoBehaviour {

	public float horizantalSpeed = 5f;

	private Vector3 origin;
	private Vector3 destination;
	private bool inflight = false;

	private Vector3 horizantalDir;


	void Start () {
	
	}

	void Update () {
		if (inflight) {
			transform.localRotation *= Quaternion.Euler(0, 200 * Time.deltaTime, 0);
			transform.position += horizantalDir * horizantalSpeed * Time.deltaTime;

		}
	}

	public void launch(Vector3 target) {
		this.origin = transform.position;
		this.destination = target;
		this.inflight = true;

		Vector3 t = (destination - origin);
		t.y = 0;
		horizantalDir = t.normalized;

		transform.position += Vector3.up * 3;
	}
}
