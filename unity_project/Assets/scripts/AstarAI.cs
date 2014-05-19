using UnityEngine;
using System.Collections;
using Pathfinding;

public class AstarAI : MonoBehaviour {
	//The point to move to
	private Vector3 targetPosition;
	private GameObject targetObject;	
	private Seeker seeker;
	private Path path;
	
	// Sounds
	public AudioClip citadel_hit;
	
	public float targetReachedDistance = 1f;
	
	//The AI's speed per second
	public float speed = 1;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	private float nextWaypointDistanceSqrd;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	public void Awake(){
		seeker = GetComponent<Seeker>();
		
		// Register callback to update creeps path when a tower placement affects the graph.
		AstarPath.OnGraphsUpdated += RecalculatePath;
	}
	
	// This is where you set the creep's target (i.e. the citadel) and start moving.
	public void DestroyTarget(GameObject theTargetObject){
		targetObject = theTargetObject;
		targetPosition = targetObject.transform.position;
		nextWaypointDistanceSqrd = nextWaypointDistance * nextWaypointDistance;
		
		RecalculatePath (null);
	}
	
	public void RecalculatePath(AstarPath script){
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position, targetPosition, OnPathComplete);
	}
	
	public void OnPathComplete (Path p) {
		Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			path.Claim(this);
			
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	
	public void Update () {
		//We have no path to move after yet
		if (path == null) return;

		// We've come to the end of our path. Add explode logic here.
		if (currentWaypoint >= path.vectorPath.Count) {
			if ((transform.position - targetPosition).sqrMagnitude <= targetReachedDistance * targetReachedDistance){
				OnTargetReached();
			}else{
				OnTargetNotReached();
			}
			
			Destroy(gameObject);
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.deltaTime;
		transform.position += dir;

		float a = Vector3.Angle(dir, Vector3.right);
		if (dir.z > 0) {a = -a;}

		transform.rotation = Quaternion.Euler(0, a+90, 0);



		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistanceSqrd) {
			currentWaypoint++;
			return;
		}
	}
	
	private void OnTargetReached(){
		// Add logic here when creep reaches citadel.
		AudioSource.PlayClipAtPoint (citadel_hit, Camera.main.transform.position);
		// Decrement Citadel Lives
		GameController.Instance.citadelLives--;
		Debug.Log ("Creep reached citadel.");
	}
	
	private void OnTargetNotReached(){
		// Add logic here when creeps reach end of path but not citadel.
		
		Debug.Log ("Creep could not reach citadel.");
	}
	
	public void OnDestroy () {
		seeker.pathCallback -= OnPathComplete;
		AstarPath.OnGraphsUpdated -= RecalculatePath;
		if (path != null) {
			path.Release (this);
		}
	} 
} 