using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float squashSize = 2;
	Vector2 velocity, lastPos;

	public GameObject art;

	GameObject explosion;

	public bool hover;
	public float defaultGravityScale;

	// Use this for initialization
	void Start () {
		explosion = transform.FindChild("Explosion").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
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
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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