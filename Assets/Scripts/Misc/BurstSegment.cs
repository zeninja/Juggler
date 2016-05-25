using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BurstSegment : MonoBehaviour {

//	public GameObject innerDot, rectangle, outerDot;
	public Image sprite;
	public float burstDuration = .5f;

	float shortSegmentMultiplier = .15f;
	float longSegmentMultiplier  = .3f;

	public float width;

	Vector2 originalScale;

	// Use this for initialization
	void Start () {

//		originalScale = innerDot.transform.localScale;
//		innerDot.transform.localScale = Vector3.zero;
//		outerDot.transform.localScale = Vector3.zero;
//
//		rectangle.GetComponent<Image>().fillOrigin = 0;
//		rectangle.GetComponent<Image>().fillAmount = 0;
//
//		rectangle.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width * 100);
//		rectangle.GetComponent<RectTransform>().localPosition = new Vector3(width + .1f * width, 0, 0);
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
//		yield return StartCoroutine(ScaleUp());
		yield return StartCoroutine(AnimateIn());
		yield return StartCoroutine(AnimateOut());
//		yield return StartCoroutine(ScaleDown());
		Reset();
	}

		#region old version
//	IEnumerator ScaleUp() {
//		float t = 0;
//		float sectionDuration = burstDuration * shortSegmentMultiplier;
//
//		while (t < sectionDuration) {
//			t += Time.fixedDeltaTime;
//
//			Vector2 currentScale = originalScale * t/sectionDuration;
//
//			if (currentScale.x > originalScale.x) {
//				currentScale = originalScale;
//			}
//
//			innerDot.transform.localScale = currentScale;
//			outerDot.transform.localScale = currentScale;
//			yield return new WaitForEndOfFrame();
//		}
//	}
//
//	IEnumerator ScaleDown() {
//		float t = 0;
//		float sectionDuration = burstDuration;
//
//		while (t < sectionDuration) {
//			t += Time.fixedDeltaTime;
//
//			Vector2 currentScale = originalScale * (1 - t/sectionDuration);
//
//			innerDot.transform.localScale = currentScale;
//			outerDot.transform.localScale = currentScale;
//			yield return new WaitForEndOfFrame();
//		}
//
//		innerDot.transform.localScale = Vector2.zero;
//		outerDot.transform.localScale = Vector2.zero;
//	}
//
//	IEnumerator AnimateIn() {
//		float t = 0;
//		float sectionDuration = burstDuration/2;
//
//		while (t < sectionDuration) {
//			t += Time.fixedDeltaTime;
//			outerDot.transform.localPosition = new Vector2(2 * t/sectionDuration * width, 0);
//			rectangle.GetComponent<Image>().fillAmount = t/sectionDuration;
//			yield return new WaitForEndOfFrame();
//		}
//	}
//
//	IEnumerator AnimateOut() {
//		float t = 0;
//		float sectionDuration = burstDuration/2;
//		rectangle.GetComponent<Image>().fillOrigin = 1;
//
//		while (t < sectionDuration) {
//			t += Time.fixedDeltaTime;
//			innerDot.transform.localPosition = new Vector2(2 * t/sectionDuration * width, 0);
//
//			rectangle.GetComponent<Image>().fillAmount = 1 - t/sectionDuration;
//			yield return new WaitForEndOfFrame();
//		}
//	}
		#endregion

	IEnumerator AnimateIn() {
		float t = 0;
		float sectionDuration = burstDuration/2;

		while (t < sectionDuration) {
			t += Time.fixedDeltaTime;
			sprite.GetComponent<Image>().fillAmount = t/sectionDuration;
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator AnimateOut() {
		float t = 0;
		float sectionDuration = burstDuration/2;
		sprite.GetComponent<Image>().fillOrigin = 1;

		while (t < sectionDuration) {
			t += Time.fixedDeltaTime;
			sprite.GetComponent<Image>().fillAmount = 1 - t/sectionDuration;
			yield return new WaitForEndOfFrame();
		}
	}

	void Reset() {

//		innerDot.transform.localPosition = new Vector2(0, 0);
//		outerDot.transform.localPosition = new Vector2(0, 0);
//
		sprite.GetComponent<Image>().fillOrigin = 0;
		sprite.GetComponent<Image>().fillAmount = 0;
	}
}