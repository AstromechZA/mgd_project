using UnityEngine;
using System.Collections;

public class PlaceBuff : MonoBehaviour {
	
	public int timer;
	public int buff_time = 5;
	public float buff_amount = 2; // attack twice as fast
	GameObject placementVisualiser;
	Transform target; 
	
	private Vector3 screenPoint;
	void OnMouseDown () {
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		Instantiate (gameObject);
		// Create Placement Visualiser at the same position and rotation as the tower (use y-position of Placement Visualiser)
		// Load placement visualisation from Resources folder
		placementVisualiser = (GameObject)Instantiate(Resources.Load("PlacementVisualisation"), new Vector3(gameObject.transform.position.x, (float)-2.031304, gameObject.transform.position.z), gameObject.transform.rotation);
	}
	
	void OnMouseDrag () {
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
		transform.position = currentPos;
		transform.localScale = new Vector3(2, 0.1f, 2);
		gameObject.GetComponent<BoxCollider> ().enabled = false; // prevent collision during placement
		
		// Update the position of the Placement Visualiser, use the towers position with the Placement Visualisers y-position)
		placementVisualiser.transform.position = new Vector3 (currentPos.x, (float)-2.031304, currentPos.z);
	}
	
	IEnumerator OnMouseUp () {
		
		Color color = Color.blue;
		renderer.material.color = color;
		gameObject.GetComponent<BoxCollider> ().enabled = true; //allow for collisions
		
		yield return new WaitForSeconds(0.1f);
		Destroy (gameObject);
		
		
		Destroy (this); 
		// Destroy the Placement Visualiser
		Destroy(placementVisualiser);
	}
	
	void OnCollisionEnter(Collision col)
	{
		
		if (col.gameObject.tag == "Tower") 
		{
			Debug.Log ("Buffed a tower!");
			col.gameObject.GetComponent<Tower>();
			col.gameObject.GetComponent<Tower>().interval = col.gameObject.GetComponent<Tower>().interval/buff_amount;
		}
	}
}