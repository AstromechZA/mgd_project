using UnityEngine;
using System.Collections;

public class DoNotDestroy : MonoBehaviour
{
		void Awake ()
		{
		if (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1) {
						Destroy (gameObject);
				} else {
						DontDestroyOnLoad (transform.gameObject);
				}
		}
}