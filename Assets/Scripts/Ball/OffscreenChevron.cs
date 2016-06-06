using UnityEngine;
using System.Collections;

public class OffscreenChevron : MonoBehaviour {

//	public Sprite arrow;
//	public Sprite dead;

	public float yBound = 8.49f;
	public float xBound = 3f;

	public float xPos = 2.3f;
	public float yPos = 7.25f;

	[System.NonSerialized]
	public GameObject ball;
	SpriteRenderer spriteRenderer;
	public SpriteRenderer background;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		transform.parent = null;
	}

	void Update() {
		if(GameManager.gameOver) { return; }

		spriteRenderer.enabled = ball.transform.position.y > yBound || Mathf.Abs(ball.transform.position.x) > xBound;
		background.enabled = ball.transform.position.y > yBound || Mathf.Abs(ball.transform.position.x) > xBound;

		if(ball.transform.position.y > yBound) {
			transform.position = new Vector2(ball.transform.position.x, yPos);

			// Keep the chevron from going offscreen horizontally
			if (Mathf.Abs(ball.transform.position.x) > xBound) {
				transform.position = new Vector2(Mathf.Sign(transform.position.x) * xPos, yPos);
			}
		}

		if (Mathf.Abs(ball.transform.position.x) > xBound) {
			transform.position = new Vector2(Mathf.Sign(ball.transform.position.x) * xPos, ball.transform.position.y);

			// Keep the chevron from going offscreen v
			if (ball.transform.position.y > yBound) {
				transform.position = new Vector2(Mathf.Sign(transform.position.x) * xPos, yPos);
			}
		}
	}

	public void SetColors() {
		GetComponent<SpriteRenderer>().color = ColorSchemeManager.ballColor;
		background.color = ColorSchemeManager.bgColor;
	}

	public void HandleDeath() {
		Invoke("Die", .5f);
	}

	void Die() {
		Destroy(gameObject);
	}
}
