using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float squashSize = 2;
	public Vector2 velocity, lastPos;

	public Vector2 bounds;
	public float yBound;

	public GameObject art;
//	public GameObject sphere;			// TEMP??	
	public SpriteRenderer sprite, background, ring, ringBG;

	[System.NonSerialized]
	public bool canBeCaught;
	[System.NonSerialized]
	public bool held;

	public float launchVelocity = 10;

	Rigidbody2D rb;

	GameObject explosion;

	OffscreenChevron chevron;

	HSBColor hsbColor;
	public Color flashColor;
	public Color originalColor;

	public float flashDuration = .15f;
	public int zDepth;

	void Awake() {
		explosion = transform.FindChild("Explosion").gameObject;
		chevron = transform.FindChild("Chevron").gameObject.GetComponent<OffscreenChevron>();
		chevron.ball = gameObject;

		rb = GetComponent<Rigidbody2D>();

	}

	void Start() {
		SetColor();
	}

	void OnEnable() {
		SetColor();
		lastPos = transform.position;
	}

	public void Launch() {
		lastPos = transform.position;
		rb.velocity = Vector2.up * launchVelocity;
		canBeCaught = false;
	}

	void SetColor() {
		sprite.GetComponent<ColorSchemeUtility>().UpdateColor();
		background.GetComponent<ColorSchemeUtility>().UpdateColor();

		ring.GetComponent<ColorSchemeUtility>().UpdateColor();
		ringBG.GetComponent<ColorSchemeUtility>().UpdateColor();

		originalColor = sprite.color;
		chevron.SetColors();

		hsbColor = HSBColor.FromColor(sprite.color);
	}

	void SetDarkerColor() {
		float darkenAmount = .7f;

		HSBColor tempColor = hsbColor;
		tempColor.b = hsbColor.b * (1 -  darkenAmount * (rb.velocity.y/launchVelocity));
		Color convertedColor = HSBColor.ToColor(tempColor);

		sprite.color = convertedColor;
	}

	public void SetDepth() {
		sprite.sortingOrder = zDepth * 4;
		background.sortingOrder = zDepth * 4 - 1;
		ring.sortingOrder = zDepth * 4 - 2;
		ringBG.sortingOrder = zDepth * 4 - 3;
	}

	public void ActivateRing() {
		ring.enabled = held;
		ringBG.enabled = held;
	}

	Vector2 lastVelocity;
	
	// Update is called once per frame
	void Update () {
		velocity = Vector2.Lerp(velocity, (Vector2)transform.position - lastPos, Time.deltaTime * 3);
		lastPos = transform.position;

		SquashAndStretch();
		CheckBounds();

		if (!canBeCaught) {
			if (rb.velocity.y <= 0) {	
				canBeCaught = true;
			}

			SetDarkerColor();
			transform.rotation = Quaternion.identity;			// Not sure why but for some reason when balls launch they rotate to 0 rather than always being there. This fixes that
		}
	}

	void SquashAndStretch() {
		// Make the balls squash and stretch to show/exaggerate motion

		if((Vector3)velocity != Vector3.zero) {
			Vector3 diff = velocity.normalized;
	         
			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
	        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }

//		if((Vector3)rb.velocity != Vector3.zero) {
//			Vector3 diff = rb.velocity.normalized;
//	         
//			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
//	        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
//        }

		art.transform.localScale = new Vector2(1 - velocity.magnitude * squashSize, 1 + velocity.magnitude * squashSize);

	}

	public void FlashColor() {
		// Flash the ball's color to white when it's grabbed
		StartCoroutine(Flash());
	}

	IEnumerator Flash() {
		sprite.GetComponent<SpriteRenderer>().color = flashColor;
		yield return new WaitForSeconds(flashDuration);
		sprite.GetComponent<SpriteRenderer>().color = originalColor;
	}

	void CheckBounds() {
		// Balls die when out of bounds
		if (Mathf.Abs(transform.position.x) > bounds.x || transform.position.y < bounds.y || transform.position.y > yBound) {
			GameManager.GetInstance().HandleGameOver();
		}
	}

	public void HandleDeath() {
		StartCoroutine(ShakeAndDie());
	}

	public AnimationCurve shakeCurve;

	IEnumerator ShakeAndDie() {
		yield return new WaitForSeconds(Random.Range(0, .2f));

		rb.velocity = Vector2.zero;
		rb.gravityScale = 0;
		chevron.HandleDeath();

		float t = 0;
		float duration = .5f;

		while (t < 1) {
			t += Time.deltaTime/duration;
			transform.localScale = new Vector2(.5f + shakeCurve.Evaluate(t), 1 - shakeCurve.Evaluate(t));

			yield return new WaitForEndOfFrame();
		}
		Die();
	}

	void Die() {
		explosion.transform.parent = null;
		explosion.GetComponent<Explosion>().Trigger();

		StartCoroutine(ResetAndPool());
	}

	IEnumerator ResetAndPool() {
		rb.velocity = Vector2.zero;
		rb.gravityScale = .75f;
		lastPos = transform.position;
		velocity = Vector2.zero;
		transform.localScale = Vector3.one;
		transform.rotation = Quaternion.identity;
		yield return new WaitForEndOfFrame();
		ObjectPool.instance.PoolObject(gameObject);

		transform.position = new Vector2(0, 6);
		lastPos = transform.position;
	}

	public void HandleGameOver() {
		// Explode
		HandleDeath();
	}
}