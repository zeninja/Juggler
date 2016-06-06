using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).GetComponent<Text>().enabled =  !GameManager.gameOver && !GameManager.gameStarted;
		}
	}
}
