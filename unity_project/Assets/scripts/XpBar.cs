using UnityEngine;
using System.Collections;

public class XpBar: ScriptableObject{
	
	float endPos_x = -34.85545F;
	float startPos_x = -57.6198F;

	void Start () {
				
		}

	public void addXp (float xp){
		GameObject temp  = GameObject.Find("xpBar");
		Vector3 newPos = new Vector3(xp, 0.0F, 0.0F);
		temp.transform.position += newPos;
		Player.experience += (int)xp;
	
		// level up
		if (temp.transform.position.x >= endPos_x) 
			{
			Player.perk_points++;
			Vector3 movePos = new Vector3(startPos_x - endPos_x, 0.0F, 0.0F);
			temp.transform.position += movePos;
			}
		}
}