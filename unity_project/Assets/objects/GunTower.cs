using UnityEngine;
using System.Collections;

public class GunTower : MonoBehaviour {
	
	public float fireRate = 0.5f;
	
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
	 * |       45
	 * |
	 * |
	 * |
	 * +----------X  0
	 */ 
	
	void Start () {
		mapBones ();
		
		barrelAnimator = new DualBarrelGun(0.5f);
		
		_setTurretAngle(0);
		_setGunElevation(0);
		_setBarrelLProgress(0.8f);
		_setBarrelRProgress(0.0f);
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
	
	void Update () {
		if(Input.GetKey(KeyCode.LeftArrow)) _incrementTurretAngle(-2);
		if(Input.GetKey(KeyCode.RightArrow)) _incrementTurretAngle(+2);
		if(Input.GetKey(KeyCode.UpArrow)) _incrementGunElevation(+2);
		if(Input.GetKey(KeyCode.DownArrow)) _incrementGunElevation(-2);
		
		if(barrelAnimator.isReady()) {
			barrelAnimator.fire();
		}
		barrelAnimator.update();
		
		_setBarrelLProgress(1-barrelAnimator.getProgressLeft());
		_setBarrelRProgress(1-barrelAnimator.getProgressRight());
		
		
	}
	
	#region Turret Rotation
	// -------------------------------------------------------------------------------------------------------------
	
	private void _incrementTurretAngle (float degrees) {
		turretBone.transform.Rotate (Vector3.left, degrees, Space.Self);
	}
	
	private void _setTurretAngle (float degrees) {
		turretBone.localEulerAngles = new Vector3(90-degrees, 90, 0);
	}
	
	private float _getTurretAngle () {
		return 90-turretBone.localEulerAngles.x;
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
		barrelLBone.localScale = new Vector3(percent*0.4f + 0.6f, 1, 1);
	}
	
	private void _setBarrelRProgress (float percent) {
		barrelRBone.localScale = new Vector3(percent*0.4f + 0.6f, 1, 1);
	}
	
	private void _updateGunBarrels () {
		
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
			return Mathf.Clamp01(ease(progress[0]));
		}
		
		public float getProgressRight () {
			return Mathf.Clamp01(ease(progress[1]));
		}
		
	}
	
	
	
	
}
