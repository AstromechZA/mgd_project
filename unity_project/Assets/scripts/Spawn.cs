using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
	// Spawn enemies on an interval
	public float interval = 0.4f;
	float timeLeftover = 0.0f;
	
	// Object to spawn
	public GameObject PseudoSpider = null;
	
	// Destination of spawned objects
	public GameObject destination = null;
	
	void Update () {
		// Check if it is time to spawn the next enemy
		timeLeftover -= Time.deltaTime;
		if (timeLeftover <= 0.0f) {

			// Bug fix: (might not be needed for other models), try transform.position instead of pos
			// Create Spider position, Shift the spider down
			Vector3 pos = new Vector3(transform.position.x,(float)-3.5,transform.position.z);

			// Spawn spider instance
			GameObject spider = (GameObject)Instantiate(PseudoSpider, pos, Quaternion.identity);
			
			// Access the navmesh agent component of the spider
			NavMeshAgent nma = spider.GetComponent<NavMeshAgent>();
			nma.destination = destination.transform.position;
			
			// Reset the timer
			timeLeftover = interval;
		}
	}
}