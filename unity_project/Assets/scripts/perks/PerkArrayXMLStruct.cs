using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("PerkCollection")]
public class PerkArrayXMLStruct
{
	[XmlArray("Perks"),XmlArrayItem("PerkXMLStruct")]
	public PerkXMLStruct[] perks;
}

