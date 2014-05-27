using UnityEngine;
using System.Collections;

public class CentreAlignTextMesh : MonoBehaviour {

	// Centre align the text
	void Awake () {
		GetComponent<TextMesh>().alignment = TextAlignment.Center;
	}
}
