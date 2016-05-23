using UnityEngine;
using System.Collections;

public class NewTweens : MonoBehaviour {

	public AnimationCurve easeOutBackCurve;

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
}
