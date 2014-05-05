using UnityEngine;
using System.Collections;

public class SniperTowerController : MonoBehaviour {
	#region PUBLICVARS ========================================================================== //
	
	public float fireRate = 1.0f;

	#endregion
	#region PRIVATEVARS ========================================================================== //

	// bones
	private Transform turretBone;
	private Transform barrelBone;
	
	private Quaternion baseQuat = new Quaternion(0, 0, -0.7f, 0.7f);
	
	// barrel
	private SingleBarrelGun barrelAnimator;

	#endregion
	#region STANDARD ========================================================================== //

	void Start () {
		mapBones ();

		barrelAnimator = new SingleBarrelGun(this.fireRate);

		setTurretAngle(Random.Range(0, 360));
	}

	void Update () {
		incrementTurretAngle(0.5f);

		if (barrelAnimator.isReady()) barrelAnimator.fire();

		// update gun barrel easing functions
		barrelAnimator.update();
		// set gun barrel progress
		setBarrelProgress(1-barrelAnimator.getProgress());
	}
	
	#endregion
	#region BONES ========================================================================== //

	private void mapBones () {
		Transform a = transform.Find ("Armature");
		turretBone = a.Find("turret");
		barrelBone = turretBone.Find("gun");
	}

	#endregion
	#region TURRET ANGLE ========================================================================== //

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

	#endregion
	#region BARREL ANIMATOR ========================================================================== //

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
		
	}

	#endregion
}
