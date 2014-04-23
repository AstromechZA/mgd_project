using UnityEngine;
using System.Collections;

public class guntower : MonoBehaviour {

	public Transform turretbone;
	public Transform gunbone;

	private float gunelevation = 0.0f;
	private float gunelevationPerFrame = 1.0f;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (turretbone != null) turretbone.transform.Rotate (Vector3.left, 1.0f, Space.Self);
		if (gunbone != null) {
			gunbone.transform.Rotate (Vector3.up, gunelevationPerFrame, Space.Self);
			gunelevation += gunelevationPerFrame;
			if (gunelevation > 200 || gunelevation < -20) gunelevationPerFrame *= -1;
		}
	}
}
