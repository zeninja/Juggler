using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject ball;
	bool firstBallSpawned;
	bool debugBallSpawned;

	public static bool gameOver;

	private static GameManager instance;
	private static bool instantiated;

	List<GameObject> balls = new List<GameObject>();

	public UnityEngine.UI.Image ballImage;
	float inputDuration = 0;
	float spawnHoldThreshold = .5f;

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

	}
	
	// Update is called once per frame
	void Update () {
		DebugSpawn();
		SpawnBallOnInput();
	}

	void SpawnBallOnInput() {
		if (!gameOver && !firstBallSpawned) {
//			if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
//				SpawnBall();
//				firstBallSpawned = true;
//			}

			if (Input.touchCount > 0 || Input.GetMouseButton(0)) {
				inputDuration += Time.deltaTime;

				ballImage.enabled = true;
				ballImage.fillAmount = inputDuration/spawnHoldThreshold;

				if (inputDuration >= spawnHoldThreshold) {
					SpawnFirstBall();
					firstBallSpawned = true;
					ballImage.enabled = false;
				}
			} else {
				ballImage.fillAmount = 0;
				inputDuration = 0;
			}
		}
	}

	void SpawnFirstBall() {
		GameObject newBall = Instantiate(ball);
		newBall.transform.position = new Vector3(0, 6, 0);
		balls.Add(newBall);
	}

	public void SpawnBall() {
		if(gameOver) { return; }

		GameObject newBall = Instantiate(ball);
		newBall.transform.position = new Vector3(Random.Range(-2.3f, 2.3f), 9.5f, 0);

		balls.Add(newBall);
	}

	void DebugSpawn() {
		#if UNITY_EDITOR 
		if (Input.GetKeyDown(KeyCode.Space) || (Input.touches.Length == 2 && !debugBallSpawned) && !gameOver) {
			SpawnBall();
			debugBallSpawned = true;
		}

		if (Input.touches.Length == 0) {
			debugBallSpawned = false;
		}
		#endif
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
	}

	public void Restart() {
		gameOver = false;
		firstBallSpawned = false;
	}
}
