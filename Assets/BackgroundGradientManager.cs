using UnityEngine;
using System.Collections;

public class BackgroundGradientManager : MonoBehaviour {

	public static Color topColor, botColor;
	public Material background;

	public Color debugTop, debugBot;

	// Use this for initialization
	void Awake () {
		SetBackgroundGradient();
	}
	
	// Update is called once per frame
	void Update () {
		debugTop = topColor;
		debugBot = botColor;
	}

	public void SetBackgroundGradient() {
		topColor = ColorSchemeManager.bgColor;
		botColor = topColor + new Color(.1f, .1f, .1f, 1);
	}
}
