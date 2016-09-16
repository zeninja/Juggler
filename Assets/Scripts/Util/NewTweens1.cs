using UnityEngine;
using System.Collections;

public static class NewTweens1 {

	public static AnimationCurve easeOutBackCurve;
	public static AnimationCurve pulseCurve;
//	public AnimationCurve newPulseCurve;

	public static IEnumerator MoveToEaseOutBack(GameObject target, Vector3 from, Vector3 to, float duration) {
		float t = 0; 
		Vector3 difference = to - from;

		while ( t < 1 ) {
			t += Time.deltaTime/duration;
			target.transform.position = from + easeOutBackCurve.Evaluate(t) * difference;
			yield return new WaitForEndOfFrame();
		}
	}

	public static IEnumerator ScaleToEaseOutBack(GameObject target, Vector3 from, Vector3 to, float duration) {
		float t = 0; 
		Vector3 difference = to - from;

		while ( t < 1 ) {
			t += Time.deltaTime/duration;
			target.transform.localScale = from + easeOutBackCurve.Evaluate(t) * difference;
			yield return new WaitForEndOfFrame();
		}
	}

	public static IEnumerator PulseScale(GameObject target, Vector3 pulseAmount, float duration) {
//		StopCoroutine("PulseScale");
		target.transform.localScale = Vector3.one;

		float t = 0;
		Vector3 originalScale = Vector3.one;

		while ( t < 1 ) {
			t += Time.deltaTime/duration;
			target.transform.localScale = originalScale + pulseCurve.Evaluate(t) * pulseAmount;
			yield return new WaitForEndOfFrame();
		}
	}
}
