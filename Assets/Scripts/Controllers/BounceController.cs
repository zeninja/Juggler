using UnityEngine;
using System.Collections;

public class BounceController : MonoBehaviour {

	Vector2 startPos, endPos;

	public float moveSpeed = 10;
	public float throwForce = 10;

	Vector2 velocity;
	Vector2 lastPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ManageInput();
		Move();

		if (Input.GetKeyDown(KeyCode.I)) {
			enabled = !enabled;
		}
	}

	void ManageInput() {
		if (GameManager.gameOver) { return; }

		if (Input.GetMouseButtonDown(0)) {
			startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = startPos;
		}

		if (Input.GetMouseButton(0)) {
			endPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Quaternion rotation = Quaternion.LookRotation(endPos - startPos, transform.up);
    		transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
		}
	}

	void Move() {
		velocity = endPos - startPos;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Ball")) {
			// Rotate the ball when it's hit
			other.transform.GetChild(0).GetComponent<Rigidbody>().AddForceAtPosition(velocity * throwForce, other.transform.position - transform.position, ForceMode.Impulse);

			ScoreManager.GetInstance().UpdateScore();
			ShakeHand();
		}
	}

	void ShakeHand() {
		// Animate the hand

	}

	Vector2 newVelocity;

	void OnDrawGizmos() {
		if(enabled) {
			Gizmos.DrawWireSphere(startPos, .5f);

			Gizmos.color = Color.red;
			Gizmos.DrawLine(startPos, endPos);

//			Gizmos.color = Color.green;
//			Gizmos.DrawLine(startPos, startPos + newVelocity);
		}
	}
}