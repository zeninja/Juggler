using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float explosionDuration = .3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Trigger() {
		GetComponent<SpriteRenderer>().enabled = true;
		gameObject.ScaleTo(Vector3.one, explosionDuration/2, 0);
		gameObject.ScaleTo(Vector3.zero, explosionDuration/2, explosionDuration/2);
		Invoke("Die", explosionDuration);
	}

	void Die() {
		Destroy(gameObject);
	}
}
