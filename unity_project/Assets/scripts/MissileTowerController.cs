using UnityEngine;
using System.Collections;

public class MissileTowerController : MonoBehaviour {
	#region PUBLICVARS ====================================================================== //
	
	public float fireRate = 5.0f;
	
	public float range = 8;
	
	#endregion
	#region PRIVATEVARS ===================================================================== //

	private const float DOOROPENAMOUNT = 0.3f;

	// bones
	private Transform mainBone;
	private Transform northBone;
	private Transform southBone;
	private Transform eastBone;
	private Transform westBone;
	
	#endregion
	#region STANDARD ======================================================================== //

	void Start () {
		mapBones();

		setDoorProgress(1.0f);

	}

	void Update () {
	
	}

	#endregion
	#region MISC ============================================================================ //

	private void mapBones () {
		Transform a = transform.Find ("Armature");
		mainBone = a.Find("main");
		northBone = mainBone.Find("north");
		southBone = mainBone.Find("south");
		eastBone = mainBone.Find("east");
		westBone = mainBone.Find("west");
	}
	
	#endregion
	#region DOORS ============================================================================ //

	private void setDoorProgress(float percent) {
		float d = percent * DOOROPENAMOUNT;
		northBone.transform.localPosition = new Vector3(-1, 0, -d);
		southBone.transform.localPosition = new Vector3(-1, 0, d);
		eastBone.transform.localPosition = new Vector3(-1, -d, 0);
		westBone.transform.localPosition = new Vector3(-1, d, 0);
	}

	#endregion

}
