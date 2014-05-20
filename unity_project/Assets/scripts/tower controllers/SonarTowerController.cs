using UnityEngine;
using System.Collections;

public class SonarTowerController : BaseTowerController {
	public float spinrate = 500;
	public float spinUpRate = 0.4f;
	public float spinDownRate = 0.5f;

	private float range;
	private float cost;

	private Transform topBone;
	private float velocity = 0;

	private AbstractCreep currentTarget;

	public override void Start () {
		mapBones ();

		TowerProperties tp = GetComponent<TowerProperties> ();
		cost = tp.cost;
		range = tp.range;
	}

	public override void Update () {
		if (currentTarget == null) {
			currentTarget = NearestCreepFinder.Instance.GetNearest (transform.position);
		}
		
		// is the target within range
		if(currentTarget != null && withinRange(currentTarget.transform.position)) {
			velocity += 1;
			velocity = Mathf.Clamp(velocity * (1 + spinUpRate * Time.deltaTime), 0, spinrate);
		} else {
			velocity -= 1;
			velocity = Mathf.Clamp(velocity / (1 + spinDownRate * Time.deltaTime), 0, spinrate);
			currentTarget = null;
		}
		if(velocity > 0) topBone.rotation *= Quaternion.Euler(Time.deltaTime * velocity, 0, 0);
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
