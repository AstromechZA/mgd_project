using UnityEngine;
using System.Collections;
using Pathfinding;

public class PathObstacle : MonoBehaviour {

	public void UpdateGraphForObject(){
		if (collider != null) {
			GraphUpdateObject go = new GraphUpdateObject (collider.bounds);
			AstarPath.active.UpdateGraphs (go);
			AstarPath.active.FlushGraphUpdates ();
		}
	}
}
