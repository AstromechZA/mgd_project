using UnityEngine;
using System.Collections;

public class AbilityNuke : MonoBehaviour
{
		public GameObject nukeEffect;
		public AudioClip sound_cast;
		public AudioClip sound_start;
		public AudioClip sound_invalid;
		private Vector3 screenPoint;
		public bool castable = true;
		public float nextCast = 0;
		public float cooldown = 10.0F;
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
				} else {
						// Stops error sound from playing if player touches AOE during countdown
						if (Time.time > (nextCast - cooldown + sound_start.length)) { 
								AudioSource.PlayClipAtPoint (sound_invalid, Camera.main.transform.position);
						}
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
						castable = false;
						nextCast = Time.time + cooldown;
						AudioSource.PlayClipAtPoint (sound_start, Camera.main.transform.position);
						yield return new WaitForSeconds (sound_start.length);
						
						// Deal damage to enemies in area of effect
						Vector3 dropSpot = transform.position;
						GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Instantiable Object");
						for (int i =0; i < enemies.Length; i++) {
								if (enemies [i] && enemies [i].GetComponent<AstarAI> ()) { // Checks if it's a creep
										if (Vector3.Distance (dropSpot, enemies [i].transform.position) < 20) {
												Debug.Log ("Nuked an enemy!");
												enemies [i].GetComponent<AbstractCreep> ().Hit (50);
										}
					
								}
						}

						// Playsound and show nuke effect
						AudioSource.PlayClipAtPoint (sound_cast, Camera.main.transform.position);
						GameObject nuke = (GameObject)Instantiate (nukeEffect, transform.position, transform.rotation);
						
						// Restore ability position
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;

						// Wait for sound to finish then destroy effect
						yield return new WaitForSeconds (sound_cast.length);
						Destroy (nuke);
				}
		}
}









