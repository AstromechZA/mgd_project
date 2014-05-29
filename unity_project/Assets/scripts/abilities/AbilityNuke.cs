using UnityEngine;
using System.Collections;

public class AbilityNuke : MonoBehaviour
{
		// Visual and sound effects
		public GameObject nukeEffectPrefab;
		private GameObject nukeEffect;
		public AudioClip sound_cast;
		public AudioClip sound_start;
		public AudioClip sound_invalid;

		// Nuking
		private bool castable = true;
		private float nextCast = 0;
		private float cooldown = 20.0F;
		private float damage = 1000.0F;

		// Placement
		private Vector3 startPos;
		private Vector3 startScale;
		private Color startColor;
		private Vector3 screenPoint;
		private bool targetAbilityUsed = false; // For achievements

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
						
						// Not castable, store next time the ability can be cast
						castable = false;
						nextCast = Time.time + cooldown;

						// Play sound and wait for it to finish
						AudioSource.PlayClipAtPoint (sound_start, Camera.main.transform.position);
						yield return new WaitForSeconds (sound_start.length);

						// Playsound and show nuke effect
						AudioSource.PlayClipAtPoint (sound_cast, Camera.main.transform.position);
						nukeEffect = Instantiate (nukeEffectPrefab, new Vector3 (transform.position.x, 3, transform.position.z), transform.rotation) as GameObject;
						
						// Deal damage to enemies in area of effect
						Vector3 dropSpot = transform.position;
						GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
						for (int i =0; i < enemies.Length; i++) {
								if (enemies [i] && enemies [i].GetComponent<AstarAI> ()) { // If it's a creep
										if (Vector3.Distance (dropSpot, enemies [i].transform.position) < 20) {
												enemies [i].GetComponent<AbstractCreep> ().Hit (damage);
										}
					
								}
						}

						// Reset ability position, scale and change color to black
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;

						// Wait for sound to finish then destroy effect
						yield return new WaitForSeconds (sound_cast.length);
						Destroy (nukeEffect);

						// Achievements
						if (!targetAbilityUsed) {
								AchievementController.Instance.uniqueTargetAbilitiesUsed++;
								targetAbilityUsed = true;
						}

				}

		}

		public void setDamage (float amount)
		{
				damage = amount;
		}

		public float getDamage ()
		{
				return damage;
		}
}