using System.Linq;
using UnityEngine;
using System.Xml.Serialization;

public class Achievement
{	
	// Serialised variables
	#region Serialised Variables
	[XmlAttribute("id")]
	public int id;

	public bool achieved;
	public string name;
	public string type;
	public string goal;
	public float target;
	public string image;
	public string skill;
	public int rewardPoints = 1;	
	#endregion
	
	private float currentProgress = 0.0f;

	private Texture2D achievementIcon;
	private Texture2D skillIcon;
	private Texture2D rankIcon;

	#region Colours
	private Color lightGrey;
	private Color darkerGrey;
	private Color cTenPercentAlpha;
	private Color cTwentyPercentAlpha;
	private Color cFiftyPercentAlpha;
	private Color cAlpha;
	#endregion

	// Manager vars
	private Rect achievementImagePosition;
	private Rect skillLevelImagePosition;
	private Rect targetLabelPosition;
	private string targetStringAchieved;
	private string targetStringUnachieved;

	#region Notifer Element Positions
	Rect notifierPosition;
	Rect notifierBoxPosition;
	Rect notifierBoxBorderPosition;
	Rect notifierSkillIconPositon;
	Rect notifierAchievementIconPosition;
	Rect notifierProgressPosition;
	Rect notifierAchievementNamePosition;
	Rect notifierAchievementGoalPosition;
	#endregion

	// Ranks
	private string[] rankImages = new string[7]{"bronze_rank","silver_rank", "gold_rank", "platinum_rank", "diamond_rank", "master_rank","grandmaster_rank"};

	
	#region Setup
	public void Setup(string achievement_image, string skill_level){

		if (skill_level != "none") {
			skillIcon = (Texture2D)Resources.Load("Achievements/skill_level/"+skill_level);
		}

		if (type == "Rank") {
			achievementIcon = (Texture2D)Resources.Load("Achievements/bases/"+achievement_image);
			rankIcon = (Texture2D)Resources.Load ("Achievements/ranks/" + rankImages [id - 31]);
		} else {
			achievementIcon = (Texture2D)Resources.Load("Achievements/achievement_categories/"+achievement_image);
		}


		// Colour setup
		lightGrey = new Color(0.35f, 0.35f, 0.35f, 1.0f);
		darkerGrey = new Color(0.25f, 0.25f, 0.25f, 1.0f);
		cTenPercentAlpha = new Color(1, 1, 1, 0.1f);
		cTwentyPercentAlpha = new Color(1, 1, 1, 0.2f);
		cFiftyPercentAlpha = new Color(1, 1, 1, 0.5f);

		// Manager Position Setup
		achievementImagePosition = new Rect (30.0f, 11.0f, 65.0f, 65.0f);
		skillLevelImagePosition = new Rect (25.0f, 6.0f, 75.0f, 75.0f);
		targetLabelPosition = new Rect (125.0f, 10.0f, 250.0f, 25.0f);

		targetStringAchieved = target.ToString ("0.#") + "/" + target.ToString ("0.#");
		targetStringUnachieved = currentProgress.ToString ("0.#") + "/" + target.ToString ("0.#");

		// Notifier Position Setup
		notifierPosition = new Rect((float)Screen.width - 270.0f,(float)Screen.height - 80.0f, 270.0f, 80.0f);
		notifierBoxPosition = new Rect(1.0f, 1.0f, notifierPosition.width-1, notifierPosition.height-1);
		notifierBoxBorderPosition = new Rect(0.0f, 0.0f, notifierPosition.width, notifierPosition.height);
		notifierSkillIconPositon = new Rect(15.0f, 6.0f, 65.0f, 65.0f);
		notifierAchievementIconPosition = new Rect(20.0f, 11.0f, 55.0f, 55.0f);
		notifierProgressPosition = new Rect(95.0f, 8.0f, 250.0f, 25.0f);
		notifierAchievementNamePosition = new Rect(95.0f, 27.0f, notifierPosition.width - 80.0f - 50.0f, 25.0f);
		notifierAchievementGoalPosition = new Rect (95.0f, 52.0f, notifierPosition.width - 80.0f, 25.0f);
	}
	#endregion

	#region Update Progress
	public bool AddProgress(float progress)
	{
		if (achieved)
		{
			return false;
		}
		
		currentProgress += progress;
		if (currentProgress >= target)
		{
			currentProgress = target;
			achieved = true;
			return true;
		}
		
		return false;
	}
	
