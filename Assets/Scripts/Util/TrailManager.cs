using UnityEngine;
using System.Collections;

public class TrailManager : MonoBehaviour {

	public Color[] colors;
	int currentColorIndex = 0;

	GameObject currentTrail;
	public GameObject trailPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HandleGrab() {
		if(currentTrail != null) {
			currentTrail.transform.parent = null;
			currentTrail.GetComponent<TrailRenderer>().material.renderQueue = GetComponent<Ball>().zDepth;
		}

		currentColorIndex++;

		if (currentColorIndex > colors.Length - 1) {
			currentColorIndex = 0;
		}

		currentTrail = Instantiate(trailPrefab) as GameObject;
		currentTrail.transform.position = transform.position;
		currentTrail.GetComponent<TrailRenderer>().material.color = colors[currentColorIndex];

		currentTrail.transform.parent = transform;
	}
}
