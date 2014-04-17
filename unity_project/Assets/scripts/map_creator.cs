using UnityEngine;
using System.Collections;
using System.Linq;

public class map_creator : MonoBehaviour {

	// Use this for initialization
	public GameObject floorTile;
	public GameObject floorGrate;
	public TextAsset mapFile;
	public float tileSize = 1.0f;
	public float tileYOffset = -0.5f;
	public float grateYOffset = -0.7f;
	public bool zFlip = true;

	void Start () {

		var lines = mapFile.text.Split('\n').Select(l => l.Trim());

		// calculate max line length
		int longestline = lines.Select(l => l.Length).Max();
		int numlines = lines.Count();

		// calculate center
		float centerx = longestline / 2.0f;
		float centerz = numlines / 2.0f;

		// tile orientation
		Quaternion uprotation = Quaternion.LookRotation(Vector3.up);

		// remember positive Z is in the ^ direction.
		//      Z ^
		//        |
		//        +---> X

		int zflip = ((zFlip)?-1:1);

		float z = -centerz * tileSize * zflip;
		foreach(string line in lines) {
			float x = -centerx * tileSize;
			foreach(char c in line) {
				if(c == '#') {
					Instantiate(floorTile, new Vector3(x, tileYOffset, z), uprotation);
				}else if(c == '=') {
					Instantiate(floorGrate, new Vector3(x, grateYOffset, z), uprotation);
				}
				x += tileSize;
			}
			z += tileSize * zflip;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
