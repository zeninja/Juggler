using UnityEngine;
using System.Collections;

public class GrabControllerSlide : MonoBehaviour {

	Vector2 startPos, endPos;
	bool holdingBall;

	GameObject ball;

	public float throwForceModifier = 2f;
	Vector2 throwDirection;

	LineRenderer lineRenderer;

	public GameObject arrow;

	void Start() {
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Update() {
		throwDirection = endPos - startPos;
		throwDirection = ConvertVector2ToAbsoluteValue(endPos);

		if (Input.GetMouseButton(0)) {
			if (!holdingBall) {
				transform.position = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, 0);
			} else {
				endPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				endPos = ConvertVector2ToAbsoluteValue(endPos);
				startPos = transform.position;

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
		Vector3[] linePositions = new Vector3[] { endPos, startPos };
		lineRenderer.SetPositions(linePositions);

		float distance = Mathf.Abs((endPos - startPos).magnitude);
		lineRenderer.material.mainTextureScale = new Vector2(distance * 1.65f, 1);
		// Multiply by 1.65 because for some unknown reason linerenderers have trouble with tiling textures properly?
		// Probably an issue with the texture??
	}

	void UpdateArrow() {
		// Draw an arrow at the end of the line
		arrow.SetActive(true);
		arrow.transform.position = ConvertVector2ToAbsoluteValue(endPos);
//		Vector3 diff = endPos - startPos;

		arrow.transform.rotation = Rotation2D();

//		if(diff != Vector3.zero) {
//	        diff.Normalize();
//
//	        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
//
//			if (diff.y < 0) {
//				rot_z += 180;
//			}
//
//	        arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
//        }
	}

	void UpdateRotation() {
		// Rotate the hand to look towards the aim direction
		transform.rotation = Rotation2D();
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
			ball.GetComponent<Rigidbody2D>().gravityScale = .5f;
			ball.transform.parent = null;

			ball = null;
		}
	}

	void Reset() {
		ball = null;
		holdingBall = false;
		transform.position = Vector2.zero;
		transform.rotation = Quaternion.identity;
	}

	void OnDrawGizmos() {
		if(enabled) {
			Gizmos.DrawWireSphere(startPos, .5f);

			Gizmos.color = Color.red;
			Gizmos.DrawLine(startPos, endPos);

			Gizmos.DrawWireSphere(endPos, .5f);

//			Gizmos.color = Color.green;
//			Gizmos.DrawLine(startPos, startPos + newVelocity);
		}
	}

	Vector2 ConvertVector2ToAbsoluteValue(Vector2 vectorIn) {
		Vector2 vectorOut = Vector2.zero;

		if (vectorIn.y < 0) {
			return new Vector2(-vectorIn.x, Mathf.Abs(vectorIn.y));
		}
		return vectorOut;	 
	}

	Quaternion Rotation2D() {
		Vector3 diff = endPos - startPos;
		Quaternion rotation = Quaternion.identity;

		if(diff != Vector3.zero) {
	        diff.Normalize();
	        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

			if (diff.y < 0) { rot_z += 180; }

			rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }

        return rotation;
	}
}