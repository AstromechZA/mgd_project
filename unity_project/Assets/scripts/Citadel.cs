﻿using UnityEngine;
using System.Collections;

public class Citadel : MonoBehaviour {

	void Update() {
		// find all enemies
		Enemy[] enemies = (Enemy[])FindObjectsOfType(typeof(Enemy));
		if (enemies != null) {
			// find all enemies that are close to the castle
			for (int i = 0; i < enemies.Length; ++i) {
				float range = Mathf.Max(collider.bounds.size.x, collider.bounds.size.z)+2;
				if (Vector3.Distance(transform.position, enemies[i].transform.position) <= range) {            
					// decrease castle health
					Player.castleHealth = Player.castleHealth - 1;
					
					// destroy enemy
					Destroy(enemies[i].gameObject);
				} 
			}
		}
	}

	void checkIfEnemyReachedCitadel(){
		// Find all objects which are enemies
		Enemy[] enemies = (Enemy[])FindObjectsOfType(typeof(Enemy));
		if (enemies != null) {
			// Check which enemies are in range of the citadel
			enemiesInRange(enemies);
		}
	}

	void enemiesInRange(Enemy[] enemies){
		// Check to see which enemies are in range of the citadel
		for (int i = 0; i < enemies.Length; ++i) {
			// Range created from citadel mesh collider
			float range = Mathf.Max(collider.bounds.size.x, collider.bounds.size.z)+2;

			// If an enemy is in range: Decrease the citadel's health and destroy the enemy
			if (Vector3.Distance(transform.position, enemies[i].transform.position) <= range) {            
				Player.castleHealth = Player.castleHealth - 1;
				Destroy(enemies[i].gameObject);
			} 
		}
	}
}