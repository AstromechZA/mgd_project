using System;
using UnityEngine;
using System.Collections;

public class PerkController : Singleton<PerkController>
{	
	private Perk[] perks;
	public Perk[] Perks { get {return perks;}}
	
	private float currentExperience = 0.0f;
	private float maxExperience = 10;
	public float perPointExpMultiplier = 2;
	private int perkPointsAvailable = 0;

	public PerkController ()
	{
		CreatePerks();
	}
	
	private void CreatePerks() {
		Perk p1 = new Perk("Lead Streamlining", "All of your Gun Towers gain +20 range.", "twr-gun-range", 50, new Perk[]{});
		Perk p2 = new Perk("Heated Shot", "All of your Gun Towers gain +1 damage", "twr-gun-damage", 1, new Perk[]{p1});
		Perk p3 = new Perk("Alien Bankers", "Gain +1 credits per kill.", "creep-all-reward", 1, new Perk[]{p1});
		Perk p4 = new Perk("High Frequency Occilators", "Increase Beam Tower burst length by 1 seconds", "twr-beam-time", 1, new Perk[]{p2, p3});
		
		this.perks = new Perk[]{p1, p2, p3, p4};
	}
	
	// reset all the perks
	private void ResetAll() {
		foreach (Perk p in perks) {
			p.Reset();		
		}	
	}
	
	#region --- PERK POINTS AND EXPERIENCE ---
	private void AddExperience(float val) {
		currentExperience += val;
		while (currentExperience > maxExperience) {
			currentExperience -= maxExperience;
			perkPointsAvailable += 1;
			maxExperience *= perPointExpMultiplier;
		}
	}
	
	// return the current progress to next point
	private void GetExperienceProgress() {
		return currentExperience / maxExperience;
	}
	
	#endregion
	
	#region --- TREE STRUCTURE SPACING AND GENERATION ---
	// calculate a tree structure and spacing
	// this sets the 'center' property of each perk
	public Vector2 Space(int colSpace, int rowSpace) {

		ArrayList columns = FindColumns();
	
		int x = 0;
		foreach (ArrayList currentColumn in columns) {
			// number of perks in this column
			int height = currentColumn.Count * rowSpace - rowSpace;
			float hheight = height/2.0f;
			float y = -hheight;
			
			foreach (Perk p in currentColumn) {
				p.center = new Vector2(x, y);
				y += rowSpace;
			}
			
			x += colSpace;
		}
		
		// return root anchor point
		return new Vector2(colSpace, Screen.height/2);
	}
	
	private ArrayList FindColumns() {
		ArrayList columns = new ArrayList();
		
		// first identify last column
		ArrayList lastColumn = FindLastColumn();
		
		// while there were perks in the last column
		while (lastColumn.Count > 0) {
			
			// add it to the left of the columns list
			columns.Insert(0, lastColumn);
			
			// calculate the previous column
			BasicHashSet prevColumn = new BasicHashSet();
			foreach (Perk p in lastColumn) {
				foreach (Perk r in p.prereqs) {
					prevColumn.Add(r);
				}
			}
			
			// set as last column
			lastColumn = prevColumn.ToArrayList();			
		}
		
		return columns;
	}
	
	private ArrayList FindLastColumn() {
		BasicHashSet seen = new BasicHashSet();
		// add all to set
		foreach (Perk p in perks) {
			seen.Add(p);
		}
		// remove everything that is a prerequisite
		foreach (Perk p in perks) {
			foreach (Perk r in p.prereqs) {
				seen.Remove(r);
			}
		}
		// return
		return seen.ToArrayList();		
	}
	#endregion
}

