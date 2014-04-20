using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	// Enemies health
	public int health = 5;
	
	public void onDeath() {        
		// Increase players experience level
		Player.experience = Player.experience + 10;
		
		// Destroy the enemy object
		Destroy(gameObject); 
	}
}
