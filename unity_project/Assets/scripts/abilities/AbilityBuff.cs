using UnityEngine;
using System.Collections;

public class AbilityBuff : MonoBehaviour
{
		public GameObject buffEffect;
		public AudioClip sound_cast;
		public AudioClip sound_invalid;
		private Vector3 screenPoint;
		public bool castable = true;
		public float nextCast = 0;
		public float cooldown = 10.0F;
		public float buffTime = 5.0F;
		public float buffAmount = 2.0F; // Attack 2x faster
		public bool buffed = false;
		Transform target;
		Vector3 startPos;
		Vector3 startScale;
		Color startColor;
		bool targetAbilityUsed = false;
	
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
				
						castable = false;
						nextCast = Time.time + cooldown;

						Vector3 dropSpot = transform.position;
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;
			
						GameObject[] towers = GameObject.FindGameObjectsWithTag ("Instantiable Object");
						for (int i =0; i < towers.Length; i++) {
								if (towers [i].GetComponent<TowerProperties> ()) { // check if it's a tower
										if (Vector3.Distance (dropSpot, towers [i].transform.position) < 30) {
												StartCoroutine ("buff", towers [i]);
										}
								}
						}
						if (!targetAbilityUsed) {
								AchievementController.Instance.uniqueTargetAbilitiesUsed++;
								targetAbilityUsed = true;
						}
				}
		}

		IEnumerator buff (GameObject tower)
		{
				Debug.Log ("Buffed a tower!");
						
				// Store initial firerate
				float startRate = tower.GetComponent<TowerProperties> ().fireRate;

				// Buff tower
				tower.GetComponent<TowerProperties> ().fireRate = startRate / buffAmount;
						
				// Set animation rates
				if (tower.GetComponent<GunTowerController> ())
						tower.GetComponent<GunTowerController> ().setFireRate (startRate / buffAmount);
				else if (tower.GetComponent<MissileTowerController> ())
						tower.GetComponent<MissileTowerController> ().setOpenRate (startRate / buffAmount);
				else if (tower.GetComponent<SniperTowerController> ())
						tower.GetComponent<SniperTowerController> ().setFireRate (startRate / buffAmount);

				GameObject buff = (GameObject)Instantiate (buffEffect, new Vector3 (tower.transform.position.x, buffEffect.transform.position.y, tower.transform.position.z), tower.transform.rotation);
				yield return new WaitForSeconds (buffTime);
				Destroy (buff);

				// Reset to starting firerate
				tower.GetComponent<TowerProperties> ().fireRate = startRate;

				// Animation rates reset
				if (tower.GetComponent<GunTowerController> ())
						tower.GetComponent<GunTowerController> ().setFireRate (startRate);
				else if (tower.GetComponent<MissileTowerController> ())
						tower.GetComponent<MissileTowerController> ().setOpenRate (startRate);
				else if (tower.GetComponent<SniperTowerController> ())
						tower.GetComponent<SniperTowerController> ().setFireRate (startRate);
		}
}
