using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour {

	SpriteRenderer[] spriteRenderers;
	public float duration = .25f;
	public float targetScaleMultiplier = 2.5f;

	// Use this for initialization
	void Start () {
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
	}

	public void StartSpread() {

		StartCoroutine(Spread());
	}

	IEnumerator Spread() {
		// wait a few frames so that when the ring starts spreading it will be rotated (if the player has moved their finger)
		for(int i = 0; i < 3; i++) {
			yield return new WaitForEndOfFrame();
		}

		#if !UNITY_EDITOR
		transform.parent = null;
		#endif

		for (int i = 0; i < spriteRenderers.Length; i++) {
			spriteRenderers[i].enabled = true;
		}
		float t = 0;

		while(t < 1) {
			t += Time.fixedDeltaTime/duration;
			transform.localScale = Vector3.one * (1 + targetScaleMultiplier * t);
			yield return new WaitForEndOfFrame();
		}
		Reset();
		#if !UNITY_EDITOR
		Destroy(gameObject);
		#endif
	}

	void Reset() {
		#if !UNITY_STANDALONE_IOS
		for (int i = 0; i < spriteRenderers.Length; i++) {
			spriteRenderers[i].enabled = false;
		}
		transform.localScale = Vector3.one;
		#endif
	}
}
