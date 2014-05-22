using UnityEngine;
using System.Collections;

public abstract class BaseTowerController : MonoBehaviour {

	protected float range = 0f;
	protected float damage = 0f;
	protected float fireRate = 0f;

	protected AbstractCreep currentTarget;

	void Awake(){
		TowerProperties tp = GetComponent<TowerProperties>();
		this.range = tp.range;
		this.damage = tp.damage;
		this.fireRate = tp.fireRate;
	}

	public abstract void Start ();

	public abstract void Update ();

	protected bool WithinRange(Vector3 t) {
		return (transform.position - t).sqrMagnitude < this.range * this.range;
	}

	// Function for damaging creeps. Use deltaTime when you want a damage per second effect.
	protected void Fire(bool useDeltaTime = true){
		if (currentTarget != null) {
			if(useDeltaTime){
				currentTarget.Hit (this.damage * Time.deltaTime);
			}else{
				currentTarget.Hit (this.damage);
			}
		} else {
			Debug.LogWarning("Fire() method called with no target associated to the tower.");
		}
	}
}
