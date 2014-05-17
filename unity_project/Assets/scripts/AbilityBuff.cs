using UnityEngine;
using System.Collections;

public class AbilityBuff : MonoBehaviour
{
		private Vector3 screenPoint;
		public bool castable = true;
		public float nextCast = 0;
		public float cooldown = 3.0F;
		public float buffTime = 5.0F;
		public float buffAmount = 2.0F; // Attack 2x faster
		public bool buffed = false;
		GameObject placementVisualiser;
		Transform target;
		Vector3 startPos;
		Vector3 startScale;
		Color startColor;
		Color tower_startColor;
	
		void Start ()
		{
				startPos = transform.position;
				startScale = transform.localScale;
				startColor = renderer.material.color;
				tower_startColor = GameObject.Find ("Buy_Tower1").renderer.material.color;
		}
	
		void Update ()
		{
				if (Time.time > nextCast) {
						castable = true;
						renderer.material.color = startColor;
				}
		}
	
		void OnMouseDown ()
		{
				if (castable) {
						screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
						placementVisualiser = (GameObject)Instantiate (Resources.Load ("PlacementVisualisation"), new Vector3 (gameObject.transform.position.x, (float)-2.031304, gameObject.transform.position.z), gameObject.transform.rotation);
				}
		}
	
		void OnMouseDrag ()
		{
				if (castable) {
						Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
						Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
						transform.position = currentPos;
						transform.localScale = new Vector3 (2, 0.1f, 2);
						gameObject.GetComponent<BoxCollider> ().enabled = false; // prevent collision during placement
			
						placementVisualiser.transform.position = new Vector3 (currentPos.x, (float)-2.031304, currentPos.z);
			
				}
		}
	
		IEnumerator OnMouseUp ()
		{
				if (castable) {
			
						gameObject.GetComponent<BoxCollider> ().enabled = true;
						yield return new WaitForSeconds (0.1f);
			
						castable = false;
						nextCast = Time.time + cooldown;
			
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;
			
						Destroy (placementVisualiser);
				}
		}

		void OnCollisionEnter (Collision col)
		{
				if (col.gameObject.tag == "Tower")
						StartCoroutine ("buff", col);	
		}
	
		IEnumerator buff (Collision col)
		{
				if (!buffed) {
						Debug.Log ("Buffed a tower!");
						
						// Store initial interval
						float startInterval = col.gameObject.GetComponent<Tower> ().interval;

						// Buff tower
						col.gameObject.GetComponent<Tower> ().interval = col.gameObject.GetComponent<Tower> ().interval / buffAmount;
						col.gameObject.GetComponent<Tower> ().renderer.material.color = Color.magenta;
						buffed = true;

						// Wait for the buff time
						yield return new WaitForSeconds (buffTime);

						// Reset to starting interval and colour
						col.gameObject.GetComponent<Tower> ().interval = startInterval;
						col.gameObject.GetComponent<Tower> ().renderer.material.color = tower_startColor;
						buffed = false;
				}
		}
}
