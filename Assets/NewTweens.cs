using UnityEngine;
using System.Collections;

public class NewTweens : MonoBehaviour {

	public AnimationCurve easeOutBackCurve;
	static AnimationCurve _easeOutBackCurve;

	void Start() {
		_easeOutBackCurve = easeOutBackCurve;
	}

//	public float easeOutBack(float t, float from, float to, float duration) {
//		float t = 0;
//		while (t < 1) {
//			t = Time.deltaTime/duration;
//			Debug.Log(easeOutBackCurve.Evaluate(t));
//			return easeOutBackCurve.Evaluate(t) * (from - to);
//		}
//		return 0;
//	}

//	public float easeOutBack(float t, float from, float to) {
//		return easeOutBackCurve.Evaluate(t) * (from - to);
//	}

	public IEnumerator MoveToEaseOutBack(GameObject target, Vector3 from, Vector3 to, float duration) {
		float t = 0; 
		Vector3 difference = to - from;

		while ( t < 1 ) {
			t += Time.deltaTime/duration;
			target.transform.position = from + _easeOutBackCurve.Evaluate(t) * difference;
			yield return new WaitForEndOfFrame();
		}
	}


}
