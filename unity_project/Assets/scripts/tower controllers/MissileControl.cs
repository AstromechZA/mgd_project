using UnityEngine;
using System.Collections;

public class MissileControl : MonoBehaviour {

	public float horizantalSpeed = 5f;
	public float flightDuration = 1.5f; //seconds
	private float damage = 1f;
	public GameObject missileExplodePrefab;
	
	private GameObject missileEffect;
	public float Damage{
		get {
			return damage;
		}
		set{
			damage = value;
		}
	}

	private float damageRadius = 25f;
	public float DamageRadius {
		get {
			return damageRadius;
		}
		set {
			damageRadius = value;
		}
	}

	private Vector3 origin;
	private Vector3 destination;

	private bool inflight = false;
	private bool exploding = false;
	private float explodeAge = 0;
	private float flightProgress = 0.0f;

	private Vector3 flightVector;
	private Quaternion angleQuat;

	void Update () {
		if (inflight && !exploding) {
			flightProgress += Time.deltaTime / flightDuration;

			float sflightProgress = squish(flightProgress);

			transform.localRotation = angleQuat * Quaternion.Euler(0,0,-sflightProgress*180);
			transform.localRotation *= Quaternion.Euler(0, flightProgress*1000, 0);

			Vector3 newpos = origin + sflightProgress * flightVector;
			newpos.y = 10;
			transform.position  = newpos;

			float val = 4 * (-Mathf.Abs(sflightProgress - 0.5f)) + 12;

			transform.localScale = val * Vector3.one;

			float damageRadiusSqr = DamageRadius * DamageRadius;
			foreach (AbstractCreep creep in NearestCreepFinder.Instance.CreepList) {
				if((creep.transform.position - transform.position).sqrMagnitude <= damageRadiusSqr){
					Debug.DrawLine(creep.transform.position, transform.position, Color.red);
				}
			}

			// Missile has landed.
			if (flightProgress > 1){
				// Damage all units within the landing area.
				
				missileEffect = Instantiate (missileExplodePrefab, new Vector3 (transform.position.x, 3, transform.position.z), transform.rotation) as GameObject;
				
				transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
				
				ApplyMissileDamage();
				
				// Wait for sound to finish then destroy effect
				exploding = true;
				
			}
		} else if (exploding) {
			
			explodeAge += Time.deltaTime;
			
			if (explodeAge > 0.5) {
				Destroy (missileEffect);
				
				Destroy(this.gameObject);
			}
			
		}
	}

	private float squish(float t) {
		float tt = (t*2-1);
		return 0.5f + tt/(1+Mathf.Abs(tt));
	}

	public void launch(Vector3 target) {
		this.origin = transform.position;
		this.destination = target;
		this.inflight = true;

		flightVector = (destination - origin);

		angleQuat = Quaternion.Euler(0, -angleTo(this.origin, this.destination), 0);
	}

	private float angleTo(Vector3 from, Vector3 to) {
		Vector3 dp = to - from;
		dp.y = 0;
		float a = Vector3.Angle(Vector3.right, dp);
		if (dp.z < 0) {a = -a;}
		return a;
	}

	private void ApplyMissileDamage(){
		float damageRadiusSqr = DamageRadius * DamageRadius;

		foreach (AbstractCreep creep in NearestCreepFinder.Instance.CreepList) {
			if((creep.transform.position - transform.position).sqrMagnitude <= damageRadiusSqr){
				creep.Hit(Damage + PerkController.Instance.GetPerkBonus(Perk.PerkType.TWR_MISSILE_DMG));
			}
		}
	}
}
