using UnityEngine;
using System.Collections;

public class SlingshotController : MonoBehaviour {

	Vector2 startPos;

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
		if (Input.GetMouseButtonDown(0)) {
			startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		if (Input.GetMouseButton(0)) {
			transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Quaternion rotation = Quaternion.LookRotation(startPos - (Vector2)transform.position, transform.up);
    		transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
		}
	}

	void Move() {
		if (!Input.GetMouseButton(0)) {
			transform.position = Vector3.Lerp(transform.position, startPos, moveSpeed * Time.deltaTime);
		}

		velocity = (Vector2)transform.position - lastPos;
		lastPos = transform.position;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Ball")) {
			ThrowBall(other.gameObject);
		}
	}

	void ThrowBall(GameObject target) {
		target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		target.GetComponent<Rigidbody2D>().AddForce(velocity * throwForce, ForceMode2D.Impulse);
	}

	void OnDrawGizmos() {
		if(enabled) {
			Gizmos.DrawWireSphere(startPos, .5f);
		}
	}
}
