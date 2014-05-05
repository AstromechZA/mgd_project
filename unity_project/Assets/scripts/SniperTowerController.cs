using UnityEngine;
using System.Collections;

public class SniperTowerController : MonoBehaviour {
	#region PUBLICVARS ========================================================================== //
	
	public float fireRate = 2.0f;

	public float range = 8;

	public float turnRate = 50;

	public Material laserMaterial;

	#endregion
	#region PRIVATEVARS ========================================================================== //

	// bones
	private Transform turretBone;
	private Transform barrelBone;
	
	private Quaternion baseQuat = new Quaternion(0, 0, -0.7f, 0.7f);
	
	// barrel
	private SingleBarrelGun barrelAnimator;

	// laser
	private LineRenderer laser;

	#endregion
	#region STANDARD ========================================================================== //

	void Start () {
		mapBones ();

		barrelAnimator = new SingleBarrelGun(this.fireRate);

		laser = (LineRenderer)gameObject.AddComponent("LineRenderer");
		laser.material = laserMaterial;
		laser.SetColors(Color.cyan, Color.magenta);
		laser.SetWidth(0.3f, 0.15f);
		laser.SetVertexCount(2);
		laser.enabled = false;

		setTurretAngle(Random.Range(0, 360));
	}

	void Update () {
		if(barrelAnimator.age() > 0.2f) laser.enabled = false;

		Vector3? target = targetMouse();
		// is the target within range
		if(Input.GetMouseButton(0) && target.HasValue && withinRange(target.Value)) {
			// rotate towards tower, return whether within firing angle
			bool canFireUpon = pointGunsToward(target.Value);
			// if it can fire,
			if(canFireUpon && barrelAnimator.isReady()) {
				barrelAnimator.fire();
				laser.SetPosition(0, barrelBone.position + new Vector3(0,5,0) );
				laser.enabled = true;
				laser.SetPosition(1, target.Value + new Vector3(0,5,0));
			}
		}

		// update gun barrel easing functions
		barrelAnimator.update();
		// set gun barrel progress
		setBarrelProgress(1-barrelAnimator.getProgress());
	}
	
	#endregion
	#region MISC ========================================================================== //

	private void mapBones () {
		Transform a = transform.Find ("Armature");
		turretBone = a.Find("turret");
		barrelBone = turretBone.Find("gun");
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
