using UnityEngine;
using System.Collections;

public class AbilityBuff : MonoBehaviour
{
	public AudioClip sound_cast;
		private Vector3 screenPoint;
		public bool castable = true;
		public float nextCast = 0;
		public float cooldown = 10.0F;
		public float buffTime = 5.0F;
		public float buffAmount = 2.0F; // Attack 2x faster
		public bool buffed = false;
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
				}
		}
	
		void OnMouseDrag ()
		{
				if (castable) {
						Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
						Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
						transform.position = currentPos;
						transform.localScale = new Vector3 (40, 2.5F, 40);
				}
		}
	
		void OnMouseUp ()
		{
				if (castable) {
						
					AudioSource.PlayClipAtPoint (sound_cast, Camera.main.transform.position);
			
						castable = false;
						nextCast = Time.time + cooldown;

						Vector3 dropSpot = transform.position;
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;

						Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);


			
						GameObject[] towers = GameObject.FindGameObjectsWithTag ("Instantiable Object");
						for (int i =0; i < towers.Length; i++) {
								if (towers [i].GetComponent<TowerProperties> ()) { // check if it's a tower
										if (Vector3.Distance (dropSpot, towers [i].transform.position) < 20) {
												StartCoroutine ("buff", towers [i]);
										}
								}
				
						}
			
						Destroy (placementVisualiser);
				}
		}

		IEnumerator buff (GameObject enemy)
		{
				if (!buffed) {
						Debug.Log ("Buffed a tower!");
						
						// Store initial firerate
						float startRate = enemy.GetComponent<TowerProperties> ().fireRate;

						// Buff tower
						enemy.GetComponent<TowerProperties> ().fireRate = startRate / buffAmount;
						// TODO: change tower visual while buffed
						buffed = true;

						// Wait for the buff time
						yield return new WaitForSeconds (buffTime);

						// Reset to starting firerate
						enemy.GetComponent<TowerProperties> ().fireRate = startRate;
						buffed = false;
				}
		}
}
