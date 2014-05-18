using UnityEngine;
using System.Collections;

public class AbilityFreeze : MonoBehaviour
{
	public AudioClip sound_cast;
	private Vector3 screenPoint;
		public bool castable = true;
		public float nextCast = 0;
		public float slowTime = 3.0F;
		public float slowAmount = 0.2F; // 20% movement speed
		public float cooldown = 10.0F;
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
				if (castable) 
						screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
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
			AudioSource.PlayClipAtPoint(sound_cast, Camera.main.transform.position);  // plays dilator_cast

			
						castable = false;
						nextCast = Time.time + cooldown;

						Vector3 dropSpot = transform.position;
						transform.position = startPos;
						transform.localScale = startScale;
						renderer.material.color = Color.black;

		
		

						GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Instantiable Object");
						for (int i =0; i < enemies.Length; i++) {
								if (enemies [i].GetComponent<AstarAI> ()) {
										if (Vector3.Distance (dropSpot, enemies [i].transform.position) < 20) {
												StartCoroutine ("freeze", enemies [i]);
										}
								}
						}
				}
		}
	
		IEnumerator freeze (GameObject enemy)
		{
				Debug.Log ("Froze an enemy!");
				float startSpeed = enemy.GetComponent<AstarAI> ().speed;
				enemy.GetComponent<AstarAI> ().speed = startSpeed * slowAmount;
				yield return new WaitForSeconds (slowTime);
				enemy.GetComponent<AstarAI> ().speed = startSpeed; 
		}
}