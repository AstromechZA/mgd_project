using UnityEngine;
using System.IO;

public class AchievementNotifier : MonoBehaviour
{
	public AudioClip achievementEarnedSound;

	// GUI styles
	public GUIStyle GUIStyleAchievement;
	public GUIStyle GUIStyleAmazing;
	public GUIStyle GUIStyleGeneral;
	public GUIStyle GUINotifierStyle;

	private Achievement achievmentToDisplay;

	// Position of Achievement scroll
	private Vector2 scrollPosition;
	private Touch touch;

	// Notification timer
	private float timer = 0.0f;
	private bool timerActive = false;
	
	void Start()
	{
		// Set fonts up
		GUIStyleAchievement.normal.textColor = Color.white;
		GUIStyleAmazing.normal.textColor = Color.white;
		Color c1 = new Color(0.52f, 0.52f, 0.52f, 1.0f);
		GUIStyleGeneral.normal.textColor = c1;
		GUIStyleAchievement.fontSize = 16;
		GUIStyleAmazing.fontSize = 20;

		GUINotifierStyle.normal.background = (Texture2D)Resources.Load ("Achievements/bases/notifier_background");
	}

	#region Achievement Earned
	private void achievementEarned(Achievement a)
	{
		AudioSource.PlayClipAtPoint(achievementEarnedSound, Camera.main.transform.position);    
		achievmentToDisplay = a;

		// Add achievement to totalAchieved
		AchievementController.Instance.totalAchieved++;

		// Set the achievement as achieved
		AchievementController.Instance.achievementsArray[a.id].achieved = true;

		// Display the achievement for 3 seconds
		timerActive = true;
		timer = 3.0f;
	}
	#endregion

	#region Update Achievement Progress
	private void updateAchievements(){
		if (AchievementController.Instance.achievementsArray != null) {
			for (int i=0; i<AchievementController.Instance.achievementsArray.Length; i++) {
				// Ensure a notification is not in place
				if (!timerActive) {
					if (AchievementController.Instance.achievementsArray[i].type == "Tower Building") {
						if (AchievementController.Instance.achievementsArray[i].SetProgress(AchievementController.Instance.towersBuilt)) {
							achievementEarned(AchievementController.Instance.achievementsArray[i]);
						}
					} else if (AchievementController.Instance.achievementsArray[i].type == "Endurance") {
						if (AchievementController.Instance.achievementsArray[i].SetProgress(AchievementController.Instance.totalTimePlayed)) {
							achievementEarned(AchievementController.Instance.achievementsArray[i]);
						}
					} else if (AchievementController.Instance.achievementsArray[i].type == "Enemies Killed") {
						if (AchievementController.Instance.achievementsArray[i].SetProgress(AchievementController.Instance.enemiesDestroyed)) {
							achievementEarned(AchievementController.Instance.achievementsArray[i]);
						}
					}
					else if(AchievementController.Instance.achievementsArray[i].type == "Rank"){
						if (AchievementController.Instance.achievementsArray[i].SetProgress(AchievementController.Instance.totalAchieved)) {
							// change current rank
							AchievementController.Instance.currentRank++;
							achievementEarned(AchievementController.Instance.achievementsArray[i]);
						}
					}
				}
			}
		}
	}
	#endregion

	void Update(){
		//Update the players time played
		AchievementController.Instance.totalTimePlayed = AchievementController.Instance.timePlayed + (Time.timeSinceLevelLoad/60);

		// Update achievements
		updateAchievements ();

		// Update timer if its active
		if (timerActive) {
			// If the timer is finished, set active to false and achievement display to null
			if (timer <= 0.0f){
				timer = 0.0f;
				timerActive = false;
				achievmentToDisplay = null;
			}
			else{
				timer -= Time.deltaTime;
			}
		}
	}

	void OnGUI(){
		// Create Notification if an achievement has been achieved
		if (achievmentToDisplay!=null){
			achievmentToDisplay.GUINotify(GUIStyleAchievement, GUIStyleAmazing, GUIStyleGeneral, GUINotifierStyle);
		}
	}
	
}
