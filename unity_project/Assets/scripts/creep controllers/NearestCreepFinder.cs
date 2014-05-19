using UnityEngine;
using System.Collections;

public class NearestCreepFinder : Singleton<NearestCreepFinder> {

	ArrayList creeps;

	void Awake() {
		creeps = new ArrayList();
	}

	public void register(AbstractCreep c) {
		creeps.Add(c);
	}

	public void deregister(AbstractCreep c) {
		creeps.Remove(c);
	}

	public AbstractCreep getNearest(Vector3 v) {
		float min = 1<<20;
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
