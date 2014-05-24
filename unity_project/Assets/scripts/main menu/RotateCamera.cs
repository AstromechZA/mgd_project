using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public float speed = 2;
	
	//Calculating our own delta time (Since we use Time.timeScale and set it to 0 as a solution for pausing the game)
	// Note: Setting Time.timeScale effects Time.deltaTime	
	float timeAtLastFrame = 0F;
	float timeAtCurrentFrame = 0F;
	float deltaTime = 0F;

	void Update ()
	{
		//Create delta time using realtimeSinceStartup
		timeAtCurrentFrame = Time.realtimeSinceStartup;
		deltaTime = timeAtCurrentFrame - timeAtLastFrame;
		timeAtLastFrame = timeAtCurrentFrame; 

		transform.Rotate(Vector3.up * (speed * deltaTime));
	}
}
