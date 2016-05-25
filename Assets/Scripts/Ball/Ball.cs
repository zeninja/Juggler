using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float squashSize = 2;
	Vector2 velocity, lastPos;

	public GameObject art;

	[System.NonSerialized]
	public bool canBeCaught;
	[System.NonSerialized]
	public bool held;

	public float launchVelocity = 10;

	Rigidbody2D rigidbody;

	GameObject explosion;

	// Use this for initialization
	void Awake () {
		explosion = transform.FindChild("Explosion").gameObject;

		rigidbody = GetComponent<Rigidbody2D>();
	}

	public void Launch() {
		rigidbody.velocity = Vector2.up * launchVelocity;
		canBeCaught = false;
	}

	public void SetColor(Color newColor) {
		art.GetComponent<MeshRenderer>().material.color = newColor;
	}

	void SetTransparent() {
		Color currentColor = art.GetComponent<MeshRenderer>().material.color;
		currentColor.a = canBeCaught ? .5f : 1f;
		art.GetComponent<MeshRenderer>().material.color = currentColor;
	}
	
	// Update is called once per frame
	void Update () {
//		if (!canBeCaught) {
//			if (rigidbody.velocity.y <= 0) {	
//				canBeCaught = true;
//			}
//			SetTransparent();
//		}

		velocity = Vector2.Lerp(velocity, (Vector2)transform.position - lastPos, Time.deltaTime * 3);
		lastPos = transform.position;

		SquashAndStretch();
	}

	void SquashAndStretch() {
		if((Vector3)velocity != Vector3.zero) {
			Vector3 diff = velocity.normalized;
	         
			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }

		art.transform.localScale = new Vector2(1 - velocity.magnitude * squashSize, 1 + velocity.magnitude * squashSize);
	}

	public void HandleDeath() {
		rigidbody.velocity = Vector2.zero;
		gameObject.ShakeScale(new Vector3(1, 1, 0) * .5f, .5f, 0);

		Invoke("Die", .5f);
	}

	void Die() {
		explosion.transform.parent = null;
		explosion.GetComponent<Explosion>().Trigger();
		Destroy(gameObject);
	}

	public void HandleGameOver() {
		// Explode
		HandleDeath();
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, (Vector2)transform.position + velocity * 5);
	}
}