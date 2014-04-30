using UnityEngine;
using System.Collections;

public class TowerBulk : MonoBehaviour {

	public Component twr;
	public int tilesWide = 32;
	public int tilesHigh = 18;

	// Use this for initialization
	void Start () {
		
		int maxx = tilesWide;
		int maxz = tilesHigh;
		
		float centerx = maxx / 2.0f;
		float centerz = maxz / 2.0f;

		for (int z=0;z<maxz;z+=2) {
			for (int x=0;x<maxx;x+=2) {
				Instantiate(twr, new Vector3(x-centerx+1f, 0, z-centerz+1f), Quaternion.identity);
			}
		}
	}

}
