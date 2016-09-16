using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour {

	public Sprite mutedSprite;
	public Sprite notMutedSprite;

	public void Mute() {
		AudioManager.muted = !AudioManager.muted;
		GetComponent<Image>().sprite = AudioManager.muted ? mutedSprite : notMutedSprite;


		LeanTween.scale(gameObject, Vector3.one, .25f).setEase(LeanTweenType.easeSpring);
	}
}
