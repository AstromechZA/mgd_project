using UnityEngine;
using System.Collections;

public class PerkInterface : MonoBehaviour {
	
	public Texture2D sidebarTexture;
	public Texture2D titleTexture;
	public Texture2D perkCircleTexture;
	
	private int sidebarWidth = 300;
	
	#region GUISTYLES
		private Texture2D backgroundT;
	#endregion

	#region REGIONS
		private Rect fullScreen;
		private Rect sideBar;
		private Rect titleBox;
		private Rect backBtn;
		private Vector2 treeRoot;
	#endregion
	
	#region SELECTED
		private bool selected = false;
	#endregion
	
	void Awake () {
		fullScreen = new Rect(0, 0, Screen.width, Screen.height);
		backgroundT = TextureFactory.ColorTexture(51, 51, 51);
		
		sideBar = new Rect(Screen.width - sidebarWidth, 0, sidebarWidth, Screen.height);
		
		titleBox = new Rect((Screen.width - sidebarWidth - titleTexture.width)/2, 0, titleTexture.width, titleTexture.height);
		
		backBtn = new Rect(0, 0, 65, 35);
		
		treeRoot = PerkController.Instance.Space(100, 150);
	}

	void Start () {
	
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) { 
			Application.LoadLevel ("gridtest");
		}
		
		// is mouse down?
		if (Input.GetMouseButtonDown(0)) {
			bool actioned = false;
			
			// check which perk if any is selected
			foreach (Perk p in PerkController.Instance.Perks) {
				Vector2 pos = treeRoot + p.center;
				if (Geometry.CenterRectOnPoint(50, 50, pos).Contains(Geometry.Vector2To3(Input.mousePosition))) {
					Select (p);
					actioned = true;
				}
			}
			if (!actioned) Deselect();
		}
	}

	void Select(Perk p) {
		selected = true;
	}
	
	void Deselect () {
		selected = false;
	}

	void OnGUI () {
		GUI.DrawTexture(fullScreen, backgroundT);
		GUI.DrawTexture(titleBox, titleTexture);
		
		if (GUI.Button (backBtn, "Back")) {
			Application.LoadLevel ("gridtest");
		}
		
		foreach (Perk p in PerkController.Instance.Perks) {
			Vector2 pos = treeRoot + p.center;
			foreach (Perk r in p.prereqs) {
				if (p.bought) {
					LineDrawer.DrawLine(pos, treeRoot + r.center, Color.blue, 10, true);
				} else if (r.bought) {
					LineDrawer.DrawLine(pos, treeRoot + r.center, Color.gray, 8, true);
				} else {
					LineDrawer.DrawLine(pos, treeRoot + r.center, Color.black, 7, true);
				}
			}
		}
		
		foreach (Perk p in PerkController.Instance.Perks) {
			Vector2 pos = treeRoot + p.center;
			GUI.DrawTexture(Geometry.CenterRectOnPoint(perkCircleTexture.width, perkCircleTexture.height, pos), perkCircleTexture);
		}
		
		if (selected) {
			GUI.DrawTexture(sideBar, sidebarTexture);
		}
	
		
	}
}
