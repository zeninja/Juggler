using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BurstController : MonoBehaviour {

	public BurstSegment[] bursts;

	void Start() {
		for (int i = 0; i < bursts.Length; i++) {
			bursts[i].GetComponent<Image>().enabled = false;
		}
	}

	public void Burst() {
		for (int i = 0; i < bursts.Length; i++) {
			bursts[i].GetComponent<Image>().enabled = true;
			bursts[i].GetComponent<BurstSegment>().StartAnimation();
		}
	}
}
