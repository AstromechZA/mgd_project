using UnityEngine;
using System.Collections;

public class AbilityNuke : MonoBehaviour
{
		private Vector3 screenPoint;
		public bool castable = true;
		public float nextCast = 0;
		public float cooldown = 3.0F;
		public int damage = 4;
		GameObject placementVisualiser;
		Transform target;
		Vector3 startPos;
		Vector3 startScale;
		Color startColor;

		void Start ()
		{
				startPos = transform.position;
				startScale = transform.localScale;
				startColor = renderer.material.color;
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

				if (col.gameObject.tag == "Enemy") {
						Debug.Log ("Nuked an enemy!");
						col.gameObject.GetComponent<Enemy> ().health -= damage;

						if (col.gameObject.GetComponent<Enemy> ().health <= 0)         
								col.gameObject.GetComponent<Enemy> ().onDeath (); 
				}
		}
}
