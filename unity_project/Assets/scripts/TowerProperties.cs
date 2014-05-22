using UnityEngine;
using System.Collections;

public class TowerProperties : MonoBehaviour {
	public int cost = 1;
	public float fireRate = 1f;
	public float range = 40f;
	public float damage = 1f;

	public float moveLeewayTime = 3.0f;
	private bool moveLeeway = true;
	public float placementTime;

	public AudioClip build_sound;
	public AudioClip build_error;
	public Material placement_lines; 
	public Material placement_lines_red; 
	public GameObject placementVisualiser;

	public bool HasMoveLeeway{
		get{
			// No point recalculating if it's already expired.
			if (moveLeeway) {
				if (Time.time - placementTime > moveLeewayTime) {
					moveLeeway = false;
				}
			}

			return moveLeeway;
		}
	}
}
