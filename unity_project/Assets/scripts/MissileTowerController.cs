using UnityEngine;
using System.Collections;

public class MissileTowerController : MonoBehaviour {
	#region PUBLICVARS ====================================================================== //
	
	public float fireRate = 5.0f;
	
	public float range = 8;
	
	#endregion
	#region PRIVATEVARS ===================================================================== //

	private DoorControl doorControl;
	private const float DOOROPENAMOUNT = 0.3f;

	// bones
	private Transform mainBone;
	private Transform northBone;
	private Transform southBone;
	private Transform eastBone;
	private Transform westBone;
	
	#endregion
	#region STANDARD ======================================================================== //

	void Start () {
		mapBones();

		doorControl = new DoorControl(fireRate);
	}

	void Update () {
		Vector3? target = targetMouse();

		if(Input.GetMouseButton(0) && target.HasValue && withinRange(target.Value)) {
			if(doorControl.isReady()) {
				doorControl.fire();
			}
		}
		
		doorControl.update();
		setDoorProgress(doorControl.getProgress());
	}

	#endregion
	#region MISC ============================================================================ //

	private void mapBones () {
		Transform a = transform.Find ("Armature");
		mainBone = a.Find("main");
		northBone = mainBone.Find("north");
		southBone = mainBone.Find("south");
		eastBone = mainBone.Find("east");
		westBone = mainBone.Find("west");
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

	private bool withinRange(Vector3 t) {
		return (transform.position - t).magnitude < this.range;
	}
	
	#endregion
	#region DOORS ============================================================================ //

	private void setDoorProgress(float percent) {
		float d = percent * DOOROPENAMOUNT;
		northBone.transform.localPosition = new Vector3(-1, 0, -d);
		southBone.transform.localPosition = new Vector3(-1, 0, d);
		eastBone.transform.localPosition = new Vector3(-1, -d, 0);
		westBone.transform.localPosition = new Vector3(-1, d, 0);
	}

	#endregion
	#region DOORSANIMATOR ==================================================================== //

	private class DoorControl {
		
		private float progress = 1.0f;
		
		float timeBetweenShots = 1.0f;
		
		public DoorControl(float timeBetweenShots) {
			this.timeBetweenShots = timeBetweenShots;
		}
		
		public float age () {
			return progress;
		}
		
		public bool isReady () {
			return progress >= 1.0f;
		}
		
		public void fire () {
			progress = 0.0f;
		}
		
		public void update () {
			float inc = Time.deltaTime / timeBetweenShots;
			progress = progress + inc;
		}
		
		public float ease (float x) {
			return 1.3f * Mathf.Pow(x, 1/6f) * Mathf.Cos(x * Mathf.PI/2);
		}
		
		public float getProgress () {
			return ease(Mathf.Clamp01(progress));
		}
	}

	#endregion

}
