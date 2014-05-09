using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	private static MapManager _instance;
	public static MapManager Instance { get { return _instance; } }

	// Map dimensions in terms of nav mesh points.
	public int mapWidth = 44;
	public int mapHeight = 21;
	public int mapCellSize = 10;

	// Offsets of the map grid from the center of the camera due to the GUI.
	public float mapXGridOffset = 0f;
	public float mapZGridOffset = 5f;

	public GameObject citadel;

	private float xShift;
	private float zShift;
	private float halfCellSize;

	private bool[,] occupancyGrid;

	void Awake(){
		_instance = this;
		
		xShift = (float)mapWidth / 2 * mapCellSize - (mapWidth % 2 == 0 ? 0 : mapCellSize / 2);
		zShift = (float)mapHeight / 2 * mapCellSize - (mapHeight % 2 == 0 ? 0 : mapCellSize / 2);
		halfCellSize = mapCellSize / 2;

		// Instantiate occupancy grid to empty.
		occupancyGrid = new bool[mapWidth, mapHeight];

		for (int x=0; x < mapWidth; x++) {
			for(int z=0; z < mapHeight; z++){
				occupancyGrid[x,z] = false;
			}
		}
	}

	public bool IsCellOccupied(int x, int z){
		return occupancyGrid[x,z];
	}

	/**
	 * Converts world position into snapped grid coordinates.
	 */
	public Vector3 SnapToGrid(Vector3 coordinates){
		float x = coordinates.x;
		float z = coordinates.z;

		// Adjust coordinates to compensate for offset grid and -ve coordinates.
		x += xShift;
		z += zShift;

		int gridX = Mathf.RoundToInt (x / mapCellSize);
		int gridZ = Mathf.RoundToInt (z / mapCellSize);

		gridX = Mathf.Clamp (gridX, 1, mapWidth - 1);
		gridZ = Mathf.Clamp (gridZ, 1, mapHeight - 1);
	
		return new Vector3(gridX * mapCellSize - xShift, 0f, gridZ * mapCellSize - zShift);
	}

	/**
	 * Determines whether a snapped position is valid for tower placement
	 * by checking the 4 boxes a tower overlaps.
	 * 
	 * Returns a Vector4 with elements set to 1 if it's occupied and 0 if not.
	 * i.e. Vector4.zero is a valid placement spot (when all 4 are unoccupied).
	 */
	public Vector4 PlacementQuery(Vector3 coordinates){
		float x = coordinates.x;
		float z = coordinates.z;

		x += xShift;
		z += zShift;

		int gridX1 = Mathf.FloorToInt ((x - halfCellSize) / mapCellSize);
		int gridZ1 = Mathf.FloorToInt ((z + halfCellSize) / mapCellSize);

		int gridX2 = Mathf.FloorToInt ((x + halfCellSize) / mapCellSize);
		int gridZ2 = Mathf.FloorToInt ((z + halfCellSize) / mapCellSize);

		int gridX3 = Mathf.FloorToInt ((x - halfCellSize) / mapCellSize);
		int gridZ3 = Mathf.FloorToInt ((z - halfCellSize) / mapCellSize);

		int gridX4 = Mathf.FloorToInt ((x + halfCellSize) / mapCellSize);
		int gridZ4 = Mathf.FloorToInt ((z - halfCellSize) / mapCellSize);

		Vector4 queryResponse = new Vector4 (
			(IsCellOccupied (gridX1, gridZ1) ? 1 : 0), 
			(IsCellOccupied (gridX2, gridZ2) ? 1 : 0), 
			(IsCellOccupied (gridX3, gridZ3) ? 1 : 0), 
			(IsCellOccupied (gridX4, gridZ4) ? 1 : 0));

		return queryResponse;
	}

	public void SetOccupancyForPosition(Vector3 coordinates, bool occupied) {
		float x = coordinates.x;
		float z = coordinates.z;
		
		x += xShift;
		z += zShift;
		
		int gridX1 = Mathf.FloorToInt ((x - halfCellSize) / mapCellSize);
		int gridZ1 = Mathf.FloorToInt ((z + halfCellSize) / mapCellSize);
		
		int gridX2 = Mathf.FloorToInt ((x + halfCellSize) / mapCellSize);
		int gridZ2 = Mathf.FloorToInt ((z + halfCellSize) / mapCellSize);
		
		int gridX3 = Mathf.FloorToInt ((x - halfCellSize) / mapCellSize);
		int gridZ3 = Mathf.FloorToInt ((z - halfCellSize) / mapCellSize);
		
		int gridX4 = Mathf.FloorToInt ((x + halfCellSize) / mapCellSize);
		int gridZ4 = Mathf.FloorToInt ((z - halfCellSize) / mapCellSize);

		SetCellOccupancy (gridX1, gridZ1, occupied);
		SetCellOccupancy (gridX2, gridZ2, occupied);
		SetCellOccupancy (gridX3, gridZ3, occupied);
		SetCellOccupancy (gridX4, gridZ4, occupied);
	}

	public void SetCellOccupancy(int x, int z, bool occupied){
		occupancyGrid [x, z] = occupied;
	}
}
