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
	public GameObject towerTest;
	#endregion

	#region min/max X/Z
	private float _minScrollX = 0;
	private float _maxScrollX = 0;
	private float _minScrollZ = 0;
	private float _maxScrollZ = 0;

	public float getMinScrollX(){return _minScrollX;}
	public float getMaxScrollX(){return _maxScrollX;}
	public float getMinScrollZ(){return _minScrollZ;}
	public float getMaxScrollZ(){return _maxScrollZ;}
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

		this._minScrollX = -centerx - tileSize/2.0f;
		this._maxScrollX = centerx - tileSize/2.0f;
		this._minScrollZ = -centerz - tileSize/2.0f;
		this._maxScrollZ = centerz - tileSize/2.0f;

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
				
					if(Random.value > 0.7f && towerTest != null) {
						Instantiate(towerTest, new Vector3(x, 0.98f, z), Quaternion.identity);
					}
				
				}else if(c == '=') {
					Instantiate(floorGrate, new Vector3(x, grateYOffset, z), uprotation);
				}



				x += tileSize;
			}
			z += tileSize * zflip;
		}
	}



}
