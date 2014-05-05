using UnityEngine;
using System.Collections;
using Pathfinding;

public class AstarAI : MonoBehaviour {
	//The point to move to
	public Vector3 targetPosition;
	public GameObject targetObject;
	
	private Seeker seeker;
	private CharacterController controller;
	
	//The calculated path
	public Path path;
	
	//The AI's speed per second
	public float speed = 1;
	
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	private float nextWaypointDistanceSqrd;
	
	public void Start () {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
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