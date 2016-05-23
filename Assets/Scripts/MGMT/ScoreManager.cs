using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public GameObject oldScore, newScore, highScoreDisplay, newHighScoreText;
	public GameObject nextBall;
	int score;

	public float animationDuration = .3f;
	Vector2 scorePos, highPos, lowPos;

	private static ScoreManager instance;
	private static bool instantiated;

	int spawnAmount = 5;

	int highScore;

	public int highScoreDuration = 3;

	public Color scoreColor;
	public Color highScoreColor;
	public Color targetColor;

	public Gradient highScoreGradient;

	string highScoreKey = "highScore";

	public static ScoreManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(ScoreManager)) as ScoreManager;
			if (!instance)
				Debug.Log("No ScoreManager!!");
		}
		return instance;
	}

	void Awake() {
		if (PlayerPrefs.HasKey(highScoreKey)) {
			highScore = PlayerPrefs.GetInt(highScoreKey);
			highScoreDisplay.GetComponent<Text>().text = highScore.ToString();
		} else {
			PlayerPrefs.SetInt(highScoreKey, 0);
		}
	}

	// Use this for initialization
	void Start () {
		scorePos = oldScore.transform.position;
		highPos  = newScore.transform.position;
		lowPos   = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, -300));

		SetColors();
//		StartCoroutine(Rainbow(highScoreDisplay, 10, true));
		StartCoroutine(RainbowHighScore());
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.C)) {
			PlayerPrefs.SetInt(highScoreKey, 0);
			highScore = 0;
			highScoreDisplay.GetComponent<Text>().text = highScore.ToString();

		}
	}

	void SetColors() {
		oldScore.GetComponent<Text>().color = scoreColor;
		newScore.GetComponent<Text>().color = scoreColor;
		nextBall.GetComponent<Text>().color = targetColor;
		highScoreDisplay.GetComponent<Text>().color = targetColor;
	}

	public void UpdateScore() {
		if (GameManager.gameOver) { return; }

		score++;
		CheckBallSpawn();
		StartCoroutine("AnimateScore");
	}

	void CheckBallSpawn() {
		if (score % spawnAmount == 0) {
			spawnAmount += spawnAmount + 3;
			GameManager.GetInstance().SpawnBall();
		}

		nextBall.GetComponent<Text>().text = "Next ball in: " + (spawnAmount - score).ToString();
	}

	IEnumerator AnimateScore() {
		newScore.GetComponent<Text>().text = score.ToString();

		StartCoroutine(GetComponent<NewTweens>().MoveToEaseOutBack(oldScore, scorePos, lowPos, animationDuration));
		StartCoroutine(GetComponent<NewTweens>().MoveToEaseOutBack(newScore, highPos, scorePos, animationDuration));
		yield return new WaitForSeconds(animationDuration + Time.deltaTime);
		oldScore.GetComponent<Text>().text = score.ToString();


		oldScore.transform.position = scorePos;
		newScore.transform.position = highPos;
	}

	public void HandleGameOver() {
		StartCoroutine(SetHighScoreAndReset());
	}

	IEnumerator SetHighScoreAndReset() {
		yield return StartCoroutine(SetHighScore());
		yield return StartCoroutine(Reset());
	}

	IEnumerator SetHighScore() {
		// Update the saved high score and animate the new high score
		if (score > highScore) {
			highScore = score;
			PlayerPrefs.SetInt(highScoreKey, highScore);
			PlayerPrefs.Save();

			StartCoroutine(RainbowNumber());
			StartCoroutine(RainbowText());
			StartCoroutine(UpdateHighScoreDisplay());
			yield return StartCoroutine(FlashText());
		}
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator Reset() {
		spawnAmount = 5;

		float startingScore = score;
		float resetDuration = .5f;

		yield return new WaitForSeconds(.5f);
		while (score > 0) {
			score--;
			oldScore.GetComponent<Text>().text = score.ToString();

			yield return new WaitForSeconds(resetDuration/startingScore);
		}
		nextBall.GetComponent<Text>().text = "Next ball in: 5";
		GameManager.GetInstance().Restart();
	}

	IEnumerator FlashText() {
		float timeFinished = Time.time + highScoreDuration;

		newHighScoreText.GetComponent<Text>().enabled = true;

		while (Time.time < timeFinished) {
			newHighScoreText.GetComponent<Text>().enabled = !newHighScoreText.GetComponent<Text>().enabled;
			oldScore.GetComponent<Text>().enabled = !oldScore.GetComponent<Text>().enabled;
			yield return new WaitForSeconds(.25f);
		}
		newHighScoreText.GetComponent<Text>().enabled = false;
		oldScore.GetComponent<Text>().enabled = true;
	}

	IEnumerator RainbowNumber() {
		float t = 0;

		while (t < 1) {
			t += Time.deltaTime/highScoreDuration;
			oldScore.GetComponent<Text>().color = highScoreGradient.Evaluate(t);
			yield return new WaitForEndOfFrame();
		}

		oldScore.GetComponent<Text>().color = scoreColor;
	}

	IEnumerator RainbowText() {
		float t = 0;

		while (t < 1) {
			t += Time.deltaTime/highScoreDuration;
			newHighScoreText.GetComponent<Text>().color = highScoreGradient.Evaluate(t) ;

			yield return new WaitForEndOfFrame();
		}

		newHighScoreText.GetComponent<Text>().material.color = scoreColor;
	}

	IEnumerator RainbowHighScore() {
		float t = 0;
		float rainbowDuration = 10;

		while (t < 1) {
			t += Time.deltaTime/rainbowDuration;
			highScoreDisplay.GetComponent<Text>().color = highScoreGradient.Evaluate(t);

			yield return new WaitForEndOfFrame();

			if (t > 1) {
				t = 0;
			}
		}
	}

	IEnumerator UpdateHighScoreDisplay() {
		Vector2 highScorePos   = highScoreDisplay.transform.position;
		Vector2 highScoreScale = highScoreDisplay.transform.localScale;

		highScoreDisplay.MoveTo(highScorePos * 2, .5f, 0, EaseType.easeInBack);
		highScoreDisplay.ScaleTo(Vector2.one * .25f, .5f, 0, EaseType.easeInBack);

		yield return new WaitForSeconds(.5f + Time.deltaTime);

		highScoreDisplay.GetComponent<Text>().text = score.ToString();
		highScoreDisplay.MoveTo(highScorePos, .5f, 0, EaseType.easeOutBack);
		highScoreDisplay.ScaleTo(highScoreScale, .5f, 0, EaseType.easeOutBack);
	}
}