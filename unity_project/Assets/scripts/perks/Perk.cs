using System;
using UnityEngine;
public class Perk
{
	public string name;
	public string longDescription;
	public string type;
	public float value;
	public bool bought;
	public Perk[] prereqs;
	
	public Vector2 center;

	public Perk (
		string name, 
		string longDescription, 
		string type, 
		float value,
		Perk[] prereqs		
	) {
		this.bought = false;
		this.name = name;
		this.longDescription = longDescription;
		this.type = type;
		this.value = value;
		this.prereqs = prereqs;
	}
	
	public void Reset() {
		this.bought = false;	
	}
}

