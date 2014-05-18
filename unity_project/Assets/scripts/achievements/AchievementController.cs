using UnityEngine;
using System.IO;

public  class AchievementController : MonoBehaviour {

	// Create Singleton
	private static AchievementController achievementController1;
	public static AchievementController achievementController
	{
		get{return achievementController1;}
	}
	
	public Achievements achievements;
	public Achievement[] achievementsArray;
	public float timePlayed = 0;
	public int towersBuilt = 0;
	public int enemiesDestroyed = 0;
	public int totalAchieved = 0;
	public int currentRank = 0;
	public float totalTimePlayed = 0;
	public AudioClip play_select;
	public AudioClip play_back;

	
	private string path;
	
	// Keep this gameobject active from scene to scene 
	void Awake(){
		if (achievementController1 == null)
		{
			achievementController1 = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (achievementController1 != this)
		{
			// Singleton already exists (destroy new one)
			Destroy(this);
		}
	}
	
	void Start(){
		path = Application.persistentDataPath + "/achievement.xml";
		
		// Move file to persistentDataPath if it does not already exist
		if (!File.Exists(path)) {
			TextAsset texture = Resources.Load ("Achievements/achievement") as TextAsset;
			System.IO.File.WriteAllBytes (path, texture.bytes);
		}
		
		// Load and Setup achievements
		loadAndSetupAchievements();
	}

	private void loadAndSetupAchievements(){
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
	
	public void resetAchievements(){
		// Copy original achievements file from resources to persistentDataPath
		TextAsset texture = Resources.Load ("Achievements/achievement") as TextAsset;
		System.IO.File.WriteAllBytes (path, texture.bytes);
		
		loadAndSetupAchievements();
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
