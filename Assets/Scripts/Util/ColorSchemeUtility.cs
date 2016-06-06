using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorSchemeUtility : MonoBehaviour {

	public enum ColorSource { background, score, ball }
	public ColorSource myColorSource;

//	[System.NonSerialized]
	public Color currentColor;

	void Update() {
		
	}

	public void UpdateColor() {
		switch(myColorSource) {
			case ColorSource.background:
				currentColor = ColorSchemeManager.bgColor;
				break;
			case ColorSource.ball:
				currentColor = ColorSchemeManager.ballColor;
				break;
			case ColorSource.score:
				currentColor = ColorSchemeManager.scoreColor;
				break;
		}

		if(GetComponent<Text>() != null) {
			GetComponent<Text>().color = currentColor;
		}

		if(GetComponent<SpriteRenderer>() != null) {
			GetComponent<SpriteRenderer>().color = currentColor;
		}

		if (GetComponent<Image>() != null) {
			GetComponent<Image>().color = currentColor;
		}
	}
}
