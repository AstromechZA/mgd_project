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

		if(_mapCreator != null) {
			if(newTranslate.x < _mapCreator.getMinX() || 
			   newTranslate.x > _mapCreator.getMaxX()) {
				newTranslate.x = oldTranslate.x;
				Debug.Log("scroll x out of range " + newTranslate.x + " " + _mapCreator.getMinX() + "-" + _mapCreator.getMaxX());
			}
			if(newTranslate.z < _mapCreator.getMinZ() || 
			   newTranslate.z > _mapCreator.getMaxZ()) {
				newTranslate.z = oldTranslate.z;
				Debug.Log("scroll z out of range " + newTranslate.z + " " + _mapCreator.getMinZ() + "-" + _mapCreator.getMaxZ());
			}
		}
		transform.position = newTranslate;
	}
}
