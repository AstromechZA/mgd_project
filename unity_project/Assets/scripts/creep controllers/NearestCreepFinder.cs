using UnityEngine;
using System.Collections;

public class NearestCreepFinder : Singleton<NearestCreepFinder> {

	ArrayList creeps;

	void Awake() {
		creeps = new ArrayList();
	}

	public void Register(AbstractCreep c) {
		creeps.Add(c);
	}

	public void Deregister(AbstractCreep c) {
		creeps.Remove(c);
	}

	public AbstractCreep GetNearest(Vector3 v) {
		float min = float.MaxValue;
		AbstractCreep closest = null;
		foreach (AbstractCreep c in creeps) {
			float d = (c.transform.position - v).sqrMagnitude;
			if (d < min) {
				min = d;
				closest = c;
			}
		}
		return closest;
	}
}
