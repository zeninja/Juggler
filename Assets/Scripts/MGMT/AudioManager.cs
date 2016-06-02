using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioClip gameOver;
	public AudioClip ballExplosion;
	public AudioClip ballSquirm;
	public AudioClip newHighScore;
	public AudioClip grab;
	public AudioClip throwSound;
	public AudioClip bgMusic;

	private static AudioManager instance;
	private static bool instantiated;

	public static AudioManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
			if (!instance)
				Debug.Log("No AudioManager!!");
		}
		return instance;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayClip(AudioClip clip) {
		
	}
}
