using UnityEngine;
using System.Collections;

public class CreepOneAnimator : MonoBehaviour {

	private Transform  	legLeftOneBone, 	legRightOneBone,
						legLeftTwoBone, 	legRightTwoBone,
						legLeftThreeBone, 	legRightThreeBone;

	private float progress = 0.0f;

	private const float animationRate = 10;
	private float cAnimationRate = animationRate;

	private float maxLegAngle = 20;
	private Quaternion baseQ = new Quaternion(-0.7f, 0f, 0f, 0.7f);

	void Start () {
		mapBones();
	}

	void Update () {
		// increment progress
		progress += cAnimationRate * Time.deltaTime;

		// edge cases swap animationRate sign
		if (progress > 1) {
			progress = 1;
			cAnimationRate *= -1;
		} else if (progress < -1) {
			progress = -1;
			cAnimationRate *= -1;
		}

		// set leg transforms
		setAllLegAngles(progress * maxLegAngle);
	}

	private void mapBones() {
		legLeftOneBone = transform.Find ("LeftOne");
		legLeftTwoBone = transform.Find ("LeftTwo");
		legLeftThreeBone = transform.Find ("LeftThree");

		legRightOneBone = transform.Find ("RightOne");
		legRightTwoBone = transform.Find ("RightTwo");
		legRightThreeBone = transform.Find ("RightThree");
	}

	private void setAllLegAngles(float angle) {
		// set angle for all legs, middle legs oppose
		setLegAngle(legLeftOneBone, angle);
		setLegAngle(legLeftTwoBone, -angle);
		setLegAngle(legLeftThreeBone, angle);
		setLegAngle(legRightOneBone, angle);
		setLegAngle(legRightTwoBone, -angle);
		setLegAngle(legRightThreeBone, angle);
	}

	private void setLegAngle(Transform t, float angle) {
		t.localRotation = baseQ * Quaternion.Euler(0, 0, angle);
	}
}
