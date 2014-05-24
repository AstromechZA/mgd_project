using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public float speed = 2;
	
	void Update ()
	{
		transform.Rotate(Vector3.up * (speed * Time.deltaTime));
	}
}
