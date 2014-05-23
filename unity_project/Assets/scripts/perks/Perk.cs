using System;
using UnityEngine;
public class Perk
{
	public string name;
	public string longDescription;
	public PerkType type;
	public float value;
	public bool bought;
	public Perk[] prereqs;
	
	public Vector2 center;

	public Perk (
		string name, 
		string longDescription, 
		PerkType type, 
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
	
	public bool CanBeBought() {
		if (bought) return false;
		if (prereqs.Length == 0) return true;
		foreach (Perk p in prereqs) {
			if (p.bought) return true;
		}
		return false;
	}
	
	public static PerkType StringToPerkType(string s) {
		return (PerkType) Enum.Parse(typeof(PerkType), s);
	}
	
	public enum PerkType {
		TWR_ALL_RANGE,
		TWR_GUN_RANGE,
		TWR_BEAM_RANGE,
		TWR_SONAR_RANGE,
		TWR_MISSILE_RANGE,
		
		TWR_ALL_DMG,
		TWR_GUN_DMG,
		TWR_BEAM_DMG,
		TWR_SONAR_DMG,
		TWR_MISSILE_DMG,
		
		TWR_ALL_SPEED,
		TWR_GUN_SPEED,
		TWR_BEAM_SPEED,
		TWR_SONAR_SPEED,
		TWR_MISSILE_SPEED,
		
		CREEP_ALL_REWARD
	}
}

