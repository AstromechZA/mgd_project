using UnityEngine;
using System.Collections;

public class MissileControl : MonoBehaviour {

	public float horizantalSpeed = 5f;
	public float flightDuration = 1.5f; //seconds

	private Vector3 origin;
	private Vector3 destination;

	private bool inflight = false;
	private float flightProgress = 0.0f;

	private Vector3 flightVector;
	private Quaternion angleQuat;

	void Start () {
		
	}

	void Update () {
		if (inflight) {
			flightProgress += Time.deltaTime / flightDuration;

			float sflightProgress = squish(flightProgress);

			transform.localRotation = angleQuat * Quaternion.Euler(0,0,-sflightProgress*180);

			Vector3 newpos = origin + sflightProgress * flightVector;
			newpos.y = 10;
			transform.position  = newpos;

			float val = 4 * (-Mathf.Abs(sflightProgress - 0.5f)) + 12;

			transform.localScale = val * Vector3.one;

			if (flightProgress > 1) Destroy(this.gameObject);
		}
	}

	private float squish(float t) {
		float tt = (t*2-1);
		return 0.5f + tt/(1+Mathf.Abs(tt));
	}

	public void launch(Vector3 target) {
		this.origin = transform.position;
		this.destination = target;
		this.inflight = true;

		flightVector = (destination - origin);

		angleQuat = Quaternion.Euler(0, -angleTo(this.origin, this.destination), 0);
	}

	private float angleTo(Vector3 from, Vector3 to) {
		Vector3 dp = to - from;
		dp.y = 0;
		float a = Vector3.Angle(Vector3.right, dp);
		if (dp.z < 0) {a = -a;}
		return a;
	}
}
