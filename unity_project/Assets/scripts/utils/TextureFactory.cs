using System;
using UnityEngine;
public class TextureFactory
{
	public static Texture2D ColorTexture (Color c)
	{
		Texture2D t = new Texture2D(1,1);
		t.SetPixel(0, 0, c);
		t.Apply();
		return t;
	}

	public static Texture2D ColorTexture (int r, int g, int b)
	{
		return ColorTexture(new Color(r/255.0f, g/255.0f, b/255.0f));
	}
	
	public static Texture2D ColorTexture (float r, float g, float b)
	{
		return ColorTexture(new Color(r, g, b));
	}
}

