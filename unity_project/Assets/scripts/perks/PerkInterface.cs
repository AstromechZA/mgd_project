using UnityEngine;
using System.Collections;

public class PerkInterface : MonoBehaviour {
	
	public Texture2D sidebarTexture;
	public Texture2D titleTexture;
	
	private int sidebarWidth = 300;
	
	#region GUISTYLES
		private Texture2D backgroundT;

	#endregion

	#region REGIONS
		private Rect fullScreen;
		private Rect sideBar;
		private Rect titleBox;
		private Rect backBtn;
	#endregion

	void Awake () {
		fullScreen = new Rect(0, 0, Screen.width, Screen.height);
		backgroundT = TextureFactory.ColorTexture(51, 51, 51);
		
		sideBar = new Rect(Screen.width - sidebarWidth, 0, sidebarWidth, Screen.height);
		
		titleBox = new Rect((Screen.width - sidebarWidth - titleTexture.width)/2, 0, titleTexture.width, titleTexture.height);
		
		backBtn = new Rect(0, 0, 65, 35);
	}

	void Start () {
	
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) { 
			Application.LoadLevel ("gridtest");
		}
	}

	void OnGUI () {
		GUI.DrawTexture(fullScreen, backgroundT);
		GUI.DrawTexture(sideBar, sidebarTexture);
		GUI.DrawTexture(titleBox, titleTexture);
		
		if (GUI.Button (backBtn, "Back")) {
			Application.LoadLevel ("gridtest");
		}
	}
}
