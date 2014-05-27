using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("AchievementCollection")]
public class Achievements 
{	
	// Serialised variables
	#region Serialised Variables
	[XmlAttribute("towersBuilt")]
	public int towersBuilt;
	
	[XmlAttribute("enemiesKilled")]
	public int enemiesKilled;

	[XmlAttribute("timePlayedInMinutes")]
	public float timePlayedInMinutes;

	[XmlAttribute("totalAchieved")]
	public int totalAchieved;

	[XmlAttribute("currentRank")]
	public int currentRank;

	[XmlAttribute("moneyMade")]
	public float moneyMade;

	[XmlAttribute("gamesWon")]
	public int gamesWon;
	
	[XmlAttribute("perfectionist")]
	public int perfectionist;	
	
	[XmlAttribute("cleanSlate")]
	public int cleanSlate;

	[XmlArray("Achievements"),XmlArrayItem("Achievement")]
	public Achievement[] achievementsArray;
	#endregion

	// Setup achievement category image and skill level image
	public void Setup(){
		for (int i=0; i<achievementsArray.Length; i++) {
			achievementsArray[i].Setup(achievementsArray[i].image, achievementsArray[i].skill);
		}
	}

	// SAVE achievements
	public  void Save(string path)
	{
		var serializer = new XmlSerializer(typeof(Achievements));
		using(var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
		
	}

	// LOAD achievements
	public static Achievements Load(string path)
	{
		var serializer = new XmlSerializer(typeof(Achievements));
		using(var stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as Achievements;
		}
	}
}