using UnityEngine;
using System.Collections;

public class AbilityFreeze : MonoBehaviour
{
		// Sound effects
		public AudioClip sound_cast;
		public AudioClip sound_invalid;
		
		// Freezing
		private bool castable = true;
		private float nextCast = 0;
		private float slowTime = 3.0F;
		private float slowAmount = 0.2F;
		private float cooldown = 10.0F;

		// Placement
		private Vector3 screenPoint;
		private Vector3 startPos;
		private Vector3 startScale;
		private Color startColor;
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
				if (castable) 
						screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
				else {
						AudioSource.PlayClipAtPoint (sound_invalid, Camera.main.transform.position);
				}
		}
	
		void OnMouseDrag ()
		{
				if (castable) {
						Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
						Vector3 currentPos = Camera.main.ScreenToWorldPoint (currentScreenPoint);
						transform.position = currentPos;
						transform.localScale = new Vector3 (40, 2.5f, 40);
				}
		}
	
		void OnMouseUp ()
		{
				if (castable) {
						AudioSource.PlayClipAtPoint (sound_cast, Camera.main.transform.position);  // plays dilator_cast

						Vector3 dropSpot = transform.position;

						// Not castable, store next time the ability can be cast
						castable = false;
						nextCast = Time.time + cooldown;

						// Reset ability position, scale and change color to black
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;

						GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
						for (int i =0; i < enemies.Length; i++) {
								if (enemies [i].GetComponent<AstarAI> ()) { // If it is an enemy
										if (Vector3.Distance (dropSpot, enemies [i].transform.position) < 20) {
												StartCoroutine ("freeze", enemies [i]);
										}
								}
						}

						// Achievements
						if (!targetAbilityUsed) {
								AchievementController.Instance.uniqueTargetAbilitiesUsed++;
								targetAbilityUsed = true;
						}
				}

		}
	
		IEnumerator freeze (GameObject enemy)
		{
				// Store starting speed
				float startSpeed = enemy.GetComponent<AstarAI> ().speed;
				
				// If a normal enemy
				if (enemy.transform.FindChild ("model/Cube")) {
						
						// Change speed and colour
						Color creepColor = enemy.transform.FindChild ("model/Cube").renderer.material.color;
						enemy.transform.FindChild ("model/Cube").renderer.material.color = Color.blue;
						enemy.GetComponent<AstarAI> ().speed = startSpeed * slowAmount;

						// Wait for slowTime
						yield return new WaitForSeconds (slowTime);

						// If the enemy still exists, restore speed and colour
						if (enemy) {
								enemy.GetComponent<AstarAI> ().speed = startSpeed;
								enemy.transform.FindChild ("model/Cube").renderer.material.color = creepColor;
						}

				}
		
				// If the enemy is a boss, only change speed
				else {
						enemy.GetComponent<AstarAI> ().speed = startSpeed * slowAmount;
						yield return new WaitForSeconds (slowTime);
						if (enemy) 
								enemy.GetComponent<AstarAI> ().speed = startSpeed;
				}
		}

		public void setSlowAmount (float amount)
		{
				slowAmount = amount;
		}

		public float getSlowAmount ()
		{
				return slowAmount;
		}
}