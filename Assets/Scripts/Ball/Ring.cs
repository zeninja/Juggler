using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour {

	Transform hand;

	SpriteRenderer[] spriteRenderers;
	public float duration = .25f;
	public float targetScaleMultiplier = 2.5f;

	// Use this for initialization
	void Start () {
		hand = transform.parent;
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
	}

	public void SetRotation(Vector3 targetPos) {
//		Vector3 diff = targetPos - hand.position;
//        diff.Normalize();
// 
//        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
//
//		for (int i = 0; i < transform.childCount; i++) {
//			transform.GetChild(i).rotation = Quaternion.Euler(rot_z, 0, transform.GetChild(i).rotation.eulerAngles.z);
//		}
	}

	public void StartSpread() {
		StartCoroutine(Spread());
	}

	IEnumerator Spread() {
		// wait a few frames so that when the ring starts spreading it will be rotated (if the player has moved their finger)
		for(int i = 0; i < 3; i++) {
			yield return new WaitForEndOfFrame();
		}

		transform.parent = null;

		for (int i = 0; i < spriteRenderers.Length; i++) {
			spriteRenderers[i].enabled = true;
		}
		float t = 0;

		while(t < 1) {
			t += Time.fixedDeltaTime/duration;
			transform.localScale = Vector3.one * (1 + targetScaleMultiplier * t);
			yield return new WaitForEndOfFrame();
		}

		for (int i = 0; i < spriteRenderers.Length; i++) {
			spriteRenderers[i].enabled = false;
		}

		transform.parent = hand;
		transform.localScale = Vector3.one;
	}
}
