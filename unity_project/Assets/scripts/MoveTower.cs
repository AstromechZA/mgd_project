using UnityEngine;
using System.Collections;

// from http://unityshorttutorials.blogspot.com/2013/11/drag-and-drop-in-unity.html
public class MoveTower : MonoBehaviour {

	public int move_cost = 1;
	
	private Vector3 screenPoint;
	void OnMouseDown () {
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
	}
	
	void OnMouseDrag (){
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
		transform.position = currentPos;
		gameObject.GetComponent<Tower> ().enabled = false; // can't shoot while dragging
		gameObject.GetComponent<NavMeshObstacle> ().enabled = false; // can't block while dragging			
	}
	
	void OnMouseUp () {
		Player.energy -= move_cost; // cost to move
		gameObject.GetComponent<Tower>().enabled = true; // allow shooting again
		gameObject.GetComponent<NavMeshObstacle> ().enabled = true; // allow blocking again
	}
}
