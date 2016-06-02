using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour {

	public void CheckScoreAndSpawnBall() {
		if (ScoreManager.score >= 25) {
			if (ScoreManager.score % 25 == 0) {
				SpawnBall();
			}
		} else {
			if (ScoreManager.score == 15) {
				SpawnBall();
			}

			if (ScoreManager.score == 5) {
				SpawnBall();
			}
		}
	}

	void SpawnBall() {
		GameManager.GetInstance().LaunchBall();
	}
}
