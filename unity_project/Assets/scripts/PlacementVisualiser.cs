using UnityEngine;
using System.Collections;

public class PlacementVisualiser : MonoBehaviour {

	GameObject negativeXBar;
	GameObject positiveXBar;
	GameObject negativeZBar;
	GameObject positiveZBar;
	GameObject cam;

	public float transparency = 0.4f;
	
	private Color _green; 
	private Color _red;

	// Use this for initialization
	void Start () {
		_green = new Color(0.0f,1.0f,0.0f,transparency);
		_red = new Color(1.0f,0.0f,0.0f,transparency);

		negativeXBar = transform.Find("negativeX").gameObject;
		positiveXBar = transform.Find("positiveX").gameObject;
		negativeZBar = transform.Find("negativeZ").gameObject;
		positiveZBar = transform.Find("positiveZ").gameObject;

		cam = GameObject.Find("Main Camera");
	}

	void setPosition(float centerx, float centerz) {
		Vector3 destination = new Vector3 (centerx, transform.position.y, centerz);
		transform.Translate (destination - transform.position);
	}

	void setCollides(bool xp, bool xn, bool zp, bool zn) {
		positiveXBar.renderer.material.color = (xp ? _red : _green);
		negativeXBar.renderer.material.color = (xn ? _red : _green);
		positiveZBar.renderer.material.color = (zp ? _red : _green);
		negativeZBar.renderer.material.color = (zn ? _red : _green);
	}

	void Update () {
		// nothing yet

		if (cam != null) {

			float centerx = cam.transform.position.x;
			float centerz = cam.transform.position.z;

			centerx = Mathf.Round(centerx / 0.5f) * 0.5f;
			centerz = Mathf.Round(centerz / 0.5f) * 0.5f;

			setPosition(centerx, centerz);
		}



	}
}
