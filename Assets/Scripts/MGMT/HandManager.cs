using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour {

	int prevTouchCount;
	public GameObject handPrefab;

	static Dictionary<int, GameObject> hands = new Dictionary<int, GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ManageTouches();
		DebugHand();
	}

	void ManageTouches() {
		for (int i = 0; i < Input.touchCount; i++) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				SpawnHand(Input.GetTouch(i).fingerId);
			}

//			if (Input.GetTouch(i).phase == TouchPhase.Ended) {
//				RemoveHand(Input.GetTouch(i).fingerId);
//			}
		}
	}

	void SpawnHand(int id) {
		GameObject hand = Instantiate(handPrefab) as GameObject;
		hand.GetComponent<GrabControllerDragDownDirect>().id = id;
		hands.Add(id, hand);
	}

	public static void RemoveHand(int id) {
		Destroy(hands[id]);
		hands.Remove(id);
	}

	bool handSpawned;
	void DebugHand() {
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		if (Input.GetMouseButton(0) && !handSpawned) {
			Instantiate(handPrefab);
			handSpawned = true;
		}
		#endif
	}
}
