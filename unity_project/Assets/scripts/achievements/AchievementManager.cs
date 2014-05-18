using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	#region Variables
	// GUI styles
	public GUIStyle GUIStyleAchievement;
	public GUIStyle GUIStyleAmazing;
	public GUIStyle GUIStyleGeneral;
	public GUIStyle GUIStyleManagerBackground;
	
	// Achievements completed and Total achiements 
	private int currentAchievementPoints = 0;
	private int potentialAchievementPoints = 0;
	
	// Rank and Trophy images
	private Texture2D iconRank;
	private Texture2D iconTrophy;
	
	// Width of GUI
	private float achievementGUIWidth;
	
	// GUI component position variables
	private Rect backButton;
	
	
	private Rect resetButton;
	private Rect resetBoxPosition;
	private bool reset;
	private Rect resetBoxLabelPosition;
	private Rect resetBoxLabelPosition2;
	private Rect resetBoxYesButtonPosition;
	private Rect resetBoxNoButtonPosition;
	
	private Rect headerBoxPosition;
	private Rect logoPosition;
	private Rect amazingTdText;
	private Rect achievementsHeading;
	private Rect trophyPosition;
	private Rect rewardPointTextPosition;
	private Rect scrollViewPosition;
	private Rect scrollViewRectangle;
	
	private Color guiStyleAchievmentOriginal;
	
	// Ranks
	private string[] rankImages = new string[7]{"bronze_rank","silver_rank", "gold_rank", "platinum_rank", "diamond_rank", "master_rank","grandmaster_rank"};
	// Position of Achievement scroll and touch
	private Vector2 scrollPosition;
	private Vector2 achievementScrollviewLocation = Vector2.zero;
	private Touch touch;
	#endregion
	
	void Start(){
		
		setupAchievements();
		
		// Load player rank and trophy icon
		iconRank = (Texture2D)Resources.Load ("Achievements/ranks/"+rankImages[AchievementController.achievementController.currentRank]);
		iconTrophy = (Texture2D)Resources.Load ("Achievements/Trophy-icon");
		
		// GUI component positions
		achievementGUIWidth = (float)Screen.width;
		resetButton = new Rect(achievementGUIWidth-100, 7, 80, 30);
		resetBoxPosition = new Rect(achievementGUIWidth-200, 50.0f, 200.0f, 100.0f);
		resetBoxLabelPosition = new Rect(achievementGUIWidth-190, 60.0f, 200.0f, 30.0f);
		resetBoxLabelPosition2 = new Rect(achievementGUIWidth-190, 80.0f, 200.0f, 30.0f);
		resetBoxYesButtonPosition = new Rect(achievementGUIWidth-180, 110.0f, 65.0f, 30.0f);
		resetBoxNoButtonPosition = new Rect(achievementGUIWidth-85, 110.0f, 65.0f, 30.0f);
		
		//headerBoxPosition = new Rect(-3.0f, 0.0f, achievementGUIWidth+5, 45.0f);
		logoPosition = new Rect(25.0f, 0.0f, 180.0f, 180.0f);
		amazingTdText = new Rect(230.0f, 47.0f, 200.0f, 25.0f);
		achievementsHeading = new Rect(230.0f, 80.0f, 200.0f, 25.0f);
		trophyPosition = new Rect(230.0f, 120.0f, 17.0f, 17.0f);
		rewardPointTextPosition = new Rect(255.0f, 120.0f, 200.0f, 25.0f);
		scrollViewPosition = new Rect(0.0f, 160.0f, achievementGUIWidth, (float)Screen.height - 160.0f);
		
		// Font set up
		GUIStyleAchievement.normal.textColor = Color.white;
		GUIStyleAmazing.normal.textColor = Color.white;
		Color c1 = new Color(0.52f, 0.52f, 0.52f, 1.0f);
		GUIStyleGeneral.normal.textColor = c1;
		GUIStyleAchievement.fontSize = 18;
		GUIStyleAmazing.fontSize = 26;
		GUIStyleGeneral.fontSize = 16;
		guiStyleAchievmentOriginal = GUIStyleAchievement.normal.textColor; 
		
		// View rectangle
		scrollViewRectangle = new Rect(0.0f, 0.0f, achievementGUIWidth, (AchievementController.achievementController.achievementsArray.Count () * 90.0f));
		GUIStyleManagerBackground.normal.background = (Texture2D)Resources.Load ("Achievements/bases/notifier_background_2");
			

	}
	
	#region Setup Achievements
	private void setupAchievements()
	{
		currentAchievementPoints = 0;
		potentialAchievementPoints = 0;
		
		foreach (Achievement achievement in AchievementController.achievementController.achievementsArray)
		{
			// Setup current and potential achievment points
			if (achievement.achieved)
			{
				currentAchievementPoints += achievement.rewardPoints;
			}			
			potentialAchievementPoints += achievement.rewardPoints;
		}

		if (AchievementController.achievementController.achievementsArray != null) {
			for (int i=0; i<AchievementController.achievementController.achievementsArray.Length; i++) {
				// Ensure a notification is not in place
				if (AchievementController.achievementController.achievementsArray[i].type == "Tower Building") {
					AchievementController.achievementController.achievementsArray[i].SetProgress(AchievementController.achievementController.towersBuilt);
				} else if (AchievementController.achievementController.achievementsArray[i].type == "Endurance") {
					AchievementController.achievementController.achievementsArray[i].SetProgress(AchievementController.achievementController.totalTimePlayed);
				} else if (AchievementController.achievementController.achievementsArray[i].type == "Enemies Killed") {
					AchievementController.achievementController.achievementsArray[i].SetProgress(AchievementController.achievementController.enemiesDestroyed);
				}
				else if(AchievementController.achievementController.achievementsArray[i].type == "Rank"){
					AchievementController.achievementController.achievementsArray[i].SetProgress(AchievementController.achievementController.totalAchieved);
				}
			}
		}
	}
	#endregion
	
	void OnGUI()
	{
		// Create a box of the screen (Change its colour to the background colour required)
		// Note -> Cant rely on camera colour as game objects appear through it
		GUI.Box(new Rect(0,0,Screen.width, Screen.height),"",GUIStyleManagerBackground);
		
		// Reset Button
		if (GUI.Button (resetButton, "Reset")) {
			// Pop up with are you sure yes/no
			if (reset){
				reset = false;
			}
			else{
				reset = true;
			}
		}
		
		if(reset) {
			GUI.Box(resetBoxPosition, "");
			GUI.Label(resetBoxLabelPosition, "Are you sure you want to ", GUIStyleGeneral);
			GUI.Label(resetBoxLabelPosition2, "reset your achievements?", GUIStyleGeneral);
			
			if (GUI.Button(resetBoxYesButtonPosition, "Yes")) {
				//Reset
				AchievementController.achievementController.resetAchievements();
				currentAchievementPoints = 0;
				reset = false;
			}
			else if (GUI.Button(resetBoxNoButtonPosition, "No")) {
				// Cancel Reset
				reset = false;
			}
		}
		
		// Initial start from top
		float yValue = 0.0f; 
		
		// Create Achievement Header
		GUI.DrawTexture(logoPosition, iconRank);
		GUI.Label(amazingTdText, "aMazing TD", GUIStyleAmazing);			
		GUIStyleAchievement.normal.textColor = GUIStyleGeneral.normal.textColor; // Change text colour
		GUIStyleAchievement.fontSize = 24;
		GUI.Label(achievementsHeading, "ACHIEVEMENTS",GUIStyleAchievement);
		GUIStyleAchievement.fontSize = 18;
		GUI.DrawTexture(trophyPosition, iconTrophy);
		GUI.Label(rewardPointTextPosition, currentAchievementPoints + "/" + potentialAchievementPoints, GUIStyleAchievement);
		GUIStyleAchievement.normal.textColor = guiStyleAchievmentOriginal; // Reset text colour
		
		// Scrollview Setup (full of all the achievements.
		achievementScrollviewLocation = GUI.BeginScrollView(scrollViewPosition, achievementScrollviewLocation, scrollViewRectangle, GUIStyle.none, GUIStyle.none);
		foreach (Achievement achievement in AchievementController.achievementController.achievementsArray)
		{
			Rect position = new Rect(-3.0f, yValue, achievementGUIWidth+5, 88.0f);
			achievement.GUIManage(position,GUIStyleAchievement, GUIStyleAmazing, GUIStyleGeneral);
			yValue += 90.0f;
		}
		GUI.EndScrollView();
	}
	
	// Handle touch input on the scroll view & Listen for Back Button Presses
	void Update()
	{
		int scrollVelocity = 3;
		if(Input.touchCount > 0)
		{
			touch = Input.touches[0];
			if (touch.phase == TouchPhase.Moved)
			{
				achievementScrollviewLocation.y += scrollVelocity*touch.deltaPosition.y;
			}
		}
		// Go back to menu if user presses back button
		if (Input.GetKeyDown (KeyCode.Escape)) { 
			GameObject.Find ("menu_back").audio.Play ();
			Application.LoadLevel ("menu");
		}
	}
}
