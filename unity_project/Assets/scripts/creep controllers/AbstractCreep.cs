using UnityEngine;
using System.Collections;

public class AbstractCreep : MonoBehaviour {

	public float startingHealth = 10;
	public int bountyValue = 1;

	// -----------------------
	private float healthValue;

	void Awake() {
		healthValue = startingHealth;
	}

	void Start () {
		NearestCreepFinder.Instance.Register(this);
	}

	public void Hit(float damage) {
		healthValue -= damage;
		if( healthValue <= 0) {
			int bounty = bountyValue + (int)(PerkController.Instance.GetPerkBonus(Perk.PerkType.CREEP_ALL_REWARD));
			AchievementController.Instance.enemiesDestroyed++;
			AchievementController.Instance.moneyMade += bounty;
			GameController.Instance.citadelCredits += bounty;
			PerkController.Instance.AddExperience(2);
			Destroy(gameObject);
		}
	}

	public void OnDestroy() {
		Debug.Log("Deregister creep");
		if (NearestCreepFinder.Instance != null) NearestCreepFinder.Instance.Deregister(this);
	}
}
