using UnityEngine;
using System.Collections;

public class SonarTowerController1 : MonoBehaviour {
	public float spinrate = 500;
	public float spinUpRate = 0.4f;
	public float spinDownRate = 0.5f;
	public float range = 6;

	private Transform topBone;
	private float velocity = 0;

	void Start () {
		mapBones ();
	}

	void Update () {
		// aquire target (using mouse position for now)
		Vector3? target = targetMouse();
		// is the target within range
		if(Input.GetMouseButton(0) && target.HasValue && withinRange(target.Value)) {
			velocity += 1;
			velocity = Mathf.Clamp(velocity * (1 + spinUpRate * Time.deltaTime), 0, spinrate);
		} else {
			velocity -= 1;
			velocity = Mathf.Clamp(velocity / (1 + spinDownRate * Time.deltaTime), 0, spinrate);
		}
		topBone.rotation *= Quaternion.Euler(Time.deltaTime * velocity, 0, 0);
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
