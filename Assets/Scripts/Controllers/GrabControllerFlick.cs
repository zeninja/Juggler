using UnityEngine;
using System.Collections;

public class GrabControllerFlick : MonoBehaviour {

	Vector2 handPos;
	Vector2 dragStart, dragEnd;

	[System.NonSerialized]
	public Vector2 throwDirection;
	public float throwForceModifier = 6f;

	[System.NonSerialized]
	public bool holdingBall;

	GameObject ball;

	public Ring ring;

	[System.NonSerialized]
	public int id;
	Touch myTouch;

	void Update() {
		#if !UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
		ManageInput();
		#else 
		ManageTouchInput();
		#endif

		throwDirection = dragEnd - dragStart;
		FindTouch();
		handPos = FindHandPos();
	}

	void FindTouch() {
		#if UNITY_IOS
		for(int i = 0; i < Input.touchCount; i++) {
			if (Input.touches[i].fingerId == id) {
				myTouch = Input.GetTouch(i);
			}
		}
		#endif
	}

	Vector2 FindHandPos() {
		Vector2 handPos;

		#if !UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
		handPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		#else
		handPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position);
		#endif

		return handPos;
	}

	void ManageInput() {
		if (Input.GetMouseButton(0)) {
			if (!holdingBall) {
				dragStart = handPos;
				dragEnd = dragStart;

				transform.position = dragStart;
			} else {
				dragEnd = handPos;

				UpdateLine();
//				UpdateRotation();
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			// Throw the ball (if we're holding one) when we let go of the screen
			ThrowBall();
			HandleDeath();
		}
	}

	void ManageTouchInput() {
		if (myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary) {
			if (!holdingBall) {
				dragStart = handPos;
				dragEnd = dragStart;

				transform.position = dragStart;
			} else {
				dragEnd = handPos;

				UpdateLine();
//				UpdateRotation();
			}
		}

		if (myTouch.phase == TouchPhase.Ended) {
			// Throw the ball (if we're holding one) when we let go of the screen
			ThrowBall();
			HandleDeath();
		}
	}

	void UpdateLine() {
		ball.GetComponent<LineManager>().throwVector = throwDirection;
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Grab any balls we hit (as long as we're not already holding one)
		if (other.CompareTag("Ball") && !holdingBall) {

			GameObject newBall = other.gameObject;

			if(!newBall.GetComponent<Ball>().held && newBall.GetComponent<Ball>().canBeCaught) {
				ball = other.gameObject;
				GrabBall();
				ScoreManager.GetInstance().UpdateScore();
			}
		}
	}

	void GrabBall() {
		// Grab the ball; set its velocity and gravity to zero and child it so that it rotates
		holdingBall = true;
		ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		ball.GetComponent<Rigidbody2D>().gravityScale = 0;
		ball.GetComponent<Ball>().held = true;
		ball.GetComponent<Ball>().FlashColor();
		ball.GetComponent<Ball>().ActivateRing();
		ball.transform.parent = transform;

		GameManager.GetInstance().AdjustBallDepth(ball);

		ring.StartSpread();

	}

	void ThrowBall() {
		// Throw the ball
		if (ball != null) {
			holdingBall = false;
			ball.GetComponent<Rigidbody2D>().velocity = throwDirection * throwForceModifier;
			ball.GetComponent<Rigidbody2D>().gravityScale = .75f;
			ball.GetComponent<Ball>().held = false;
			ball.GetComponent<Ball>().ActivateRing();
			ball.transform.parent = null;

			ball = null;

			GameManager.GetInstance().AdjustBallDepth(ball);
		}
	}

	void OnDrawGizmos() {
		if(enabled) {
			Gizmos.DrawWireSphere(dragStart, .5f);

			Gizmos.color = Color.red;
			Gizmos.DrawLine(dragStart, dragEnd);

			Gizmos.DrawWireSphere(dragEnd, .5f);

		}
	}

	public void HandleDeath() {
		// public so that the hand manager can call this when the game ends if need be
		HandManager.RemoveHand(id);
		Destroy(gameObject);
	}

//	void OnApplicationExit() {
//		HandManager.RemoveHand(id);
//	}
}