using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject ball;
	bool firstBallSpawned;
	bool debugBallSpawned;
	int numBalls;

	public static bool gameStarted;
	public static bool gameOver;

	List<GameObject> balls = new List<GameObject>();

	public UnityEngine.UI.Image ballImage;
	float inputDuration = 0;
	float spawnHoldThreshold = .5f;

	public GameObject burst;

	private static GameManager instance;
	private static bool instantiated;

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
			if (Input.touchCount > 0 || Input.GetMouseButton(0)) {
				inputDuration += Time.deltaTime;

				ballImage.gameObject.SetActive(true);
				ballImage.fillAmount = inputDuration/spawnHoldThreshold;

				if (inputDuration >= spawnHoldThreshold) {
					SpawnFirstBall();
					burst.GetComponent<BurstController>().Burst();

					firstBallSpawned = true;
					ballImage.gameObject.SetActive(false);
				}
			} else {
				ballImage.fillAmount = 0;
				inputDuration = 0;
			}
		}
	}

	void SpawnFirstBall() {
		gameStarted = true;

		GameObject newBall = Instantiate(ball);
		newBall.transform.position = new Vector3(0, 6, 0);

		newBall.GetComponent<Ball>().zDepth = -1;
		newBall.GetComponent<Ball>().SetDepth();
//		newBall.GetComponent<Ball>().SetColor();
		balls.Add(newBall);
		numBalls++;
	}

	public void AdjustBallDepth(GameObject latestBall) {
		for (int i = 0; i < balls.Count; i++) {
			if(balls[i].GetComponent<Ball>().canBeCaught) {
				balls[i].GetComponent<Ball>().zDepth++;

				if (balls[i] == latestBall) {
					balls[i].GetComponent<Ball>().zDepth = 0;
				}

				balls[i].GetComponent<Ball>().SetDepth();
			}
		}
	}

	public void LaunchBall() {
		if(gameOver) { return; }

		GameObject newBall = Instantiate(ball);
		newBall.transform.position = new Vector2(Random.Range(-2.3f, 2.3f), -2);
		newBall.GetComponent<Ball>().Launch();
		newBall.GetComponent<Ball>().zDepth = -1;
		newBall.GetComponent<Ball>().SetDepth();

		balls.Add(newBall);
		numBalls++;
	}

	void DebugSpawn() {
		#if UNITY_EDITOR 
		if (Input.GetKeyDown(KeyCode.Space) || (Input.touches.Length == 2 && !debugBallSpawned) && !gameOver) {
			LaunchBall();
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
		gameStarted = false;
		DestroyBalls();

		ScoreManager.GetInstance().HandleGameOver();
		AdManager.GetInstance().CheckAd();
	}

	public void Restart() {
		gameOver = false;
		firstBallSpawned = false;
		numBalls = 0;
	}
}
