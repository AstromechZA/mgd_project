using UnityEngine;
using System.Collections;

public class FPSOverlay : MonoBehaviour
{
	float deltaTime = 0.0f;
	int screenWidth, screenHeight;
	GUIStyle style;
	Rect rect;

	void Start(){
		style = new GUIStyle ();
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = screenHeight * 3 / 100;
		style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		rect = new Rect(0, 0, screenWidth, screenHeight * 2 / 100);
	}
	
	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}
	
	void OnGUI()
	{
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		
		GUI.Label(rect, text, style);
	}
}