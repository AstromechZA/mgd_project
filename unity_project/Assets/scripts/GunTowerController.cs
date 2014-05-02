using UnityEngine;
using System.Collections;

public class GunTowerController : MonoBehaviour {
	
	public float fireRate = 0.5f;
	public float turnRate = 50;
	public float range = 6;
	 
	// bones
	private Transform turretBone;
	private Transform barrelRBone;
	private Transform barrelLBone;
	
	// barrel
	private DualBarrelGun barrelAnimator;
	
	private Quaternion baseQuat = new Quaternion(0, 0, -0.7f, 0.7f);

	private LineRenderer laser;

	void Start () {
		mapBones ();

		barrelAnimator = new DualBarrelGun(fireRate);

		_setTurretAngle(Random.Range(0, 360));

		laser = (LineRenderer)gameObject.AddComponent("LineRenderer");
		laser.material = new Material(Shader.Find("Particles/Additive"));
		laser.SetColors(Color.red, Color.yellow);
		laser.SetWidth(0.1f, 0.05f);
		laser.SetVertexCount(2);
		laser.enabled = false;

	}

	
	void Update () {
		if(barrelAnimator.age() > 0.2f) laser.enabled = false;

		Vector3? target = _targetMouse();
		if(target.HasValue && _withinRange(target.Value)) {
			bool canFireUpon = pointGunsToward(target.Value);
			if(canFireUpon && Input.GetMouseButton(0)) {
				if(barrelAnimator.isReady()) {

					int i = barrelAnimator.fire();
					if( i == 1) {
						laser.SetPosition(0, barrelRBone.position);
					} else {
						laser.SetPosition(0, barrelLBone.position);
					}
					laser.enabled = true;

					laser.SetPosition(1, target.Value);
				}
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
		turretBone = a.Find("turrent");
		barrelRBone = turretBone.Find("branch_R").Find("gun_R_0");
		barrelLBone = turretBone.Find("branch_L").Find("gun_L_0");
	}

	public void pointGunsAt(Vector3 p) {
		Vector3 dp = transform.position - p;
		if (dp.magnitude > 0) {
			float a = 180-Vector3.Angle(Vector3.right, dp);
			if (dp.z > 0) {a = -a;}
			_setTurretAngle(a);

		}
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
				_setTurretAngle(a);
				return true;
			} else {
				float dir = diff/mag;
				_incrementTurretAngle(dir * turnRate * Time.deltaTime);
				return false;
			}


		}
		return false;
		
	}

	private bool _withinRange(Vector3 t) {
		return (transform.position - t).magnitude < this.range;
	}

	private Vector3? _targetMouse() {
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane hp = new Plane(Vector3.up, Vector3.zero);
		float d = 0;
		if (hp.Raycast(r, out d)) {
			return r.GetPoint(d);
		}
		return null;
	}


	#region Turret Rotation
	// -------------------------------------------------------------------------------------------------------------
	
	private void _incrementTurretAngle (float degrees) {
		turretBone.rotation *= Quaternion.Euler(degrees, 0, 0);
	}
	
	private void _setTurretAngle (float degrees) {
		turretBone.rotation = baseQuat * Quaternion.Euler(90+degrees, 0, 0);
	}
	
	private float _getTurretAngle () {
		return 360-turretBone.rotation.eulerAngles.y-90;
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
		
		private int current = 0;
		private int next = 1;
		private float[] progress = new float[2]{1.0f, 1.0f};
		
		float timeBetweenShots = 1.0f;
		
		public DualBarrelGun(float timeBetweenShots) {
			this.timeBetweenShots = timeBetweenShots;
		}

		public float age () {
			return progress[current];
		}

		public bool isReady () {
			return progress[next] >= 1.0f && progress[current] > 0.5f;
		}
		
		public int fire () {
			progress[next] = 0.0f;
			int t = next;
			next = current;
			current = t;
			return current;
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