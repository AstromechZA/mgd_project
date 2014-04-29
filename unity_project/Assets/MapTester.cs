using UnityEngine;
using System.Collections;

public class MapTester : MonoBehaviour {

	public int tilesWide = 32;
	public int tilesHigh = 18;
	public int borderWidth = 10;

	public Component borderelement;

	void Start () {

		int maxx = tilesWide + borderWidth*2;

		int maxz = tilesHigh + borderWidth*2;

		
		float centerx = maxx / 2.0f;
		float centerz = maxz / 2.0f;


		for (int z=0;z<maxz;z++) {
			for (int x=0;x<maxx;x++) {
				if(
					x < borderWidth ||
					x >= (tilesWide + borderWidth) ||
					z < borderWidth ||
					z >= (tilesHigh + borderWidth)
				) {
					Instantiate(borderelement, new Vector3(x-centerx, 0.5f, z-centerz), Quaternion.identity);
				}
			}
		}

	}

	void Update () {
	
	}
}
