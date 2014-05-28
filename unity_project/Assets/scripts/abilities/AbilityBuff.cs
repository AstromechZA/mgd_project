using UnityEngine;
using System.Collections;

public class AbilityBuff : MonoBehaviour
{
		// Visual and sound effects
		public GameObject buffEffect;
		public AudioClip sound_cast;
		public AudioClip sound_invalid;
		
		// Buffing
		private bool castable = true;
		private float nextCast = 0;
		private float cooldown = 10.0F;
		private float buffTime = 5.0F;
		private float buffAmount = 2.0F;
		
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
				if (castable) {
						screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
				} else {
						AudioSource.PlayClipAtPoint (sound_invalid, Camera.main.transform.position);
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

						Vector3 dropSpot = transform.position; // Store where the ability was cast

						// Not castable, store next time the ability can be cast
						castable = false;
						nextCast = Time.time + cooldown;
						
						// Reset ability position, scale and change color to black
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;
			
						GameObject[] towers = GameObject.FindGameObjectsWithTag ("Instantiable Object");
						for (int i =0; i < towers.Length; i++) {
								if (towers [i].GetComponent<TowerProperties> ()) { // If it is a tower
										if (Vector3.Distance (dropSpot, towers [i].transform.position) < 30) {
												StartCoroutine ("buff", towers [i]);
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

		IEnumerator buff (GameObject tower)
		{				
				// Store starting firerate
				float startRate = tower.GetComponent<TowerProperties> ().fireRate;

				// Buff tower's firerate
				tower.GetComponent<TowerProperties> ().fireRate = startRate / buffAmount;
						
				// Buff tower's animation rate
				if (tower.GetComponent<GunTowerController> ())
						tower.GetComponent<GunTowerController> ().setFireRate (startRate / buffAmount);
				else if (tower.GetComponent<MissileTowerController> ())
						tower.GetComponent<MissileTowerController> ().setOpenRate (startRate / buffAmount);
				else if (tower.GetComponent<SniperTowerController> ())
						tower.GetComponent<SniperTowerController> ().setFireRate (startRate / buffAmount);

				// Create buff effect and wait for buffTime
				GameObject buff = (GameObject)Instantiate (buffEffect, new Vector3 (tower.transform.position.x, 
		                          buffEffect.transform.position.y, tower.transform.position.z), tower.transform.rotation);
				yield return new WaitForSeconds (buffTime);
				Destroy (buff);

				// Reset firerate
				tower.GetComponent<TowerProperties> ().fireRate = startRate;

				// Animation rates reset
				if (tower.GetComponent<GunTowerController> ())
						tower.GetComponent<GunTowerController> ().setFireRate (startRate);
				else if (tower.GetComponent<MissileTowerController> ())
						tower.GetComponent<MissileTowerController> ().setOpenRate (startRate);
				else if (tower.GetComponent<SniperTowerController> ())
						tower.GetComponent<SniperTowerController> ().setFireRate (startRate);
		}

		public void setBuffAmount (float amount)
		{
				buffAmount = amount;
		}

		public float getBuffAmount ()
		{
				return buffAmount;
		}
}