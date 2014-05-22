using UnityEngine;
using System.Collections;

public class SonarTowerController : BaseTowerController {
	public float spinrate = 500;
	public float spinUpRate = 0.4f;
	public float spinDownRate = 0.5f;

	private Transform topBone;
	private float velocity = 0;

	private AbstractCreep currentTarget;

	public override void Start () {
		mapBones ();
	}

	public override void Update () {
		if (currentTarget == null) {
			currentTarget = NearestCreepFinder.Instance.GetNearest (transform.position);
		}
		
		// is the target within range
		if(currentTarget != null && WithinRange(currentTarget.transform.position)) {
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
}
