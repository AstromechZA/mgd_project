using System;
using UnityEngine;
public class TextureFactory
{
	public static Texture2D RGBTexture (Color c)
	{
		Texture2D t = new Texture2D(1,1);
		t.SetPixel(0, 0, c);
		t.Apply();
		return t;
	}

	public static Texture2D RGBTexture (int r, int g, int b)
	{
		return RGBTexture(r/255.0f, g/255.0f, b/255.0f);
	}
	
	public static Texture2D RGBTexture (float r, float g, float b)
	{
		return RGBTexture(new Color(r, g, b));
	}
	
	
	public static Texture2D RGBATexture (Color c)
	{
		Texture2D t = new Texture2D(1,1);
		t.SetPixel(0, 0, c);
		t.Apply();
		return t;
	}
	
	public static Texture2D RGBATexture (int r, int g, int b, int a)
	{
		return RGBATexture(r/255.0f, g/255.0f, b/255.0f, a/255.0f);
	}
	
	public static Texture2D RGBATexture (float r, float g, float b, float a)
	{
		return RGBATexture(new Color(r, g, b, a));
	}
	
}

