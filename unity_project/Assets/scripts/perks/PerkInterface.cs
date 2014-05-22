using UnityEngine;
using System.Collections;

public class PerkInterface : MonoBehaviour {
	
	public Texture2D sidebarTexture;
	public Texture2D titleTexture;
	public Texture2D perkCircleTexture;
	
	private int sidebarWidth = 300;
	private Texture2D backgroundT;
	
	#region GUISTYLES
		public GUIStyle sidebarNameStyle;
		public GUIStyle sidebarDescriptionStyle;
		public GUIStyle availablePointsBoxStyle;
	#endregion

	#region REGIONS
		private Rect fullScreen;
		private Rect sideBar;
		private Rect titleBox;
		private Rect backBtn;
		private Rect perkCircleR;
		private Vector2 treeRoot;
		private Rect availablePointsBox;
	#endregion
	
	#region SELECTED
		private bool selected = false;
		private Perk selectedPerk = null;
		private Rect selectedNameRect;
		private Rect selectedDescriptionRect;
		private Rect selectedBuyButton;
		private bool selectedCanBeBought = false;
	#endregion
	
	void Awake () {
		fullScreen = new Rect(0, 0, Screen.width, Screen.height);
		backgroundT = TextureFactory.ColorTexture(51, 51, 51);
		
		sideBar = new Rect(Screen.width - sidebarWidth, 0, sidebarWidth, Screen.height);
		
		titleBox = new Rect((Screen.width - sidebarWidth - titleTexture.width)/2, 0, titleTexture.width, titleTexture.height);
				
		backBtn = new Rect(0, 0, 65, 35);
		
		treeRoot = PerkController.Instance.Space(100, 150);
		
		perkCircleR = new Rect(0, 0, perkCircleTexture.width, perkCircleTexture.height);
		
		selectedDescriptionRect = new Rect(
			Screen.width - sidebarWidth + 20,
			70,
			sideBar.width - 40,
			500
		);
		
		selectedBuyButton = new Rect(
			Screen.width - sidebarWidth + 20,
			sideBar.height - 120,
			sideBar.width - 40,
			100
		);
		
		availablePointsBox = new Rect(
			(Screen.width - sidebarWidth - titleTexture.width)/2, 
			titleTexture.height + 10, 
			titleTexture.width, 
			titleTexture.height 
		);	
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
			
			Vector2 clickpos = Geometry.FlipInScreenVertical(Geometry.Vector2To3(Input.mousePosition));
			
			Perk p = SelectPerkAtPosition(clickpos);
			if (p != null) {
				Select (p);
			} 
		}
	}

	private Perk SelectPerkAtPosition(Vector2 v) {
		// check which perk if any is selected
		foreach (Perk p in PerkController.Instance.Perks) {
			Vector2 pos = treeRoot + p.center;
			// is it on top of a perk block
			if (Geometry.CenterRectOnPoint(perkCircleR, pos).Contains(v)) {
				return p;
			}
		}
		return null;
	}
	
	void Select(Perk p) {
		selected = true;
		selectedPerk = p;
		
		Vector2 v = sidebarNameStyle.CalcSize(new GUIContent(selectedPerk.name));		
		selectedNameRect = new Rect(
			Screen.width - sidebarWidth/2 - v.x/2,
			20,
			v.x,
			v.y
		);
		
		selectedCanBeBought = p.CanBeBought() && (PerkController.Instance.GetPoints() > 0);
	}
	
	
	void Deselect () {
		selected = false;
	}

	void OnGUI () {
		GUI.DrawTexture(fullScreen, backgroundT);
		GUI.DrawTexture(titleBox, titleTexture);
		
		string av = "" + PerkController.Instance.GetPoints() + " points available";
		GUI.Box(availablePointsBox, av, availablePointsBoxStyle);
		
		if (GUI.Button (backBtn, "Back")) {
			Application.LoadLevel ("gridtest");
		}
		
		foreach (Perk p in PerkController.Instance.Perks) {
			Vector2 pos = treeRoot + p.center;
			foreach (Perk r in p.prereqs) {
				if (p.bought && r.bought) {
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
			// background
			GUI.DrawTexture(sideBar, sidebarTexture);
			
			// title text
			GUI.Box(selectedNameRect, selectedPerk.name, sidebarNameStyle);
			
			// description text
			GUI.Box(selectedDescriptionRect, selectedPerk.longDescription, sidebarDescriptionStyle);
			
			GUI.enabled = selectedCanBeBought;
			if (GUI.Button(selectedBuyButton, "Buy")) {
				// by the perk
				selectedPerk.bought = true;
				// rerender
				Select (selectedPerk);
			}
			GUI.enabled = true;
		}
	
		
	}
}
