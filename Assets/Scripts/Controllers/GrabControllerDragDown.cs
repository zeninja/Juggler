﻿using UnityEngine;
using System.Collections;

public class GrabControllerDragDown : MonoBehaviour {

	Vector2 dragStart, dragEnd;
	Vector2 lineStart, lineEnd;

	bool holdingBall;

	GameObject ball;

	public float throwForceModifier = 6f;
	Vector2 throwDirection;

	LineRenderer lineRenderer;

	GameObject arrow;

	void Start() {
		arrow = transform.FindChild("Arrow").gameObject;
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update() {
		throwDirection = dragStart - dragEnd;

		if (Input.GetMouseButton(0)) {
			if (!holdingBall) {
				dragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				transform.position = new Vector2 (dragStart.x, 0);
			} else {
				dragEnd = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

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

	void UpdateLine() {
		UpdateArrow();

		// Draw a dotted line to indicate the power and direction of the throw
		lineRenderer.enabled = true;
		Vector3[] linePositions = new Vector3[] { (Vector2)transform.position + throwDirection * 3, transform.position };
		lineRenderer.SetPositions(linePositions);

		float distance = Mathf.Abs(throwDirection.magnitude);
		lineRenderer.material.mainTextureScale = new Vector2(distance * 1.65f * 3, 1);
		// Multiply by 1.65 because for some unknown reason linerenderers have trouble with tiling textures properly?
		// Probably an issue with the texture??
	}

	void UpdateArrow() {
		// Draw an arrow at the end of the line
		arrow.SetActive(true);
//		arrow.transform.position = endPos;
		arrow.transform.position = (Vector2)transform.position + throwDirection * 3;

		Vector3 diff = throwDirection;

		if(diff != Vector3.zero) {
	        diff.Normalize();

	        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
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
	}

	void ThrowBall() {
		// Throw the ball
		if(ball != null) {
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