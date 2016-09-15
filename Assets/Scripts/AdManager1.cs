using UnityEngine;
using System.Collections;
using admob;

public class AdManager1 : MonoBehaviour {

	private static AdManager1 instance;

	public static AdManager1 GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(AdManager1)) as AdManager1;
			if (!instance)
				Debug.Log("No AdManager!!");
		}
		return instance;
	}

	// Use this for initialization
	void Start () {
//        Admob.Instance().bannerEventHandler += onBannerEvent;
        Admob.Instance().interstitialEventHandler += onInterstitialEvent;
//        Admob.Instance().rewardedVideoEventHandler += onRewardedVideoEvent;


		Admob.Instance().setTesting(true);

		Admob ad = Admob.Instance();

		ad.initAdmob("ca-app-pub-xxxxxxxxxxxxxxxx/xxxxxxxxxx", "ca-app-pub-2916476108966190/9838939664");


//        if (ad.isInterstitialReady())
//        {
//            ad.showInterstitial();
//        }
//        else
//        {
//            ad.loadInterstitial();
//        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
//	void OnGUI(){
//		if (GUI.Button (new Rect (0, 0, 100, 60), "initadmob")) {
//            Admob ad = Admob.Instance();
//          
//			ad.initAdmob("ca-app-pub-xxxxxxxxxxxxxxxx/xxxxxxxxxx", "ca-app-pub-2916476108966190/9838939664");
//          
//            ad.setTesting(true);
//		}
//        if (GUI.Button(new Rect(120, 0, 100, 60), "showInterstitial"))
//        {
//            Admob ad = Admob.Instance();
//            if (ad.isInterstitialReady())
//            {
//                ad.showInterstitial();
//            }
//            else
//            {
//                ad.loadInterstitial();
//            }
//        }
//        if (GUI.Button(new Rect(240, 0, 100, 60), "showRewardVideo"))
//        {
//            Admob ad = Admob.Instance();
//            if (ad.isRewardedVideoReady())
//            {
//                ad.showRewardedVideo();
//            }
//            else
//            {
//                ad.loadRewardedVideo("ca-app-pub-xxxxxxxxxxxxxxxxxx/xxxxxxxxxx");
//            }
//        }
//        if (GUI.Button(new Rect(240, 100, 100, 60), "showbanner"))
//        {
//            Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);
//        }
//        if (GUI.Button(new Rect(240, 200, 100, 60), "showbannerABS"))
//        {
//            Admob.Instance().showBannerAbsolute(AdSize.Banner, 0, 30);
//        }
//        if (GUI.Button(new Rect(240, 300, 100, 60), "hidebanner"))
//        {
//            Admob.Instance().removeBanner();
//        }
//	}

    void onInterstitialEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
        if (eventName == AdmobEvent.onAdLoaded)
        {
            Admob.Instance().showInterstitial();
        }
    }
//    void onBannerEvent(string eventName, string msg)
//    {
//        Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
//    }
//    void onRewardedVideoEvent(string eventName, string msg)
//    {
//        Debug.Log("handler onRewardedVideoEvent---" + eventName + "   " + msg);
//    }

	public void CheckAd() {
		TryToShowAd();
	}

	void TryToShowAd() {
		if (Admob.Instance().isInterstitialReady()) {
			Admob.Instance().showInterstitial();
		} else {
			Admob.Instance().loadInterstitial();
		}
	}
}