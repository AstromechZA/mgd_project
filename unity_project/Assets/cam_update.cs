using UnityEngine;
using System.Collections;

public class cam_update : MonoBehaviour {

	public float touch_move_scaling = 1.0f;
	public float keyboard_move_scaling = 2.0f;
	
	// Update is called once per frame
	void Update () {
	#if UNITY_ANDROID
		if(Input.touchCount > 0)
		{
			Touch touch = Input.touches[0];
			if (touch.phase == TouchPhase.Moved)
			{
				transform.Translate(touch.deltaPosition * Time.deltaTime * -1.0f * touch_move_scaling);
			}
		}
	#else
		if(Input.GetKey(KeyCode.LeftArrow)) transform.Translate(Vector3.left * Time.deltaTime * keyboard_move_scaling);
		if(Input.GetKey(KeyCode.RightArrow)) transform.Translate(Vector3.right * Time.deltaTime * keyboard_move_scaling);
		if(Input.GetKey(KeyCode.UpArrow)) transform.Translate(Vector3.up * Time.deltaTime * keyboard_move_scaling);
		if(Input.GetKey(KeyCode.DownArrow)) transform.Translate(Vector3.down * Time.deltaTime * keyboard_move_scaling);
	#endif

	}
}
