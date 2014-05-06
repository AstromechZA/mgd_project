using UnityEngine;
using System.Collections;
using Pathfinding;

public class AstarAI : MonoBehaviour {
	//The point to move to
	private Vector3 targetPosition;
	private GameObject targetObject;	
	private Seeker seeker;
	private Path path;
	
	//The AI's speed per second
	public float speed = 1;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	private float nextWaypointDistanceSqrd;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	public void Awake(){
		seeker = GetComponent<Seeker>();
	}

	// This is where you set the creep's target (i.e. the citadel) and start moving.
	public void DestroyTarget(GameObject theTargetObject){
		targetObject = theTargetObject;
		targetPosition = targetObject.transform.position;
		
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position,targetPosition, OnPathComplete);
		nextWaypointDistanceSqrd = nextWaypointDistance * nextWaypointDistance;
	}
	
	public void OnPathComplete (Path p) {
		Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	
	public void Update () {
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			Destroy(gameObject);
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.deltaTime;
		transform.position += dir;
		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistanceSqrd) {
			currentWaypoint++;
			return;
		}
	}

	public void OnDisable () {
		seeker.pathCallback -= OnPathComplete;
	} 
} 