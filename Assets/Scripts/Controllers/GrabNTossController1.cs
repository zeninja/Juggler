using UnityEngine;
using System.Collections;

public class GrabNTossController1 : MonoBehaviour {

	Vector2 startPos, endPos;

	float throwForce;
	public float throwForceModifier = 10;

	GameObject ball;
	bool holdingBall = false;
	Vector2 throwDirection;

	Vector3[] linePositions;
	LineRenderer lineRenderer;
	public GameObject arrow;

	Quaternion lookRotation;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		ManageInput();

		if (Input.GetKeyDown(KeyCode.I)) {
			enabled = !enabled;
		}
	}

	void ManageInput() {
		if(GameManager.GetInstance().state == GameManager.GameState.gameOver) { return; }

		if (Input.GetMouseButton(0)) {
			// Rotate the hand to look towards the aim direction
			Quaternion lookRotation = Quaternion.LookRotation(endPos - startPos, transform.up);
			transform.rotation = new Quaternion(0, 0, lookRotation.z, lookRotation.w);

//			Vector3 diff = endPos - startPos;
//	        diff.Normalize();
//	 
//	        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
//	        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

			endPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if (!holdingBall) {
				// If we're not holding a ball, we can move
				transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				startPos = transform.position;
			} else {
				// Once we grab a ball, we stop moving, start updating the end position,
				// and drawing the line to show direction  + power


				UpdateLine();
			}


		} else {
			lineRenderer.enabled = false;
		}

		if (Input.GetMouseButtonUp(0) && holdingBall) {
			throwDirection = endPos - startPos;
			ThrowBall();
		}
	}

	void UpdateLine() {
		arrow.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		arrow.transform.rotation = new Quaternion(0, 0, lookRotation.z, lookRotation.w);

		lineRenderer.enabled = true;
		int distance = Mathf.Abs(Mathf.CeilToInt((endPos - startPos).magnitude));
		lineRenderer.SetVertexCount(distance);

		linePositions = new Vector3[distance];
		for (int i = 0; i < distance; i++) {
			linePositions[i] = startPos + (endPos - startPos) * i/distance;
		}

		lineRenderer.SetPositions(linePositions);
		lineRenderer.material.mainTextureScale = new Vector2(distance, 1);
	}

//	void UpdateLine() {
//		lineRenderer.enabled = true;
//		linePositions = new Vector3[2] { startPos, endPos };
//		lineRenderer.SetPositions(linePositions);
//		lineRenderer.material.mainTextureScale = new Vector2((int)Vector2.Distance(linePositions[0], linePositions[1]) * 5, 1);
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Ball")) {
			ball = other.gameObject;
			GrabBall();
			ScoreManager.GetInstance().UpdateScore();
			ShakeHand();
		}
	}

	void ShakeHand() {
		// Animate the hand

	}

	void GrabBall() {
		ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		ball.GetComponent<Rigidbody2D>().gravityScale = 0;
//		ball.transform.position = transform.position;
		ball.transform.parent = transform;
		holdingBall = true;
	}

	void ThrowBall() {
		ball.GetComponent<Rigidbody2D>().gravityScale = .5f;
		ball.GetComponent<Rigidbody2D>().AddForce(throwDirection * throwForceModifier, ForceMode2D.Impulse);
		ball.transform.parent = null;
		holdingBall = false;
	}

	Vector2 ballDirection;
	Vector2 newVelocity;

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
}