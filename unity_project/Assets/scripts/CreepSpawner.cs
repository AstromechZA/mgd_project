using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepSpawner : MonoBehaviour {

	public float spawnInterval = 1f;
	public GameObject creepObject;
	public GameObject citadelObject = null;
	public AudioClip wave_start;

	private float timeToSpawn = 0f;

	void Start()
	{
		//AudioSource.PlayClipAtPoint (wave_start, Camera.main.transform.position);
	}

	void Update () {
		timeToSpawn -= Time.deltaTime;

		if (timeToSpawn <= 0f) {

			// Do spawn.
			if(creepObject != null && citadelObject != null){
				SpawnCreep();
			}

			timeToSpawn = spawnInterval;
		}
	}

	private void SpawnCreep(){
		GameObject creep = Instantiate(creepObject, transform.position, Quaternion.identity) as GameObject;
		AstarAI creepAI = creep.GetComponent<AstarAI>();
		creepAI.navigateToTarget(citadelObject);
	}
}