	public bool SetProgress(float progress)
	{
		if (achieved)
		{
			return false;
		}
		
		currentProgress = progress;
		if (progress >= target)
		{
			currentProgress = target;
			achieved=true;
			return true;
		}
		
		return false;
	}
	#endregion
	
	#region GUI
	// Truncate Long achievement strings
	public string truncate(string text, int length){
		if (text.Length > length) {
			return text.Substring (0, length) + "...";
		} else {
			return text;
		}
	}
	
	#region Achievement Notifier 
	public void GUINotify(GUIStyle GUIStyleAchievement, GUIStyle GUIStyleAmazing, GUIStyle GUIStyleGeneral, GUIStyle GUINotifierStyle)
	{
		GUI.BeginGroup(notifierPosition);
			GUI.Box(notifierBoxPosition, "", GUINotifierStyle);
		GUI.Box(notifierBoxBorderPosition, "");
			if (skill != "none") {
				GUI.DrawTexture (notifierSkillIconPositon, skillIcon);
			}
			GUI.DrawTexture(notifierAchievementIconPosition, achievementIcon);
			if (type == "Rank"){
				GUI.DrawTexture(notifierAchievementIconPosition, rankIcon);
			}
		
		GUI.Label(notifierProgressPosition, currentProgress.ToString("0.#") + "/" + target.ToString("0.#"), GUIStyleGeneral);
			GUI.Label(notifierAchievementNamePosition, truncate(name, 20), GUIStyleAchievement);
			GUI.Label(notifierAchievementGoalPosition, truncate(goal, 23), GUIStyleGeneral);
		GUI.EndGroup();
	}
	#endregion

	#region Achievement Manager
	public void GUIManage(Rect position, GUIStyle GUIStyleAchievement, GUIStyle GUIStyleAmazing, GUIStyle GUIStyleGeneral)
	{
		Color temp = GUI.color;

		// Store original GUIStyle colours
		Color GUIStyleAchievementOriginalColour = GUIStyleAchievement.normal.textColor;
		Color GUIStyleGeneralOriginalColour = GUIStyleGeneral.normal.textColor;

		// Change text colour and Skill level image alpha
		if (!achieved) {
			GUIStyleAchievement.normal.textColor = lightGrey;
			GUIStyleGeneral.normal.textColor = darkerGrey;
			cAlpha = cTenPercentAlpha;
		} else {
			cAlpha = cFiftyPercentAlpha;
		}
		
		GUI.BeginGroup(position);
			GUI.Box(new Rect(0.0f, 0.0f, position.width, position.height), "");

			// Draw Texture if applicable
			if (skill != "none") {
				GUI.color = cAlpha;
				GUI.DrawTexture (skillLevelImagePosition, skillIcon);
				GUI.color = temp;
			}

			// Draw full alpha Achievement Image
			if (achieved)
			{
				GUI.DrawTexture(achievementImagePosition, achievementIcon);
				if (type == "Rank"){
					GUI.DrawTexture(achievementImagePosition, rankIcon);
				}
				GUI.Label(targetLabelPosition, targetStringAchieved, GUIStyleGeneral);
			}
			// Draw 20 percent alpha Achievement Image
			else
			{
				GUI.color = cTwentyPercentAlpha;
				GUI.DrawTexture(achievementImagePosition, achievementIcon);
				if (type == "Rank"){
					GUI.DrawTexture(achievementImagePosition, rankIcon);
				}
				GUI.color = temp;
				GUI.Label (targetLabelPosition, currentProgress.ToString ("0.#") + "/" + target.ToString ("0.#"), GUIStyleGeneral);
			}



			// Draw achievement name and goal
			GUI.Label(new Rect(125.0f, 33.0f, position.width - 80.0f - 50.0f, 25.0f), name, GUIStyleAchievement);
			GUI.Label(new Rect(125.0f, 60.0f, position.width - 80.0f, 25.0f), goal, GUIStyleGeneral);
		GUI.EndGroup();
		
		// Reset gui style colours
		GUIStyleAchievement.normal.textColor = GUIStyleAchievementOriginalColour;
		GUIStyleGeneral.normal.textColor = GUIStyleGeneralOriginalColour;
	}
	#endregion
	#endregion
}