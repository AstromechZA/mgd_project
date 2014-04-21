using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	// Enemies health
	public int health = 5;
	public float xp_reward = 2.0F;
	public int energy_reward = 1;
	public XpBar xp_bar;
	
	public void onDeath() {        
		// Increase players experience level
		xp_bar = new XpBar();
		xp_bar.addXp (xp_reward);
		Player.energy += energy_reward;
		// Destroy the enemy object
		Destroy(gameObject); 
	}
}
