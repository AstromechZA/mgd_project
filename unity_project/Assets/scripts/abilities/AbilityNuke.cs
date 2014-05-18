using UnityEngine;
using System.Collections;

public class AbilityNuke : MonoBehaviour
{
	public AudioClip sound_cast;
	public AudioClip sound_start;
		private Vector3 screenPoint;
		public bool castable = true;
		public float nextCast = 0;
		public float cooldown = 10.0F;
		public int damage = 4;
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
	
	IEnumerator OnMouseUp ()
		{
				if (castable) {
						
						AudioSource.PlayClipAtPoint (sound_start, Camera.main.transform.position);
						yield return new WaitForSeconds(sound_start.length);
			
						castable = false;
						nextCast = Time.time + cooldown;

						Vector3 dropSpot = transform.position;
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;

						Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

						GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Instantiable Object");

						AudioSource.PlayClipAtPoint (sound_cast, Camera.main.transform.position);
						for (int i =0; i < enemies.Length; i++) {
								if (enemies [i].GetComponent<AstarAI> ()) { // Checks if it's a creep
										if (Vector3.Distance (dropSpot, enemies [i].transform.position) < 20) {
												// Do damage to creep here
												Debug.Log ("Nuked an enemy!");
												enemies [i].renderer.material.color = Color.red; // placeholder for testing
										}
								}
				
						}
				}
		}
}