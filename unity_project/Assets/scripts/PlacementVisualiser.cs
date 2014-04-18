using UnityEngine;
using System.Collections;

public class PlacementVisualiser : MonoBehaviour {
	
	public float transparency = 0.4f;
	public GameObject mapController = null;

	private MapCreator _mapCreator;

	private GameObject negativeXBar;
	private GameObject positiveXBar;
	private GameObject negativeZBar;
	private GameObject positiveZBar;
	private GameObject cam;
	
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

		if(mapController) _mapCreator = mapController.GetComponent<MapCreator>();
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


			#if UNITY_ANDROID
			#else

			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			Plane hp = new Plane(Vector3.up, Vector3.zero);
			float d = 0;
			if (hp.Raycast(r, out d)) {
				Vector3 p = r.GetPoint(d);
				centerx = p.x;
				centerz = p.z;
			}

			#endif


			centerx = Mathf.Round(centerx / 0.5f) * 0.5f;
			centerz = Mathf.Round(centerz / 0.5f) * 0.5f;

			centerx  = Mathf.Clamp(centerx, _mapCreator.getMinScrollX()+0.5f, _mapCreator.getMaxScrollX()-0.5f);
			centerz  = Mathf.Clamp(centerz, _mapCreator.getMinScrollZ()+0.5f, _mapCreator.getMaxScrollZ()-0.5f);

			setPosition(centerx, centerz);
		}



	}
}
