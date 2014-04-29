using UnityEngine;
using System.Collections;

public class CamHeightAdapter : MonoBehaviour {

	public float hWidth = 16f;

	// Use this for initialization
	void Start () {
		updateHeight();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void updateHeight() {

		Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height/2, 0));
		Plane hp = new Plane(Vector3.up, Vector3.zero);
		float d = 0;
		if (hp.Raycast(r, out d)) {
			Vector3 p = r.GetPoint(d);
			Vector3 v1 = p - Camera.main.transform.position;
			float a = Vector3.Angle(p, v1);

			float h = Mathf.Tan(a * Mathf.Deg2Rad) * hWidth;

			Camera.main.transform.position = new Vector3(0f, h + 2f, 0f);

		}

	}
}
