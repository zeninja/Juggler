using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DotManager : MonoBehaviour {

	GameObject[] dots;

	// Use this for initialization
	void Start () {
		dots = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			dots[i] = transform.GetChild(i).GetChild(0).gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.C)) {
			StartCoroutine(CascadeDots());
		}
	}

	IEnumerator CascadeDots() {
		float duration = .5f;
		for(int i = 0; i < dots.Length; i++) {
//			iTween.MoveTo(dots[i], iTween.Hash("position", new Vector2(50, 0), "time", duration, "delay", 0, "isLocal", true));
			yield return new WaitForSeconds(duration/2);
		}
	}
}
