using UnityEngine;
using System.Collections;

public class ControllerCreator : MonoBehaviour {


	void Start () {
		//Create an AchievementController if there isnt one already
		if( FindObjectOfType(typeof(AchievementController)) == null )	
		{
			
			GameObject newAchievementController = new GameObject();
			
			newAchievementController.name = "AchievementController";
			
			newAchievementController.AddComponent<AchievementController>();
		}
		// Create a GameController if there isnt one already
		if  (FindObjectOfType(typeof(GameController)) == null )	
		{
			
			GameObject newAchievementController = new GameObject();
			
			newAchievementController.name = "GameController";
			
			newAchievementController.AddComponent<GameController>();
		}
	}

}
