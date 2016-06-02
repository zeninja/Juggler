using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour {

	public GameObject handPrefab;
	static Dictionary<int, GameObject> hands = new Dictionary<int, GameObject>();

	private static HandManager instance;
	private static bool instantiated;

	public static HandManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(HandManager)) as HandManager;
			if (!instance)
				Debug.Log("No HandManager!!");
		}
		return instance;
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
		}
	}

	void SpawnHand(int id) {
		GameObject hand = Instantiate(handPrefab) as GameObject;
		hand.GetComponent<GrabControllerFlick>().id = id;
		hands.Add(id, hand);
	}

	public static void RemoveHand(int id) {
		hands.Remove(id);
	}

	void HandleGameOver() {
		for (int i = 0; i < hands.Count; i++) {
			hands[i].GetComponent<GrabControllerFlick>().HandleDeath();
		}
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
