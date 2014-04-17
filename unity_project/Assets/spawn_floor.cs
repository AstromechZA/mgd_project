using UnityEngine;
using System.Collections;
using System.Linq;

public class spawn_floor : MonoBehaviour {

	// Use this for initialization
	public GameObject ft;
	public TextAsset ta;
	public float tilesize = 1.0f;
	public float tileyoffset = -0.5f;

	void Start () {

		var lines = ta.text.Split('\n').Select(l => l.Trim());

		// calculate max line length
		int longestline = lines.Select(l => l.Length).Max();
		int numlines = lines.Count();

		// calculate center
		float centerx = longestline / 2.0f;
		float centerz = numlines / 2.0f;

		// tile orientation
		Quaternion uprotation = Quaternion.LookRotation(Vector3.up);

		float z = -centerz * tilesize;
		foreach(string line in lines) {
			float x = -centerx * tilesize;
			foreach(char c in line) {
				if(c == '#') {
					Instantiate(ft, new Vector3(x, tileyoffset, z), uprotation);
				}
				x += tilesize;
			}
			z += tilesize;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
