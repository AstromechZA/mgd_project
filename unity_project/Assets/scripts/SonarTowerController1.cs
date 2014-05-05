using UnityEngine;
using System.Collections;

public class SonarTowerController1 : MonoBehaviour {
	public float spinrate = 100;
	public float range = 6;

	private Transform topBone;

	void Start () {
		mapBones ();
	}

	void Update () {
		// aquire target (using mouse position for now)
		Vector3? target = targetMouse();
		// is the target within range
		if(Input.GetMouseButton(0) && target.HasValue && withinRange(target.Value)) {
			topBone.rotation *= Quaternion.Euler(Time.deltaTime * spinrate, 0, 0);
		}
	}

	private void mapBones () {
		Transform a = transform.Find ("Armature");
		topBone = a.Find("Bone");
	}

	private bool withinRange(Vector3 t) {
		return (transform.position - t).magnitude < this.range;
	}
	
	private Vector3? targetMouse() {
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane hp = new Plane(Vector3.up, Vector3.zero);
		float d = 0;
		if (hp.Raycast(r, out d)) {
			return r.GetPoint(d);
		}
		return null;
	}
}
