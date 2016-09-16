using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public enum GameState { menu, gameOn, gameOver, showingAd };
	public GameState state = GameState.menu;

	public GameObject ball;
//	bool firstBallSpawned;
	bool debugBallSpawned;
	int numBalls;

	List<GameObject> balls = new List<GameObject>();

	public UnityEngine.UI.Image ballImage;
	float inputDuration = 0;
	float spawnHoldThreshold = .5f;

	public GameObject burst;

	public GameObject leaderboards, noAds, mute, tutorial;

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

	void Awake() {
		if(!instantiated) {
			instance = this;
			instantiated = true;
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		DebugSpawn();
//		SpawnBallOnInput();

		switch(state) {
			case GameState.menu:
				SpawnBallOnInput();
				break;
		}
	}

	public void SetState(GameState newState) {
		state = newState;

		switch(state) {
			case GameState.menu:
				inputDuration = 0;

				tutorial.SetActive(true);
				leaderboards.SetActive(true);
				noAds.SetActive(AdManager.showAds);
				mute.SetActive(true);
				break;
			case GameState.gameOn:
				tutorial.SetActive(false);
				leaderboards.SetActive(false);
				noAds.SetActive(false);
				mute.SetActive(false);
				break;
			case GameState.gameOver:
				StartCoroutine(EndGame());
				break;
			case GameState.showingAd:
				AdManager.GetInstance().CheckAd();
				break;
		}
	}

	void SpawnBallOnInput() {
		if (state != GameState.menu) { return; }

		if (Input.touchCount > 0 || Input.GetMouseButton(0)) {
			inputDuration += Time.deltaTime;

			ballImage.gameObject.SetActive(true);
			ballImage.fillAmount = inputDuration/spawnHoldThreshold;

			if (inputDuration >= spawnHoldThreshold) {
				SpawnFirstBall();
				burst.GetComponent<BurstController>().Burst();
				ballImage.gameObject.SetActive(false);
				inputDuration = 0;
			}
		} else {
			ballImage.fillAmount = 0;
			inputDuration = 0;
		}
	}

	void SpawnFirstBall() {
		if (state != GameState.menu) { return; }

		SetState(GameState.gameOn);
		Debug.Log("Spawning first ball");
		GameObject newBall = ObjectPool.instance.GetObjectForType("Ball", false);

		newBall.transform.position = new Vector3(0, 6, 0);
		newBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

		newBall.GetComponent<Ball>().zDepth = -1;
		newBall.GetComponent<Ball>().SetDepth();
		balls.Add(newBall);
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
		GameObject newBall = ObjectPool.instance.GetObjectForType("Ball", false);

		newBall.transform.position = new Vector2(Random.Range(-2.3f, 2.3f), -2);
		newBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

		newBall.GetComponent<Ball>().Launch();
		newBall.GetComponent<Ball>().zDepth = -1;
		newBall.GetComponent<Ball>().SetDepth();

		balls.Add(newBall);
	}

	void DebugSpawn() {
		#if UNITY_EDITOR 
		if (Input.GetKeyDown(KeyCode.Space) || (Input.touches.Length == 2 && !debugBallSpawned)) {
			LaunchBall();
			debugBallSpawned = true;
		}

		if (Input.touches.Length == 0) {
			debugBallSpawned = false;
		}
		#endif
	}

	IEnumerator EndGame() {
		yield return StartCoroutine(DestroyBalls());
		ScoreManager.GetInstance().HandleGameOver();
	}

	IEnumerator DestroyBalls() {
		for (int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<Ball>().HandleDeath();
		}
		balls.Clear();
		yield return .25f;
	}

	public void HandleGameOver() {
		if (state == GameState.gameOn) {
			SetState(GameState.gameOver);
		}
	}


	public GUIStyle hudStyle;

	void OnGUI() {
		GUI.Label(new Rect(40, Screen.height - 80, Screen.width/2, Screen.height/8), state.ToString(), hudStyle);
	}
}
