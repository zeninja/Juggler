using UnityEngine;
using System.Collections;

public class NewTweens : MonoBehaviour {

	public AnimationCurve easeOutBackCurve;
	public AnimationCurve pulseCurve;

	void Start() {
		
	}

	public IEnumerator MoveToEaseOutBack(GameObject target, Vector3 from, Vector3 to, float duration) {
		float t = 0; 
		Vector3 difference = to - from;

		while ( t < 1 ) {
			t += Time.deltaTime/duration;
			target.transform.position = from + easeOutBackCurve.Evaluate(t) * difference;
			yield return new WaitForEndOfFrame();
		}
	}

	public IEnumerator ScaleToEaseOutBack(GameObject target, Vector3 from, Vector3 to, float duration) {
		float t = 0; 
		Vector3 difference = to - from;

		while ( t < 1 ) {
			t += Time.deltaTime/duration;
			target.transform.localScale = from + easeOutBackCurve.Evaluate(t) * difference;
			yield return new WaitForEndOfFrame();
		}
	}

	public IEnumerator PulseScale(GameObject target, Vector3 pulseAmount, float duration) {
		StopCoroutine("PulseScale");
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
