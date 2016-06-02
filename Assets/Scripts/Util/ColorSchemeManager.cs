using UnityEngine;
using System.Collections;

public class ColorSchemeManager : MonoBehaviour {

	public ColorSchemeUtility[] colorSchemeUtilities;
	public ColorScheme[] colorSchemes;

	public static Color bgColor;
	public static Color ballColor;
	public static Color scoreColor;

	[System.Serializable]
	public class ColorScheme {
		public string name;
		public Color[] colors;
	}

	// Shake detection code
	float accelerometerUpdateInterval = 1.0f / 60.0f;
	// The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
	float lowPassKernelWidthInSeconds = 1.0f;
	// This next parameter is initialized to 2.0 per Apple's recommendation, or at least according to Brady! ;)
	float shakeDetectionThreshold = 4.0f;

	private float lowPassFilterFactor;
	private Vector3 lowPassValue = Vector3.zero;
	private Vector3 acceleration;
	private Vector3 deltaAcceleration;

	float nextShakeTime;
	float shakeDelay = 1;

	public ColorScheme newScheme;

	// Use this for initialization
	void Start () {
		lowPassFilterFactor =  accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
		shakeDetectionThreshold *= shakeDetectionThreshold;
	    lowPassValue = Input.acceleration;

	    SetDefaultColors();
	}
	
	// Update is called once per frame
	void Update () {
		acceleration = Input.acceleration;
	    lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
	    deltaAcceleration = acceleration - lowPassValue;

	    if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold && Time.time > nextShakeTime)
	    {
	        // Perform your "shaking actions" here, with suitable guards in the if check above, if necessary to not, to not fire again if they're already being performed.
//	        Debug.Log("Shake event detected at time "+Time.time);	
	        SetNewColorScheme();
	        nextShakeTime = Time.time + shakeDelay;
	    }
	}

	public void SetNewColorScheme() {
		if (!GameManager.gameOver && !GameManager.gameStarted) {
			int randomSchemeIndex = Random.Range(0, colorSchemes.Length);
			newScheme = colorSchemes[randomSchemeIndex];

			if(newScheme.colors.Length < 3) {
				Debug.LogError("Color scheme " + newScheme.name + " does not have enough colors!");
				return;
			}

			int background = 0, ball = 0, score = 0;

			background = Random.Range(0, newScheme.colors.Length);

			ball = Random.Range(0, newScheme.colors.Length);
			while (ball == background) {
				ball = Random.Range(0, newScheme.colors.Length);
			}

			score = Random.Range(0, newScheme.colors.Length);
			while(score == ball || score == background) {
				score = Random.Range(0, newScheme.colors.Length);
			}

			bgColor    = newScheme.colors[background];
			ballColor  = newScheme.colors[ball];
			scoreColor = newScheme.colors[score];

			for(int i = 0; i < colorSchemeUtilities.Length; i++) {
				colorSchemeUtilities[i].UpdateColor();
			}

			ScoreManager.GetInstance().SetColors();
		}
	}

	void SetDefaultColors() {
		newScheme = colorSchemes[0];
		bgColor    = newScheme.colors[0];
		ballColor  = newScheme.colors[1];
		scoreColor = newScheme.colors[2];

		for(int i = 0; i < colorSchemeUtilities.Length; i++) {
			colorSchemeUtilities[i].UpdateColor();
		}
	}
}
