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
		throwDirection = dragEnd - dragStart;
		FindTouch();
		handPos = FindHandPos();

		#if !UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
		ManageInput();
		#else 
		ManageTouchInput();
		#endif
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
		Vector2 currentHandPos;

		#if !UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
		currentHandPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		#else
		currentHandPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position);
		#endif

		return currentHandPos;
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
			}
		}

		if (myTouch.phase == TouchPhase.Ended) {
			// Throw the ball (if we're holding one) when we let go of the screen
			ThrowBall();
			HandleDeath();
		}
	}

	void UpdateLine() {
		if (ball != null) {
			ball.GetComponent<LineManager>().throwVector = throwDirection;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Grab the ball "closest" to the player (has the highest z depth) as long as we're not already holding a ball
		if(!holdingBall) {
			int layerMask = 1 << LayerMask.NameToLayer("Balls");
			Collider2D[] overlappingBalls = Physics2D.OverlapCircleAll(transform.position, .6f, layerMask);

			int selectedBallIndex = -1;
			int highestZDepth = -1000;

			for(int i = 0; i < overlappingBalls.Length; i++) {
				if (overlappingBalls[i].GetComponent<Ball>().canBeCaught && !overlappingBalls[i].GetComponent<Ball>().held) {
					if (overlappingBalls[i].GetComponent<Ball>().zDepth > highestZDepth) {
						selectedBallIndex = i;
						highestZDepth = overlappingBalls[i].GetComponent<Ball>().zDepth;
					}
				}
			}

			if(selectedBallIndex >= 0) {
				GameObject newBall = overlappingBalls[selectedBallIndex].gameObject;
				
				ball = newBall;
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
		ball.GetComponent<LineManager>().Reset();
		ball.transform.parent = transform;

		GameManager.GetInstance().AdjustBallDepth(ball);

		ring.SetRotation(ball.transform.position);
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
}