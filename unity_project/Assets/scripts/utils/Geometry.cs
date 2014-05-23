using System;
using UnityEngine;
public class Geometry
{
	public static Rect CenterRectOnPoint(float width, float height, float x, float y) {
		float hw = width/2;
		float hh = height/2;
		return new Rect(x-hw, y-hh, width, height);
	}
	
	public static Rect CenterRectOnPoint(Rect r, Vector2 p) {
		return CenterRectOnPoint(r.width, r.height, p.x, p.y);
	}
	
	public static Rect CenterRectOnPoint(float width, float height, Vector2 p) {
		return CenterRectOnPoint(width, height, p.x, p.y);
	}
	
	public static Rect CenterRectOnPoint(Rect r, float x, float y) {
		return CenterRectOnPoint(r.width, r.height, x, y);
	}
	
	public static Vector3 Vector2To3(Vector2 input) {
		return new Vector3(input.x, input.y, 0);
	}
	
	public static Vector2 FlipInScreenVertical(Vector2 input) {
		return new Vector2(input.x, Screen.height - input.y);
	}
}

