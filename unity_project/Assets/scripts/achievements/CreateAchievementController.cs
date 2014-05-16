using UnityEngine;
using System.Collections;

public class CreateAchievementController : MonoBehaviour {

	//Create an AchievementController if there isnt one already
	void Start () {
		if( FindObjectOfType(typeof(AchievementController)) == null )	
		{
			
			GameObject newAchievementController = new GameObject();
			
			newAchievementController.name = "AchievementController";
			
			newAchievementController.AddComponent<AchievementController>();
		}
	}
}
