using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject ball;
	bool ballSpawned;

	public static bool gameOver;

	private static GameManager instance;
	private static bool instantiated;

	List<GameObject> balls = new List<GameObject>();

	public static GameManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(GameManager)) as GameManager;
			if (!instance)
				Debug.Log("No GameManager!!");		
		}
		return instance;
	}

	// Use this for initialization
	void Start () {
		SpawnBall();
	}
	
	// Update is called once per frame
	void Update () {
		DebugSpawn();
	}

	void DebugSpawn() {
		#if UNITY_EDITOR 
		if (Input.GetKeyDown(KeyCode.Space) || (Input.touches.Length == 2 && !ballSpawned) && !gameOver) {
			SpawnBall();
			ballSpawned = true;
		}

		if (Input.touches.Length == 0) {
			ballSpawned = false;
		}
		#endif
	}

	public void SpawnBall() {
		if(gameOver) { return; }

		GameObject newBall = Instantiate(ball);
		newBall.transform.position = new Vector3(Random.Range(-2.3f, 2.3f), 9.5f, 0);
		balls.Add(newBall);
	}

	public void DestroyBalls() {
		for (int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<Ball>().HandleDeath();
		}
		balls.Clear();
	}

	public void HandleGameOver() {
		if(gameOver) { return; }

		gameOver = true;
		DestroyBalls();

		ScoreManager.GetInstance().HandleGameOver();

		Invoke("Restart", 5);
	}

	void Restart() {
		gameOver = false;

		SpawnBall();
	}
}
