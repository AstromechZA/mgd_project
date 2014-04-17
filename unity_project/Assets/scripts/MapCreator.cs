using UnityEngine;
using System.Collections;
using System.Linq;

public class MapCreator : MonoBehaviour {

	#region publics
	public GameObject floorTile;
	public GameObject floorGrate;
	public TextAsset mapFile;
	public float tileSize = 1.0f;
	public float tileYOffset = -0.5f;
	public float grateYOffset = -0.7f;
	public bool zFlip = true;
	#endregion

	#region min/max X/Z
	private float minx = 0;
	private float maxx = 0;
	private float minz = 0;
	private float maxz = 0;

	public float getMinX(){return minx;}
	public float getMaxX(){return maxx;}
	public float getMinZ(){return minz;}
	public float getMaxZ(){return maxz;}
	#endregion

	void Start () {
		this.LoadMap();

	}

	void LoadMap () {
		var lines = mapFile.text.Split('\n').Select(l => l.Trim());
		
		// calculate max line length
		int longestline = lines.Select(l => l.Length).Max();
		int numlines = lines.Count();
		
		// calculate center
		float centerx = longestline / 2.0f;
		float centerz = numlines / 2.0f;

		this.minx = -centerx - tileSize/2.0f;
		this.maxx = centerx + tileSize/2.0f;
		this.minz = -centerz - tileSize/2.0f;
		this.maxz = centerz + tileSize/2.0f;

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



}
