using UnityEngine;
using System.Collections;

public class BounceControllerNormalized : MonoBehaviour {

	Vector2 dragStart, dragEnd;

	GameObject ball;

	public float throwForceModifier = 4f;
	Vector2 throwDirection;

	LineRenderer lineRenderer;
	public float lineLengthModifier = 2;

	public GameObject arrow;

	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update() {
		throwDirection = (dragStart - dragEnd).normalized * throwForceModifier;

		if (throwDirection == Vector2.zero) {
			throwDirection = Vector2.up * throwForceModifier;
		}


		if (Input.GetMouseButtonDown(0)) {
			dragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = dragStart;
		}

		if (Input.GetMouseButton(0)) {
			dragEnd = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

			UpdateLine();
			UpdateRotation();
		} else {
			// Turn off the line when the hand is just moving around
			lineRenderer.enabled = false;
			arrow.SetActive(false);
		}
	}

	void UpdateLine() {
		UpdateArrow();

		// Draw a dotted line to indicate the power and direction of the throw
		lineRenderer.enabled = true;
		Vector3[] linePositions = new Vector3[] { (Vector2)transform.position + throwDirection * lineLengthModifier, transform.position };
		lineRenderer.SetPositions(linePositions);

		float distance = Mathf.Abs(throwDirection.magnitude);
		lineRenderer.material.mainTextureScale = new Vector2(distance * 1.65f * lineLengthModifier, 1);
		// Multiply by 1.65 because for some unknown reason linerenderers have trouble with tiling textures properly?
		// Probably an issue with the texture??
	}

	void UpdateArrow() {
		// Draw an arrow at the end of the line
		arrow.SetActive(true);
		arrow.transform.position = (Vector2)transform.position + throwDirection * lineLengthModifier;

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
		if (other.CompareTag("Ball")) {
			ball = other.gameObject;
			ThrowBall();
			ScoreManager.GetInstance().UpdateScore();
		}
	}

	void ThrowBall() {
		// Throw the ball
		if(ball != null) {
			ball.GetComponent<Rigidbody2D>().velocity = throwDirection;
		}
//		Reset();
	}

	public void Reset() {
		transform.position = Vector2.zero;
		transform.rotation = Quaternion.identity;
		dragStart = transform.position;
		dragEnd = transform.position;
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