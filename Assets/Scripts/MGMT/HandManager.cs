﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour {

	public GameObject handPrefab;
	static Dictionary<int, GameObject> hands = new Dictionary<int, GameObject>();

	public static int totalHandCount;

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
		#if UNITY_EDITOR
		ManageInput();
		#endif

		ManageTouches();
	}


	void ManageInput() {
		if (Input.GetMouseButtonDown(0)) {
			SpawnHand(0);
		}
	}

	void ManageTouches() {
		for (int i = 0; i < Input.touchCount; i++) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				SpawnHand(Input.GetTouch(i).fingerId);
			}
		}
	}

	void SpawnHand(int id) {
		GameObject hand = ObjectPool.instance.GetObjectForType("Hand", false);
		hand.GetComponent<GrabControllerFlick>().id = id;

		if(!hands.ContainsKey(id)) {
			hands.Add(id, hand);
		} else {
			ObjectPool.instance.PoolObject(hands[id]);
			hands.Remove(id);
			hands.Add(id, hand);
		}

		totalHandCount = Mathf.Max(totalHandCount, hands.Count);
	}

	public static void RemoveHand(int id) {
		hands.Remove(id);
	}
}
