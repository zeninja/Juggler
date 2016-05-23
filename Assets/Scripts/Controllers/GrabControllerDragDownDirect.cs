using UnityEngine;
using System.Collections;

public class GrabControllerDragDownDirect : MonoBehaviour {

	Vector2 handPos;
	Vector2 dragStart, dragEnd;

	[System.NonSerialized]
	public Vector2 throwDirection;
	public float throwForceModifier = 6f;
	public float lineLengthModifier = 2;

	[System.NonSerialized]
	public bool holdingBall;

	GameObject ball;

	LineRenderer lineRenderer;
	GameObject arrow;

//	[System.NonSerialized]
	public int id;
	Touch myTouch;

	void Start() {
		arrow = transform.FindChild("Arrow").gameObject;
		lineRenderer = GetComponent<LineRenderer>();
	}

	void OnGUI() {
		if (myTouch.fingerId != null) {
			GUI.Label(new Rect(transform.position, Vector2.one * 25f), myTouch.fingerId.ToString());
		}
	}

	void Update() {
		throwDirection = dragStart - dragEnd;
		FindTouch();
		handPos = FindHandPos();

		#if !UNITY_IPHONE || UNITY_EDITOR || UNITY_STANDALONE_OSX
		ManageInput();
		#else 
		ManageTouchInput();
		#endif
	}

	void FindTouch() {
		#if UNITY_IPHONE
		for(int i = 0; i < Input.touchCount; i++) {
			if (Input.GetTouch(i).fingerId == id) {
				myTouch = Input.GetTouch(i);
			}
		}
		#endif
	}

	Vector2 FindHandPos() {
		Vector2 handPos;

		#if !UNITY_IPHONE || UNITY_EDITOR || UNITY_STANDALONE_OSX
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
				transform.position = dragStart;
			} else {
				dragEnd = handPos;

				UpdateLine();
				UpdateRotation();
			}
		} else {
			// Turn off the line when the hand is just moving around
			lineRenderer.enabled = false;
			arrow.SetActive(false);
		}

		if (Input.GetMouseButtonUp(0)) {
			// Throw the ball (if we're holding one) when we let go of the screen
			ThrowBall();
		}
	}

	void ManageTouchInput() {
		if (myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary) {
			if (!holdingBall) {
				dragStart = handPos;
				transform.position = dragStart;
			} else {
				dragEnd = handPos;

				UpdateLine();
				UpdateRotation();
			}
		} else {
			// Turn off the line when the hand is just moving around
			lineRenderer.enabled = false;
			arrow.SetActive(false);
		}

		if (myTouch.phase == TouchPhase.Ended) {
			// Throw the ball (if we're holding one) when we let go of the screen
			ThrowBall();
			HandManager.RemoveHand(id);
		}
	}

	void UpdateLine() {
		GetComponent<LineManager>().dragDistance = Mathf.Abs(throwDirection.magnitude);
	}

	void UpdateRotation() {
		// Rotate the hand to look towards the aim direction
		Vector3 diff = throwDirection;

		if(diff != Vector3.zero) {
	        diff.Normalize();
	        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Grab any balls we hit (as long as we're not already holding one)
		if (other.CompareTag("Ball") && !holdingBall) {
			ball = other.gameObject;
			GrabBall();
			ScoreManager.GetInstance().UpdateScore();
		}
	}

	void GrabBall() {
		// Grab the ball; set its velocity and gravity to zero and child it so that it rotates
		holdingBall = true;
		ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		ball.GetComponent<Rigidbody2D>().gravityScale = 0;
		ball.transform.parent = transform;

//		transform.FindChild("Ring").GetComponent<ParticleSystem>().Play();
	}

	void ThrowBall() {
		// Throw the ball
		if (ball != null) {
			holdingBall = false;
			ball.GetComponent<Rigidbody2D>().velocity = throwDirection * throwForceModifier;
			ball.GetComponent<Rigidbody2D>().gravityScale = .75f;
			ball.transform.parent = null;

			ball = null;
		}
	}

	public void Reset() {
		ball = null;
		holdingBall = false;
		transform.position = Vector2.zero;
		transform.rotation = Quaternion.identity;
	}

	void OnDrawGizmos() {
		if(enabled) {
			Gizmos.DrawWireSphere(dragStart, .5f);

			Gizmos.color = Color.red;
			Gizmos.DrawLine(dragStart, dragEnd);

			Gizmos.DrawWireSphere(dragEnd, .5f);

//			Gizmos.color = Color.green;
//			Gizmos.DrawLine(startPos, startPos + newVelocity);
		}
	}
}