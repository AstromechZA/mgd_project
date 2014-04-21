using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	// Enemies health
	public int health = 5;
	public float xp_reward = 2.0F;
	public int energy_reward = 1;
	public XpBar xp_bar;

	float timeTillBodyCleanup = 0.2f;
	bool bodyCleanup = false;

	public void onDeath() {     
		transform.animation.Play("death");
		// Increase players experience level
		xp_bar = new XpBar();
		xp_bar.addXp (xp_reward);
		Player.energy += energy_reward;

		// Destroy the object after a delay (allow for death animation to take place)
		bodyCleanup = true;
		 
	}

	void Update()
	{
		// Clean up the enemies body after 0.2 seconds after onDeath() has been called
		if (bodyCleanup) {
			timeTillBodyCleanup -= Time.deltaTime;
			if (timeTillBodyCleanup < 0) {
				Destroy(gameObject);
			}
		}

		// Get spiders navmesh component and if it is in range of the destination (attack_Melee or attack_leap)
		NavMeshAgent spiderNMA = this.GetComponent<NavMeshAgent>();
		if (spiderNMA != null && spiderNMA.active && bodyCleanup == false) {
			if (spiderNMA.remainingDistance < 2 && spiderNMA.pathStatus == NavMeshPathStatus.PathComplete) {
				transform.animation.Play ("attack_Melee");
			}
		}
	}
}



