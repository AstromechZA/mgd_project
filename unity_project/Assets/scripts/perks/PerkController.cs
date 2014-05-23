using System;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
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
		var serializer = new XmlSerializer(typeof(PerkArrayXMLStruct));
		TextAsset perktext = Resources.Load ("Perks/perks") as TextAsset;
		
		using(var sr = new StringReader(perktext.text))
		{
			PerkArrayXMLStruct perks = serializer.Deserialize(sr) as PerkArrayXMLStruct;
			
			this.perks = new Perk[perks.perks.Length];
			
			Hashtable perkByID = new Hashtable();
			
			// assemble perk objects
			foreach (PerkXMLStruct pxml in perks.perks) {
				Perk.PerkType pt = Perk.StringToPerkType(pxml.type);
				perkByID[pxml.id] = new Perk(pxml.name, pxml.description, pt, pxml.value, new Perk[]{});
			}
			// assemble linked perks
			for(int j=0;j<perks.perks.Length;j++) {
				PerkXMLStruct pxml = perks.perks[j];
				
				Perk p = perkByID[pxml.id] as Perk;
				
				Perk[] pr = new Perk[pxml.prereqs.Length];
				for(int i=0;i<pxml.prereqs.Length;i++) {
					pr[i] = perkByID[pxml.prereqs[i]] as Perk;
				}	
				// bind
				p.prereqs = pr;		
				
				this.perks[j] = p;
			}
		}
	}
	
	// reset all the perks
	private void ResetAll() {
		foreach (Perk p in perks) {
			p.Reset();		
		}	
	}
	
	#region --- PERK POINTS AND EXPERIENCE ---
	public void AddExperience(float val) {
		currentExperience += val;
		while (currentExperience > maxExperience) {
			currentExperience -= maxExperience;
			perkPointsAvailable += 1;
			maxExperience *= perPointExpMultiplier;
		}
	}
	
	// return the current progress to next point
	public float GetExperienceProgress() {
		return currentExperience / maxExperience;
	}
	
	public int GetPoints() {
		return perkPointsAvailable;
	}
	
	public void SpendPoint() {
		if (perkPointsAvailable <= 0) throw new Exception("No more perk points!");
		perkPointsAvailable--;
	}
	
	public void AddPoint() {
		perkPointsAvailable++;
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

