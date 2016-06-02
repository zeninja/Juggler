using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Leaderboards : MonoBehaviour {

	public string leaderboardID;

    void Start () {
        // Authenticate and register a ProcessAuthentication callback
        // This call needs to be made before we can proceed to other calls in the Social API

        if(!Social.localUser.authenticated) {
	        Social.localUser.Authenticate (ProcessAuthentication);
        }
    }

    // This function gets called when Authenticate completes
    // Note that if the operation is successful, Social.localUser will contain data from the server. 
    void ProcessAuthentication (bool success) {
        if (success) {
            Debug.Log ("Authenticated, checking achievements");

            // Request loaded achievements, and register a callback for processing them
            Social.LoadAchievements (ProcessLoadedAchievements);
        }
        else
            Debug.Log ("Failed to authenticate");
    }

    // This function gets called when the LoadAchievement call completes
    void ProcessLoadedAchievements (IAchievement[] achievements) {
        if (achievements.Length == 0)
            Debug.Log ("Error: no achievements found");
        else
            Debug.Log ("Got " + achievements.Length + " achievements");
     
        // You can also call into the functions like this
        Social.ReportProgress ("Achievement01", 100.0, result => {
            if (result)
                Debug.Log ("Successfully reported achievement progress");
            else
                Debug.Log ("Failed to report achievement");
        });
    }

	void ReportScore (long score, string leaderboardID) {
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
	}

	public void ShowLeaderboard() {
		Social.ShowLeaderboardUI();
	}

	public void SetHighScore(int newScore) {
		ReportScore((long)newScore, leaderboardID);
	}

	void Update() {
		// TODO: UPDATE THIS TO BE A DIRECT CALL RATHER THAN UPDATE
		GetComponent<Image>().enabled = GameManager.gameOver || !GameManager.gameStarted;
		GetComponent<Button>().enabled = GameManager.gameOver || !GameManager.gameStarted;
	}
}
