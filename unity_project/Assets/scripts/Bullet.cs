using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	// Velocity of Bullet
	public float velocity = 10.0f;
	
	// Destination of bullet (set by Tower when the bullet is created)
	Transform destination;    

	void Update () {

		// If the destination no longer exists, destroy the bullet
		if (destination == null) {
			Destroy(gameObject);
			return;
		}
		
		// Move towards destination
		float distanceToMove = Time.deltaTime * velocity;
		transform.position = Vector3.MoveTowards(transform.position, destination.position, distanceToMove);
		
		// Check if the bullet has reached the destination
		if (transform.position.Equals(destination.position)) {
			// Decrease enemies health
			Enemy enemy = destination.GetComponent<Enemy>();
			enemy.health = enemy.health - 1;
			
			// If enemies health is less than or equal to 0 (kill enemy - call onDeath())
			if (enemy.health <= 0)         
				enemy.onDeath();            
			
			// Destroy the bullet object
			Destroy(gameObject);
		}
	}
	
	public void setDestination(Transform dest) {
		destination = dest;
	}
}
