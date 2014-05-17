using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject amazingText;
	public GameObject tdText;
	public GameObject resumeGameText;
	
	void Start(){

		if (Time.timeScale == 0) {
			Destroy(GameObject.Find("amazing_text"));
			Destroy(GameObject.Find("td_text"));
			Instantiate(resumeGameText);
		} else {
			Instantiate(amazingText);
			Instantiate(tdText);
			Destroy(GameObject.Find("Resume_Game_text(Clone)"));
		}
	}
}
