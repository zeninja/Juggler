using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandController : MonoBehaviour {

	Vector2 mousePos;

	float maxRotDist = 2;		// The distance that the hand will be when it reaches the max rotation angle
	float maxRotAngle = 15;		// The most rotated the hand will ever be
	
	float maxThrowStrength = 8;
	float minThrowStrength = 1;
	public float throwStrengthMultiplier = 2;
	public float swipeThrowStrengthMultiplier = 1;
	public float dragThrowStrengthMultiplier = 1;

	List<GameObject> balls = new List<GameObject>();

	public bool useTouchInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		UpdatePos();
		UpdateRotation();
		ManageInput();

		HandleTouchInput();
	}

	void UpdatePos() {
		transform.position = new Vector2(mousePos.x, 0);
	}

	void UpdateRotation() {
		transform.rotation = Quaternion.Euler(0, 0, transform.position.x/maxRotDist * maxRotAngle);
	}

	void ManageInput() {
		if (Input.GetMouseButtonUp(0) && !useTouchInput) {
			Throw();
		}
	}

	void Throw() {
		float throwStrength = mousePos.y;
		throwStrength = Mathf.Min(mousePos.y, maxThrowStrength);
		throwStrength = Mathf.Max(mousePos.y, minThrowStrength);

		for (int i = 0; i < balls.Count; i++) {
			Debug.Log("Adding force");
			balls[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			balls[i].GetComponent<Rigidbody2D>().AddForce(transform.up * throwStrength * throwStrengthMultiplier, ForceMode2D.Impulse);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Ball")) {
			balls.Add(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (balls.Contains(other.gameObject)) {
			balls.Remove(other.gameObject);
		}
	}

	Vector2 dragStart, dragEnd;

	void HandleTouchInput() {
		if (Input.touchCount == 1) {
			Touch finger = Input.touches[0];

			if (finger.phase == TouchPhase.Began) {
				dragStart = Camera.main.ScreenToWorldPoint(finger.position);
			}

			if (finger.phase != TouchPhase.Ended) {
				dragEnd = Camera.main.ScreenToWorldPoint(finger.position);
			}

			if (finger.phase == TouchPhase.Ended) {
				ThrowBall();
			}
		}
	}

	void ThrowBall() {
//		float throwStrength = dragStart.y - dragEnd.y;

		for (int i = 0; i < balls.Count; i++) {
//			balls[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
//			balls[i].GetComponent<Rigidbody2D>().AddForce(transform.up * throwStrength * dragThrowStrengthMultiplier, ForceMode2D.Impulse);
		}
	}

//	Vector2 swipeStartPos;
//	Vector2 swipeEndPos;

	void OnDrawGizmos() {
//		Gizmos.color = Color.white;
//		Gizmos.DrawWireSphere(swipeStartPos, .25f);
//
//		Gizmos.color = Color.red;
//		Gizmos.DrawLine(swipeStartPos, swipeEndPos);

		Vector3 center = new Vector3(0, (dragEnd.y - dragStart.y)/2, 0) +  new Vector3(0, dragStart.y, 0);
		Vector3 size   = new Vector3(10, dragEnd.y - dragStart.y, 0);

		Gizmos.DrawWireCube(center, size);
	}


	#region BALL FORCE CODE
//				for (int i = 0; i < balls.Count; i++) {
//					Debug.Log("Adding force");
//					balls[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
//					balls[i].GetComponent<Rigidbody2D>().AddForce(transform.up * throwStrength * swipeThrowStrengthMultiplier, ForceMode2D.Impulse);
//				}
	#endregion
}
