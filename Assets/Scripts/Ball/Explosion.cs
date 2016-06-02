using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float explosionDuration = .3f;
	public AnimationCurve explosionCurve;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Trigger() {
		GetComponent<SpriteRenderer>().enabled = true;
		StartCoroutine("ScaleUpAndDown");

//		gameObject.ScaleTo(Vector3.one, explosionDuration/2, 0);
//		gameObject.ScaleTo(Vector3.zero, explosionDuration/2, explosionDuration/2);
		Invoke("Die", explosionDuration);
	}


	IEnumerator ScaleUpAndDown() {
		float t = 0; 

		while (t < 1) {
			t += Time.deltaTime/explosionDuration;
			transform.localScale = Vector3.one * explosionCurve.Evaluate(t);

			yield return new WaitForEndOfFrame();
		}
	}

	void Die() {
		Destroy(gameObject);
	}
}
