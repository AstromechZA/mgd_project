using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {
	// Bullet prefab
	public Bullet bulletPrefab = null;
	
	// Interval of fire (rate of fire)
	public float interval = 1.0f;
	float timeLeftover = 0.0f;
	
	// Range of attack
	public float range = 10.0f;

	
	Enemy findClosestEnemy() {
		Enemy closestEnemy = null;
		Vector3 towerPosition = transform.position;
		
		// find all enemies
		Enemy[] enemies = (Enemy[])FindObjectsOfType(typeof(Enemy));
		if (enemies != null) {
			if (enemies.Length > 0) {
				// Set first enemy in array to closest
				closestEnemy = enemies[0];
				// Compare the rest of the array with the closest, replacing it with the closest enemy
				for (int i = 1; i < enemies.Length; ++i) {
					float distToCurrentEnemy = Vector3.Distance(towerPosition, enemies[i].transform.position);
					float distToPreviousClosestEnemy = Vector3.Distance(towerPosition, closestEnemy.transform.position);
					// Check if current enemy distance less than previous
					if (distToCurrentEnemy < distToPreviousClosestEnemy) {
						closestEnemy = enemies[i];
					}
				}
			}
		}		
		return closestEnemy;
	}
	
	void Update() {
		// Check if it is time to shoot the next bullet
		timeLeftover -= Time.deltaTime;
		if (timeLeftover <= 0.0f) {
			// Find the closest enemy
			Enemy target = findClosestEnemy();
			if (target != null) {        
				// Check if the enemy is in range
				if (Vector3.Distance(transform.position, target.transform.position) <= range) {
					// Spawn the bullet
					GameObject gameObject = (GameObject)Instantiate(bulletPrefab.gameObject, transform.position, transform.rotation);
					
					// Get access to the component of the bullet
					Bullet bullet = gameObject.GetComponent<Bullet>();
					
					// Set the destination of the bullet       
					bullet.setDestination(target.transform);

					// Reset the timer
					timeLeftover = interval;
				}
			}
		}
	}
}
