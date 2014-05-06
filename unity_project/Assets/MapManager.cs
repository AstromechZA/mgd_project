using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	private static MapManager _instance;
	public MapManager Instance { get { return _instance; } }

	public int mapWidth = 44;
	public int mapHeight = 21;

	void Awake(){
		_instance = this;
	}
}
