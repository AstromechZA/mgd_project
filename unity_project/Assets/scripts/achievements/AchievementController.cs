using UnityEngine;
using System.IO;

public  class AchievementController : Singleton<AchievementController> {
	
	public Achievements achievements;
	public Achievement[] achievementsArray;
	public float timePlayed = 0;
	public int towersBuilt = 0;
	public int enemiesDestroyed = 0;
	public int totalAchieved = 0;
	public int currentRank = 0;
	public float totalTimePlayed = 0;
	
	private string path;
	
	void Awake(){

		path = Application.persistentDataPath + "/achievement.xml";
		
		// Move file to persistentDataPath if it does not already exist
		if (!File.Exists(path)) {
			TextAsset texture = Resources.Load ("Achievements/achievement") as TextAsset;
			System.IO.File.WriteAllBytes (path, texture.bytes);
		}
		
		// Load and Setup achievements
		LoadAndSetupAchievements();
	}

	private void LoadAndSetupAchievements(){
		// Load and Setup achievements
		achievements = Achievements.Load(path);
		achievementsArray = achievements.achievementsArray;
		towersBuilt = achievements.towersBuilt;
		enemiesDestroyed = achievements.enemiesKilled;
		timePlayed = achievements.timePlayedInMinutes;
		totalTimePlayed = achievements.timePlayedInMinutes;
		totalAchieved = achievements.totalAchieved;
		currentRank = achievements.currentRank;
		achievements.Setup();
	}
	
	public void ResetAchievements(){
		// Copy original achievements file from resources to persistentDataPath
		TextAsset texture = Resources.Load ("Achievements/achievement") as TextAsset;
		System.IO.File.WriteAllBytes (path, texture.bytes);
		
		LoadAndSetupAchievements();
	}
	
	void OnApplicationQuit() {
		
		// Save the completed achievements and player stats
		achievements.towersBuilt = towersBuilt;
		achievements.enemiesKilled = enemiesDestroyed; 
		achievements.timePlayedInMinutes = totalTimePlayed;
		achievements.totalAchieved = totalAchieved;
		achievements.currentRank = currentRank;
		
		achievements.Save(path);
	}

	
}
