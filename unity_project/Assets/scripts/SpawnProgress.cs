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
	
	// use SetBar anywhere between 0 and 1
	public void setProgress( float v )
	{
		float offset = 0f;
		offset = Mathf.Clamp01( v/2f );
		if (offset <= 1f){
			renderer.material.mainTextureOffset = new Vector2(offset, 0);
		}

		if (offset >= 0.5f) {
			GameController.Instance.nextWaveSpawnerActive = false;
			Destroy(gameObject);
		}

	}
}
