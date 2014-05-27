using UnityEngine;
using System.Collections;

public class MenuTextBehaviour : MonoBehaviour {

	void Update(){
		if (!(GameObject.Find ("soundtrack_menu").audio.isPlaying))
			GameObject.Find ("soundtrack_menu").audio.Play();
	}


	void OnMouseDown () {

		renderer.material.color = Color.green;

	}

	void OnMouseUp () {
		
		renderer.material.color = Color.red;

		GameObject.Find ("menu_select").audio.Play ();
		
		// Play the game (go to scene_level)
		if (GameObject.Find ("Menu Block/Play_Game_text") == this.gameObject) {
			GameObject.Find ("soundtrack_menu").audio.Pause();

			GameObject.Find ("soundtrack_level").audio.Stop ();
			GameObject.Find ("soundtrack_level").audio.PlayDelayed (4.0F);
			// Reset Game Parameters
			GameController.Instance.ResetGameParameters();
			GameController.Instance.DestroyAllObjectsWithTag("Instantiable Object");
			GameController.Instance.DestroyAllObjectsWithTag("Enemy");
			Application.LoadLevel("gridtest");
		}
		// Handle Achievements
		else if (GameObject.Find ("Menu Block/Achievement_text") == this.gameObject) {
			Application.LoadLevel ("achievements");
		}		
		// Resume the Game
		else if(GameObject.Find ("Resume_Game_text(Clone)") == this.gameObject){
			GameObject.Find ("soundtrack_menu").audio.Pause();
			Application.LoadLevel ("gridtest");	
		}
		// Exit game
		else{
			Application.Quit();
		}
	
	}
}
