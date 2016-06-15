using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public GameObject scoreDisplay, highScoreDisplay, newHighScoreText;
	public static int score;

	public float animationDuration = .3f;

	public float scorePulseAmount = .5f;

	int highScore;

	public int highScoreDuration = 3;

	public Color scoreColor;
	public Gradient highScoreGradient;

	string highScoreKey = "highScore";

	public GameCenter leaderboard;
	public ColorSchemeManager colorSchemeManager;

	private static ScoreManager instance;
	private static bool instantiated;

	bool scoreFlashing;
	public float flashDuration;
	public int numFlashes = 5;

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
		SetColors();
		StartCoroutine(RainbowHighScore());
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.C)) {
			// Quick debug reset high score
			PlayerPrefs.SetInt(highScoreKey, 0);
			highScore = 0;
			highScoreDisplay.GetComponent<Text>().text = highScore.ToString();
		}
	}

	public void SetColors() {
		scoreColor = scoreDisplay.GetComponent<ColorSchemeUtility>().currentColor;
	}

	public void UpdateScore() {
		if (GameManager.gameOver) { return; }

		score++;
		GetComponent<BallSpawner>().CheckScoreAndSpawnBall();
		StartCoroutine("AnimateScore");
	}

	IEnumerator AnimateScore() {
		if(!scoreFlashing) {
			scoreDisplay.GetComponent<Text>().text = score.ToString();
		}

		float currentPulseAmount = scorePulseAmount;

		if(score == 5 || score == 15 || score % 25 == 0) {
			StartCoroutine(FlashText(flashDuration));
		} else {
			if (!scoreFlashing) {
				StartCoroutine(GetComponent<NewTweens>().PulseScale(scoreDisplay, Vector3.one * currentPulseAmount, animationDuration));
			}
		}
		yield return new WaitForEndOfFrame();
	}

	public void HandleGameOver() {
		CheckAchievementProgress();
		StartCoroutine(SetHighScoreAndReset());
	}

	IEnumerator SetHighScoreAndReset() {
		yield return StartCoroutine(SetHighScore());
		yield return StartCoroutine(Reset());
	}

	void CheckAchievementProgress() {
		if(score >= 10) {
			GameCenter.SetComplete(GameCenter.get10Pts);
		}

		if(score >= 100) {
			GameCenter.SetComplete(GameCenter.get100Pts);
		}

		if(score >= 50 && HandManager.totalHandCount == 1) {
			GameCenter.SetComplete(GameCenter.get50Pts1Hand);
		}
	}
	
	IEnumerator SetHighScore() {
		// Update the saved high score and animate the new high score
		if (score > highScore) {
			highScore = score;
			PlayerPrefs.SetInt(highScoreKey, highScore);
			PlayerPrefs.Save();

			#if UNITY_STANDALONE_IOS
			leaderboard.SetHighScore(highScore);
			#endif

			StartCoroutine(RainbowNumber());
			StartCoroutine(RainbowText());
			StartCoroutine(UpdateHighScoreDisplay());
			yield return StartCoroutine(FlashText());
		}
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator Reset() {
		float startingScore = score;
		float resetDuration = .5f;

		yield return new WaitForSeconds(.5f);
		while (score > 0) {
			score--;
			scoreDisplay.GetComponent<Text>().text = score.ToString();

			yield return new WaitForSeconds(resetDuration/startingScore);
		}
		GameManager.GetInstance().Restart();
	}

	IEnumerator FlashText() {
		float timeFinished = Time.time + highScoreDuration;

		newHighScoreText.GetComponent<Text>().enabled = true;

		while (Time.time < timeFinished) {
			newHighScoreText.GetComponent<Text>().enabled = !newHighScoreText.GetComponent<Text>().enabled;
			yield return new WaitForSeconds(.25f);
		}
		newHighScoreText.GetComponent<Text>().enabled = false;
		scoreDisplay.GetComponent<Text>().enabled = true;
	}

	IEnumerator FlashText(float duration) {
		scoreFlashing = true;
		float timeFinished = Time.time + duration;

		scoreDisplay.GetComponent<Text>().enabled = true;

		while (Time.time < timeFinished) {
			scoreDisplay.GetComponent<Text>().enabled = !scoreDisplay.GetComponent<Text>().enabled;
			yield return new WaitForSeconds(duration/numFlashes);
		}
//		scoreDisplay.GetComponent<Text>().enabled = false;
		scoreDisplay.GetComponent<Text>().enabled = true;
		scoreFlashing = false;

		scoreDisplay.GetComponent<Text>().text = score.ToString();
	}

	IEnumerator RainbowNumber() {
		float t = 0;
		int numLoops = 3;
		int currentLoops = 0;

		while (t < highScoreDuration && currentLoops < numLoops) {
			t += Time.deltaTime;

			if (t > 1) {
				t %= 1;
				currentLoops++;
			}
//			t += (Time.deltaTime * 3)/highScoreDuration;
//			t += (Time.deltaTime * numLoops) % (highScoreDuration/numLoops);
			scoreDisplay.GetComponent<Text>().color = highScoreGradient.Evaluate(t);
			yield return new WaitForEndOfFrame();
		}

		scoreDisplay.GetComponent<Text>().color = scoreColor;
	}

	IEnumerator RainbowText() {
		float t = 0;
		int numLoops = 3;
		int currentLoops = 0;

		while (t < highScoreDuration && currentLoops < numLoops) {
			t += Time.deltaTime;
			t %= 1;

			if (t > 1) {
				t %= 1;
				currentLoops++;
			}

//			t += (Time.deltaTime * 3)/highScoreDuration;
//			t += (Time.deltaTime * numLoops) % (highScoreDuration/numLoops);
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