using UnityEngine;
using System.Collections;

public class BurstController : MonoBehaviour {

	public BurstSegment[] bursts;

	void Start() {
		for (int i = 0; i < bursts.Length; i++) {
			bursts[i].gameObject.SetActive(false);
		}
	}

	public void Burst() {
		for (int i = 0; i < bursts.Length; i++) {
			bursts[i].gameObject.SetActive(true);
			bursts[i].GetComponent<BurstSegment>().StartAnimation();
		}
	}
}
