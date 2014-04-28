using UnityEngine;
using System.Collections;

public class PlaceNuke : MonoBehaviour {
	
	public int timer;
	
	// Placement Visualiser Prefab
	//GameObject placementVisualiserPrefab = null;
	// Placement Visualiser
	public int damage = 4;
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
	
		Color color = Color.red;
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

		if (col.gameObject.tag == "Enemy") 
		{
			Debug.Log ("Nuked an enemy!");
			col.gameObject.GetComponent<Enemy>().health-= damage;

			if (col.gameObject.GetComponent<Enemy>().health <= 0)         
				col.gameObject.GetComponent<Enemy>().onDeath(); 
		}
	}
}
