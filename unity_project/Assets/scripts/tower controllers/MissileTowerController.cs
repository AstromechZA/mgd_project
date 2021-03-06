﻿using UnityEngine;
using System.Collections;

public class MissileTowerController : BaseTowerController {
	#region PUBLICVARS ====================================================================== //

	public Component projectile;
	public AudioClip sound_launch;
	public float damageRadius = 25f;
	
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


	public GameObject currentMissile;
	
	#endregion
	#region STANDARD ======================================================================== //

	public override void Start () {
		mapBones();

		spawnmissile();

		doorControl = new DoorControl(fireRate);
	}

	public override void Update () {

		if (currentTarget == null || !WithinRange (currentTarget.transform.position)) { 
			currentTarget = NearestCreepFinder.Instance.GetNearest (transform.position);
		}
		
		// is the target within range
		if(currentTarget != null && WithinRange(currentTarget.transform.position)) {
			if(doorControl.open()) {
				// fire
				//AudioSource.PlayClipAtPoint (sound_launch, Camera.main.transform.position);
				GameObject.Find ("SoundLimiter").GetComponent<SoundLimiter>().playMissile();

				currentMissile.GetComponent<MissileControl>().launch(currentTarget.transform.position);

				currentMissile = null;
				
				doorControl.close();
			} 
		} else {
			currentTarget = null;
			doorControl.close();
		}

		
		doorControl.Update();
		
		if (doorControl.is_closed() && currentMissile == null) spawnmissile();

		setDoorProgress(doorControl.getProgress());
	}

	void OnDestroy(){
		if (currentMissile != null) {
			Destroy (currentMissile);
		}
	}

	#endregion
	#region MISC ============================================================================ //

	private void spawnmissile() {
		currentMissile = (GameObject)Instantiate(projectile.gameObject, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
		currentMissile.GetComponent<MissileControl> ().Damage = this.damage;
		currentMissile.GetComponent<MissileControl> ().DamageRadius = this.damageRadius;
	}

	private void mapBones () {
		Transform a = transform.Find ("Armature");
		mainBone = a.Find("main");
		northBone = mainBone.Find("north");
		southBone = mainBone.Find("south");
		eastBone = mainBone.Find("east");
		westBone = mainBone.Find("west");
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

	public void setOpenRate(float rate){
		doorControl.setTime (rate);
	}

	#endregion
	#region DOORSANIMATOR ==================================================================== //

	private class DoorControl {

		public enum STATE {CLOSED, OPENING, OPEN, CLOSING};

		private STATE state = STATE.CLOSED;
		private float progress = 0.0f;
		private float delta;
		
		public DoorControl(float timeBetweenShots) {
			this.delta = 3.0f / timeBetweenShots;
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

		public void setTime(float time){
			this.delta = 2.0f / time;
		}

	}

	#endregion

}
