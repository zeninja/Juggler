using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float explosionDuration = .3f;
	public AnimationCurve explosionCurve;

	Transform ball;

	// Use this for initialization
	void Start () {
		ball = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Trigger() {
		GetComponent<SpriteRenderer>().enabled = true;
		StartCoroutine("ScaleUpAndDown");
		Invoke("Die", explosionDuration);
	}


	IEnumerator ScaleUpAndDown() {
		float t = 0; 

		while (t < 1) {
			t += Time.fixedDeltaTime/explosionDuration;
			transform.localScale = Vector3.one * explosionCurve.Evaluate(t);

			yield return new WaitForEndOfFrame();
		}
		transform.localScale = Vector2.zero;
	}

	void Die() {
		transform.parent = ball;
		transform.localPosition = Vector2.zero;
	}
}
