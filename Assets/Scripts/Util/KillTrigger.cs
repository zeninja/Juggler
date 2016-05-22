using UnityEngine;
using System.Collections;

public class KillTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag("Ball")) {
			GameManager.GetInstance().HandleGameOver();
		}
	}
}
