using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class PerkXMLStruct
{
	
	public int id;
	
	public int column;
	
	public string name;
	
	public string description;
	
	public string type;
	
	public float value;
	
	[XmlArray("Prereqs"),XmlArrayItem("pr")]
	public int[] prereqs;
	
}

