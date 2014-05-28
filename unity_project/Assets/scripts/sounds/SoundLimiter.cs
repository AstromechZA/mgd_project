using UnityEngine;
using System.Collections;

public class SoundLimiter : MonoBehaviour {

	// Maximum number of sounds that can play at once for each tower type
	private int maxGun = 1;
	private int maxMissile = 2;
	private int maxLaser = 2;
	
	// Sound assets
	public AudioClip gun_sound;
	public AudioClip missile_sound;
	public AudioClip laser_sound;

	// Current number of sounds playing for each tower type
	private int gunPlaying = 0; 
	private int missilePlaying = 0;
	private int laserPlaying = 0;

	// Gun tower
	public void playGun(){
		if (gunPlaying < maxGun) 
			StartCoroutine("gunSound");
	}

	private IEnumerator gunSound(){
		gunPlaying++;
		AudioSource.PlayClipAtPoint (gun_sound, Camera.main.transform.position);
		yield return new WaitForSeconds (gun_sound.length);
		gunPlaying--;
	}


	// Missile tower
	public void playMissile(){
		if (missilePlaying < maxMissile) 
			StartCoroutine("missileSound");
	}
	
	private IEnumerator missileSound(){
		missilePlaying++;
		AudioSource.PlayClipAtPoint (missile_sound, Camera.main.transform.position);
		yield return new WaitForSeconds (missile_sound.length);
		missilePlaying--;
	}


	// Laser tower
	public void playLaser(){
		if (laserPlaying < maxLaser) 
			StartCoroutine("laserSound");
	}
	
	private IEnumerator laserSound(){
		laserPlaying++;
		AudioSource.PlayClipAtPoint (laser_sound, Camera.main.transform.position);
		yield return new WaitForSeconds (laser_sound.length);
		laserPlaying--;
	}
}