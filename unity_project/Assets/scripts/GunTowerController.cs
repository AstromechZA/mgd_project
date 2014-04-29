using UnityEngine;
using System.Collections;

public class GunTowerController : MonoBehaviour {
	
	public float fireRate = 0.5f;
	public float turnRate = 0.1f;
	 
	// bones
	Transform turretBone;
	Transform rotatorRBone;
	Transform barrelRBone;
	Transform rotatorLBone;
	Transform barrelLBone;
	
	// barrel
	DualBarrelGun barrelAnimator;
	
	/* Turret Rotation orientation
* 90
*
* Z
* | 45
* |
* |
* |
* +----------X 0
*/
	
	private Quaternion baseQuat = new Quaternion(0, 0, -180, 180);
	
	void Start () {
		mapBones ();
		
		barrelAnimator = new DualBarrelGun(fireRate);
		
		_setTurretAngle(0);
		_setGunElevation(0);
	}

	
	void Update () {
		
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane hp = new Plane(Vector3.up, Vector3.zero);
		float d = 0;
		if (hp.Raycast(r, out d)) {
			Vector3 p = r.GetPoint(d);
			pointGunsToward(p);
		}
		
		if(Input.GetMouseButton(0)) {
			if(barrelAnimator.isReady()) {
				barrelAnimator.fire();
			}
		}
		barrelAnimator.update();
		
		_setBarrelLProgress(1-barrelAnimator.getProgressLeft());
		_setBarrelRProgress(1-barrelAnimator.getProgressRight());
		
		
	}
	
	// map the correct gun tower bones
	private void mapBones () {
		// have to traverse it like a tree
		Transform a = transform.Find ("Armature");
		turretBone = a.Find("turret_0");
		rotatorRBone = turretBone.Find("rotator_R_0");
		rotatorLBone = turretBone.Find("rotator_L_0");
		barrelRBone = rotatorRBone.Find("barrel_R_0");
		barrelLBone = rotatorLBone.Find("barrel_L_0");
	}

	public void pointGunsAt(Vector3 p) {
		Vector3 dp = transform.position - p;
		if (dp.magnitude > 0) {
			float a = 180-Vector3.Angle(Vector3.right, dp);
			if (dp.z > 0) {a = -a;}
			_setTurretAngle(a);
			
			
			a = Vector3.Angle(Vector3.right, new Vector3(p.magnitude, -1, 0));
			a = Mathf.Clamp(a, -90, 30);
			_setGunElevation(a);
		}
	}
	
	public void pointGunsToward(Vector3 p) {
		
		Vector3 dp = transform.position - p;
		if (dp.magnitude > 0) {
			float a = 180-Vector3.Angle(Vector3.right, dp);
			if (dp.z > 0) {a = -a;}
			
			float diff = a -_getTurretAngle();
			diff = diff % 360;
			if (diff < -180) diff += 360;
			
			_incrementTurretAngle(diff/3);
			
			a = Vector3.Angle(Vector3.right, new Vector3(p.magnitude, -1, 0));
			a = Mathf.Clamp(a, -90, 30);
			_setGunElevation(a);
		}
		
	}
	
	#region Turret Rotation
	// -------------------------------------------------------------------------------------------------------------
	
	private void _incrementTurretAngle (float degrees) {
		turretBone.rotation *= Quaternion.Euler(degrees, 0, 0);
	}
	
	private void _setTurretAngle (float degrees) {
		turretBone.rotation = baseQuat * Quaternion.Euler(degrees, 0, 0);
	}
	
	private float _getTurretAngle () {
		return 360-turretBone.rotation.eulerAngles.y;
	}
	
	// -------------------------------------------------------------------------------------------------------------
	#endregion
	
	#region Gun Elevation
	// -------------------------------------------------------------------------------------------------------------
	
	private void _incrementGunElevation (float degrees) {
		rotatorRBone.transform.Rotate (Vector3.right, degrees, Space.Self);
		rotatorLBone.transform.Rotate (Vector3.left, degrees, Space.Self);
	}
	
	private void _setGunElevation (float degrees) {
		rotatorRBone.localEulerAngles = new Vector3(-degrees, -90, 0);
		rotatorLBone.localEulerAngles = new Vector3(-degrees, -90, 180);
	}
	
	// -------------------------------------------------------------------------------------------------------------
	#endregion
	
	#region Gun Barrel
	// -------------------------------------------------------------------------------------------------------------
	
	private void _setBarrelLProgress (float percent) {
		barrelLBone.localScale = new Vector3(percent*0.3f + 0.75f, 1, 1);
	}
	
	private void _setBarrelRProgress (float percent) {
		barrelRBone.localScale = new Vector3(percent*0.3f + 0.75f, 1, 1);
	}
	
	// -------------------------------------------------------------------------------------------------------------
	#endregion
	
	
	
	
	
	private class DualBarrelGun {
		
		private int next = 0;
		private float[] progress = new float[2]{1.0f, 1.0f};
		
		float timeBetweenShots = 1.0f;
		
		public DualBarrelGun(float timeBetweenShots) {
			this.timeBetweenShots = timeBetweenShots;
		}
		
		public bool isReady () {
			return progress[next] >= 1.0f && progress[(next+1)%2] > 0.5f;
		}
		
		public int fire () {
			progress[next] = 0.0f;
			next = (next + 1) % 2;
			return (next + 1) % 2;
		}
		
		public void update () {
			float inc = Time.deltaTime / timeBetweenShots;
			progress[0] = progress[0] + inc;
			progress[1] = progress[1] + inc;
		}
		
		public float ease (float x) {
			return 1.3f * Mathf.Pow(x, 1/6f) * Mathf.Cos(x * Mathf.PI/2);
		}
		
		public float getProgressLeft () {
			return ease(Mathf.Clamp01(progress[0]));
		}
		
		public float getProgressRight () {
			return ease(Mathf.Clamp01(progress[1]));
		}
		
	}
	
	
	
	
}