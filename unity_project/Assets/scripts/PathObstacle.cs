using UnityEngine;
using System.Collections;
using Pathfinding;

public class PathObstacle : MonoBehaviour {

	public void UpdateGraphForObject(GraphUpdateObject go = null){
		if (go != null) {
			AstarPath.active.UpdateGraphs (go);
			AstarPath.active.FlushGraphUpdates ();
		} else if (collider != null) {
			go = new GraphUpdateObject (collider.bounds);
			AstarPath.active.UpdateGraphs (go);
			AstarPath.active.FlushGraphUpdates ();
		}
	}
}
