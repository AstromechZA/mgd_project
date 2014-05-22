using UnityEngine;

public class InputFactory
{
	public static bool MouseDownInRect(int button, Rect r) 
	{
		if (Input.GetMouseButtonDown(button)) {
			Vector3 v = Input.mousePosition;
			return r.Contains(new Vector2(v.x, v.z));
		}
		return false;
	}
	
	public static bool MouseUpInRect(int button, Rect r) 
	{
		if (Input.GetMouseButtonUp(button)) {
			Vector3 v = Input.mousePosition;
			return r.Contains(new Vector2(v.x, Screen.height-v.y));
		}
		return false;
	}
}

