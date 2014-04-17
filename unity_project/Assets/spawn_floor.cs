using UnityEngine;
using System.Collections;

public class spawn_floor : MonoBehaviour {

	// Use this for initialization
	public GameObject ft;

	void Start () {
			
		Quaternion q = Quaternion.LookRotation(new Vector3(0.0f,1.0f,0.0f));

		for(int i=0;i<16;i++) {
			for (int j = 0;j<16;j++) {
				bool b = Random.value > 0.3f;
				if(b)Instantiate(ft, new Vector3(i-8,-0.5f,j-8), q);
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
