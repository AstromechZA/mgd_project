using UnityEngine;
using System.Collections;

public class CameraScrollUpdator : MonoBehaviour {

	public float touch_move_scaling = 1.0f;
	public float keyboard_move_scaling = 2.0f;
	public GameObject mapController = null;

	private MapCreator _mapCreator;

	// this gets called first
	void Awake() {
	}

	// and then this
	void Start() {
		_mapCreator = mapController.GetComponent<MapCreator>();
	}

	// Update is called once per frame
	void Update () {
		Vector3 oldTranslate = transform.position * 1.0f;

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

		Vector3 newTranslate = transform.position;

		// check scroll ranges
		if(_mapCreator != null) {
			// fix scroll range
			if(newTranslate.x < _mapCreator.getMinScrollX() || 
			   newTranslate.x > _mapCreator.getMaxScrollX()) {
				newTranslate.x = oldTranslate.x;
				Debug.Log("scroll x out of range " + newTranslate.x + " " + _mapCreator.getMinScrollX() + "-" + _mapCreator.getMaxScrollX());
			}
			if(newTranslate.z < _mapCreator.getMinScrollZ() || 
			   newTranslate.z > _mapCreator.getMaxScrollZ()) {
				newTranslate.z = oldTranslate.z;
				Debug.Log("scroll z out of range " + newTranslate.z + " " + _mapCreator.getMinScrollZ() + "-" + _mapCreator.getMaxScrollZ());
			}
		}
		transform.position = newTranslate;
	}
}
