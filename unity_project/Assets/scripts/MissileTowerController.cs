using UnityEngine;
using System.Collections;

public class MissileTowerController : MonoBehaviour {
	#region PUBLICVARS ====================================================================== //

	public Component projectile;

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

	private GameObject currentMissile;
	
	#endregion
	#region STANDARD ======================================================================== //

	void Start () {
		mapBones();


		spawnmissile();

		doorControl = new DoorControl(fireRate);
	}

	void Update () {
		Vector3? target = targetMouse();

		if(Input.GetMouseButton(0) && target.HasValue && withinRange(target.Value)) {
			if(doorControl.open()) {
				// fire

				((MissileControl)currentMissile.GetComponent("MissileControl")).launch(target.Value);

				currentMissile = null;
				
				doorControl.close();
			} 
		} else {
			doorControl.close();
		}

		
		doorControl.Update();
		
		if (doorControl.is_closed() && currentMissile == null) spawnmissile();

		setDoorProgress(doorControl.getProgress());
	}

	#endregion
	#region MISC ============================================================================ //

	private void spawnmissile() {
		currentMissile = (GameObject)Instantiate(projectile.gameObject, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
	}

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

		public enum STATE {CLOSED, OPENING, OPEN, CLOSING};

		private STATE state = STATE.CLOSED;
		private float progress = 0.0f;
		private float delta;
		
		public DoorControl(float timeBetweenShots) {
			this.delta = 2.0f / timeBetweenShots;
		}

		public void Update() {
			switch(state) {
			case STATE.OPENING:
				progress += delta * Time.deltaTime;
				if (progress > 1) {
					state = STATE.OPEN;
					progress = 1;
				}
				break;
			case STATE.CLOSING:
				progress -= delta * Time.deltaTime;
				if (progress < 0) {
					state = STATE.CLOSED;
					progress = 0;
				}
				break;
			}
		}

		public float getProgress() { return progress; }

		public bool open() {
			if (state == STATE.CLOSED) {
				state = STATE.OPENING;
			} else if( state == STATE.OPEN) {
				return true;
			}
			return false;
		}

		public bool close() {
			if (state != STATE.CLOSED) {
				state = STATE.CLOSING;
				return false;
			}
			return true;
		}

		public bool is_closed() {
			return state == STATE.CLOSED;
		}

	}

	#endregion

}
