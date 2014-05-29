using UnityEngine;
using System.Collections;

public class SpawnProgress : MonoBehaviour 
{	
	private float initialWaveTimeOffset;
	private float initialWaveTimeMax;


	void OnMouseUp(){
		GameController.Instance.spawnNextWaveEarly = true;
		GameController.Instance.nextWaveSpawnerActive = false;
		Destroy(gameObject);
	}

	void Start(){
		GameController.Instance.nextWaveSpawnerActive = true;
		initialWaveTimeOffset = GameController.Instance.currentDurationOfWave;
		initialWaveTimeMax = GameController.Instance.lengthOfWave - GameController.Instance.currentDurationOfWave;
	}

	void Update()
	{
		setProgress((GameController.Instance.currentDurationOfWave-initialWaveTimeOffset)/initialWaveTimeMax);
	}
	
	// use setProgress anywhere between 0 and 1
	public void setProgress( float v )
	{
		float offset = 0f;

		// Clockwise
		offset = 1f - v;

		if (offset >= 0f){
			renderer.material.SetFloat("_Cutoff", offset); 
		}

		if (offset <= 0f) {
			GameController.Instance.nextWaveSpawnerActive = false;
			Destroy(gameObject);
		}



	}
}
