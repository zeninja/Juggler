using UnityEngine;
using System.Collections;

using admob;

public class AdManager : MonoBehaviour {

	#region Util
	private static AdManager instance;
	private static bool instantiated;

	public static AdManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(AdManager)) as AdManager;
			if (!instance)
				Debug.Log("No AdManager!!");
		}
		return instance;
	}

	public bool useAds;
	#endregion

	public static int adThreshold = 3;
	public static int currentPlays = 0;
	string numPlays = "numPlays";

	// Use this for initialization
	void Start () {
		#if UNITY_STANDALONE_IOS
		InitAds();
		Admob.Instance().setTesting(true);
		#endif
	}

	void InitAds() {
		Admob.Instance().initAdmob("admob banner id", "ca-app-pub-2916476108966190/9838939664"); //admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
		Admob.Instance().removeBanner();

		Admob.Instance().loadInterstitial();
		Admob.Instance().loadRewardedVideo("ca-app-pub-2916476108966190/1617668865");
	}

	void TryToShowAd()
	{
		#if UNITY_STANDALONE_IOS
		if (Admob.Instance().isInterstitialReady()) {
			Admob.Instance().showInterstitial();
	    } else {
	    	Admob.Instance().loadInterstitial();
	    }
		#endif
	}

	void TryToShowVideoAd() {
		#if UNITY_STANDALONE_IOS
		if (Admob.Instance().isRewardedVideoReady()) {
			Admob.Instance().showRewardedVideo();
	    } else {
			Admob.Instance().loadRewardedVideo("ca-app-pub-2916476108966190/1617668865");
	    }
		#endif
	}

	public void CheckAd() {
		currentPlays++;

//		if(currentPlays == 1) {
//			TryToShowAd();
//		}
//		if(currentPlays == 2) {
//			TryToShowVideoAd();
//		}

		if (currentPlays == adThreshold) {
			currentPlays = 0;
			TryToShowAd();
		}
	}
}
