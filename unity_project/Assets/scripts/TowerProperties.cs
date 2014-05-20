using UnityEngine;
using System.Collections;

public class TowerProperties : MonoBehaviour {
	public int cost = 1;
	public float fireRate = 1f;
	public float range = 40f;

	public float moveLeewayTime = 0.0f;
	public bool moveLeeway = false;

	public AudioClip build_sound;
	public AudioClip build_error;
	public Material placement_lines; 
	public Material placement_lines_red; 
	public GameObject placementVisualiser;

	void Update(){
		if (moveLeeway) {
			moveLeewayTime -= Time.deltaTime;

			if (moveLeewayTime <= 0){
				moveLeeway = false;
			}
		}
	}
}
