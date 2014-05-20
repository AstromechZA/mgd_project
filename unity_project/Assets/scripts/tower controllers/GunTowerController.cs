using UnityEngine;
using System.Collections;

public class GunTowerController : BaseTowerController {

	public float turnRate = 50;
	public Material laserMaterial;
	public float laserWidthMin = 1f;
	public float laserWidthMax = 2f;

	// retrieved from TowerProperties
	private float fireRate;
	private float range;
	private float cost;
	 
	// bones
	private Transform turretBone;
	private Transform barrelRBone;
	private Transform barrelLBone;
	
	// barrel
	private DualBarrelGun barrelAnimator;
	
	private Quaternion baseQuat = new Quaternion(0, 0, -0.7f, 0.7f);

	private LineRenderer laser;
	private Vector3 heightOffset = new Vector3(0, 50, 0);

	public AudioClip sound_fire;

	public override void Start () {
		mapBones ();

		TowerProperties tp = GetComponent<TowerProperties> ();
		fireRate = tp.fireRate;
		range = tp.range;
		cost = tp.cost;

		barrelAnimator = new DualBarrelGun(fireRate);

		_setTurretAngle(Random.Range(0, 360));

		laser = (LineRenderer)gameObject.AddComponent("LineRenderer");
		laser.material = laserMaterial;
		laser.SetColors(Color.red, Color.yellow);
		laser.SetWidth(laserWidthMax, laserWidthMin);
		laser.SetVertexCount(2);
		laser.enabled = false;
	}

	
	public override void Update () {
		// remove laser beam if it is too old
		if(barrelAnimator.age() > 0.2f) laser.enabled = false;

		// aquire target (using mouse position for now)
		AbstractCreep closest = NearestCreepFinder.Instance.getNearest(transform.position);

		// is the target within range
		if(closest != null && _withinRange(closest.transform.position)) {
			// rotate towards tower, return whether within firing angle
			bool canFireUpon = pointGunsToward(closest.transform.position);
			// if it can fire,
			if(canFireUpon && barrelAnimator.isReady()) {
				// FIRE, return which barrel fired
				AudioSource.PlayClipAtPoint (sound_fire, Camera.main.transform.position);

				int i = barrelAnimator.fire();
				laser.SetPosition(0, 
				    ((i == 0) ? barrelLBone.position : barrelRBone.position) + heightOffset
          		);
				laser.enabled = true;

				laser.SetPosition(1, closest.transform.position + heightOffset);
			}
		}
		// update gun barrel easing functions
		barrelAnimator.update();
		// set gun barrel progress
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
	
	public bool pointGunsToward(Vector3 p) {
		
		Vector3 dp = transform.position - p;
		if (dp.magnitude > 0) {
			float a = 180-Vector3.Angle(Vector3.right, dp);
			if (dp.z > 0) {a = -a;}

			float diff = a -_getTurretAngle();
			diff = diff % 360;
			if (diff < -180) diff += 360;

			float mag = Mathf.Abs(diff);
			if ( mag < 5f) {
				_setTurretAngle(a);
				return true;
			}

			float dir = diff/mag;
			_incrementTurretAngle(dir * turnRate * Time.deltaTime);
			return false;


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