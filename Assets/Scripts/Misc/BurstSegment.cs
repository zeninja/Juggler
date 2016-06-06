using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BurstSegment : MonoBehaviour {

	Image image;
	public float burstDuration = .5f;

	Vector2 originalScale;

	void Awake() {
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)) {
			StartAnimation();	
		}
	}

	public void StartAnimation() {
		StartCoroutine("Animate");
	}

	IEnumerator Animate() {
		yield return StartCoroutine(AnimateIn());
		yield return StartCoroutine(AnimateOut());
		Reset();
	}

	IEnumerator AnimateIn() {
		float t = 0;
		float sectionDuration = burstDuration/2;

		while (t < sectionDuration) {
			t += Time.fixedDeltaTime;
			image.GetComponent<Image>().fillAmount = t/sectionDuration;
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator AnimateOut() {
		float t = 0;
		float sectionDuration = burstDuration/2;
		image.GetComponent<Image>().fillOrigin = 1;

		while (t < sectionDuration) {
			t += Time.fixedDeltaTime;
			image.GetComponent<Image>().fillAmount = 1 - t/sectionDuration;
			yield return new WaitForEndOfFrame();
		}
	}

	void Reset() {
		image.GetComponent<Image>().fillOrigin = 0;
		image.GetComponent<Image>().fillAmount = 0;
	}
}