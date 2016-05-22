using UnityEngine;
using System.Collections;

public class VivianHandController : MonoBehaviour {

	Vector3 targetPos;
	public float moveSpeed = 10;
//	Rigidbody2D rigidbody;
	public float throwForce;

	// Use this for initialization
	void Start () {
//		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		velocity = (Vector2)transform.position - lastPosition;
		lastPosition = transform.position;

		ManageInput();
	}

	void ManageInput() {
		if (Input.GetMouseButton(0)) {
			targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			targetPos.z = 0;
		} else {
			targetPos = new Vector3(transform.position.x, 0, 0);
		}
		
		transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Ball")) {
			ThrowBall(other.gameObject);
		}
	}

	Vector2 velocity;
	Vector2 lastPosition;

	void ThrowBall(GameObject target) {
//		target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		target.GetComponent<Rigidbody2D>().AddForce(velocity * throwForce, ForceMode2D.Impulse);
	}
}
