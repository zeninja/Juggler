using UnityEngine;
using System.Collections;
using admob;

public class AdManager : MonoBehaviour {

	#region Util
	private static AdManager instance;

	public static AdManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(AdManager)) as AdManager;
			if (!instance)
				Debug.Log("No AdManager!!");
		}
		return instance;
	}

	public static bool showAds = true;
	#endregion

	public static int adThreshold = 3;
	public static int currentPlays = 0;
	string numPlays = "numPlays";
	string hasMadePurchase = "hasMadePurchase";

	public bool forceAdsOff;

	void Awake() {
		Admob.Instance().interstitialEventHandler += OnInterstitialEvent;
	}

	// Use this for initialization
	void Start () {
//		#if !UNITY_EDITOR
		if (PlayerPrefs.HasKey(hasMadePurchase)) {
			showAds = PlayerPrefs.GetInt(hasMadePurchase) == 0;
		} else {
			PlayerPrefs.SetInt(hasMadePurchase, 0);
			showAds = true;
		}

		if (forceAdsOff) {
			showAds = false;
		}

		if (PlayerPrefs.HasKey(numPlays)) {
			currentPlays = PlayerPrefs.GetInt(numPlays);
		} else {
			PlayerPrefs.SetInt(numPlays, 0);
		}

		if (showAds) {
			Admob.Instance().setTesting(true); 
			InitAds();
		}

//		Debug.Log("\nSHOW ADS VALUE: " + showAds + "\n");
//		Debug.Log("\nHAS MADE PURCHASE: " + PlayerPrefs.GetInt(hasMadePurchase));
//		#endif

	}

	void InitAds() {
		Admob.Instance().initAdmob("admob banner id", "ca-app-pub-2916476108966190/9838939664"); //admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
		Admob.Instance().removeBanner();

		Admob.Instance().loadInterstitial();
		Admob.Instance().loadRewardedVideo("ca-app-pub-2916476108966190/1617668865");
	}

	void TryToShowAd()
	{
		if(forceAdsOff) 
		{ 
			GameManager.GetInstance().SetState(GameManager.GameState.menu);
			return;
		}

		#if !UNITY_EDITOR
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			GameManager.GetInstance().SetState(GameManager.GameState.menu);
		} else {

			if (Admob.Instance().isInterstitialReady()) {
				Admob.Instance().showInterstitial();
		    } else {
		    	Admob.Instance().loadInterstitial();
		    }
			currentPlays = 0;
		}

		#endif
	}

	// not being used right now
//	void TryToShowVideoAd() {
//		if(debug) { return; }
//
//		#if !UNITY_EDITOR
//		if (showAds) {
//			if (Admob.Instance().isRewardedVideoReady()) {
//				Admob.Instance().showRewardedVideo();
//		    } else {
//				Admob.Instance().loadRewardedVideo("ca-app-pub-2916476108966190/1617668865");
//		    }
//	    }
//		#endif
//	}

	public void CheckAd() {
		if(showAds) {

			currentPlays++;

			if(Application.internetReachability != NetworkReachability.NotReachable) {

				if (currentPlays >= adThreshold) {
					TryToShowAd();
				} else {
					GameManager.GetInstance().SetState(GameManager.GameState.menu);
					Debug.Log("NO AD THRESHOLD called Setting state at: " + Time.time);

					PlayerPrefs.SetInt(numPlays, currentPlays);
				}
			} else {
				GameManager.GetInstance().SetState(GameManager.GameState.menu);
				Debug.Log("NO INTERNET called Setting state at: " + Time.time);

				PlayerPrefs.SetInt(numPlays, currentPlays);
			}

		} else {
			GameManager.GetInstance().SetState(GameManager.GameState.menu);
			Debug.Log("SHOW ADS FALSE called Setting state at: " + Time.time);
		}
	}

	void OnInterstitialEvent(string eventName, string msg) {
		switch(eventName) {
			case "onAdOpened":
				currentPlays = 0;
				PlayerPrefs.SetInt(numPlays, currentPlays);
				Debug.Log("OnAdOpened called Setting state at: " + Time.time);

				break;

			case "onAdClosed":
				GameManager.GetInstance().SetState(GameManager.GameState.menu);
				Debug.Log("OnAdClosed called Setting state at: " + Time.time);
	
				break;

			case "onAdFailedToLoad":
				Admob.Instance().loadInterstitial();
				GameManager.GetInstance().SetState(GameManager.GameState.menu);
				Debug.Log("OnAdFailedToLoad called Setting state at: " + Time.time);
				break;
		}
	}

	public static void HandlePurchaseMade() {
		showAds = false;
		PlayerPrefs.SetInt(AdManager.GetInstance().hasMadePurchase, 1);
	}
}