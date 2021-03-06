﻿using UnityEngine;
using System.Collections;

public class SniperTowerController : BaseTowerController {
	#region PUBLICVARS ====================================================================== //
	public float turnRate = 50;
	public float laserWidthMin = 3f;
	public float laserWidthMax = 6f;
	public AudioClip sound_laser;

	public Material laserMaterial;

	#endregion
	#region PRIVATEVARS ===================================================================== //
	// bones
	private Transform turretBone;
	private Transform barrelBone;
	
	private Quaternion baseQuat = new Quaternion(0, 0, -0.7f, 0.7f);
	
	// barrel
	private SingleBarrelGun barrelAnimator;

	// laser
	private LineRenderer laser;
	private Vector3 heightOffset = new Vector3(0, 50, 0);


	#endregion
	#region STANDARD ======================================================================== //

	public override void Start () {
		mapBones ();

		barrelAnimator = new SingleBarrelGun(this.fireRate);

		laser = (LineRenderer)gameObject.AddComponent("LineRenderer");
		laser.material = laserMaterial;
		laser.SetColors(Color.cyan, Color.magenta);
		laser.SetWidth(laserWidthMax, laserWidthMin);
		laser.SetVertexCount(3);
		laser.enabled = false;

		setTurretAngle(Random.Range(0, 360));
	}

	public override void Update () {
		if(barrelAnimator.age() > 0.3f) laser.enabled = false;

		if (currentTarget == null) {
			currentTarget = NearestCreepFinder.Instance.GetNearest (transform.position);
		}
		
		// is the target within range
		if(currentTarget != null && WithinRange(currentTarget.transform.position)) {
			// rotate towards tower, return whether within firing angle
			bool canFireUpon = pointGunsToward(currentTarget.transform.position);
			// if it can fire,
			if (canFireUpon) {
				Vector3 start = barrelBone.position;
				Vector3 end = currentTarget.transform.position;
				float distance = (end-start).magnitude;
				Vector3 midpoint = (start+end)/2;
				float amt = Random.value * distance - distance/2;
				amt /= 3;
				midpoint.x += amt;
				midpoint.z += amt;

				if(barrelAnimator.isReady()) {
					//AudioSource.PlayClipAtPoint (sound_laser, Camera.main.transform.position);
					GameObject.Find ("SoundLimiter").GetComponent<SoundLimiter>().playLaser();
					barrelAnimator.fire();
					laser.SetPosition(0, start + heightOffset );
					laser.SetPosition(1, midpoint + heightOffset );
					laser.SetPosition(2, end + heightOffset );
					laser.enabled = true;
					Fire (true, PerkController.Instance.GetPerkBonus(Perk.PerkType.TWR_BEAM_DMG));
				}
				else if(barrelAnimator.age() < 0.3f) {
					laser.SetPosition(1, midpoint + heightOffset );
					laser.SetPosition(2, end + heightOffset );
					Fire (true, PerkController.Instance.GetPerkBonus(Perk.PerkType.TWR_BEAM_DMG));
				}


			} else {
				laser.enabled = false;
			}
		} else {
			currentTarget = null;
			laser.enabled = false;
		}

		// update gun barrel easing functions
		barrelAnimator.update();
		// set gun barrel progress
		setBarrelProgress(1-barrelAnimator.getProgress());
	}
	
	#endregion
	#region MISC ============================================================================ //

	private void mapBones () {
		Transform a = transform.Find ("Armature");
		turretBone = a.Find("turret");
		barrelBone = turretBone.Find("gun");
	}

	public bool pointGunsToward(Vector3 p) {
		
		Vector3 dp = transform.position - p;
		if (dp.magnitude > 0) {
			float a = 180-Vector3.Angle(Vector3.right, dp);
			if (dp.z > 0) {a = -a;}
			
			float diff = a -_getTurretAngle();
			diff = diff % 360;
			if (diff < -180) diff += 360;
			
			float mag = Mathf.Abs(diff);
			if ( mag < 10f) {
				setTurretAngle(a);
				return true;
			}
			
			float dir = diff/mag;
			incrementTurretAngle(dir * turnRate * Time.deltaTime);
			return false;
			
			
		}
		return false;
		
	}

	#endregion
	#region TURRET ANGLE ==================================================================== //

	private void setTurretAngle (float degrees) {
		turretBone.rotation = baseQuat * Quaternion.Euler(degrees, 0, 0);
	}

	private void incrementTurretAngle (float degrees) {
		turretBone.rotation *= Quaternion.Euler(degrees, 0, 0);
	}

	private float _getTurretAngle () {
		return 360-turretBone.rotation.eulerAngles.y;
	}

	#endregion
	#region BARREL ========================================================================== //

	private void setBarrelProgress (float percent) {
		barrelBone.localScale = new Vector3(percent*0.3f + 0.75f, 1, 1);
	}

	public void setFireRate(float rate){
		barrelAnimator.setTime (rate);
	}

	#endregion
	#region BARREL ANIMATOR ================================================================= //

	private class SingleBarrelGun {

		private float progress = 1.0f;
		
		float timeBetweenShots = 1.0f;
		
		public SingleBarrelGun(float timeBetweenShots) {
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

		public void setTime(float time){
			this.timeBetweenShots = time;
		}
		
	}

	#endregion
}
