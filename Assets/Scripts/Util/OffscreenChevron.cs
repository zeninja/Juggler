using UnityEngine;
using System.Collections;

public class OffscreenChevron : MonoBehaviour {

	public Sprite arrow;
	public Sprite dead;

	public float yBound = 8.5f;

	public float xBound = 2.3f;
	public float yHeight = 7.25f;

	[System.NonSerialized]
	public GameObject ball;
	SpriteRenderer spriteRenderer;
	public SpriteRenderer background;

	public Color arrowColor;
	public Color deadColor;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		transform.parent = null;
	}

	void Update() {
		if(GameManager.gameOver) { return; }
		spriteRenderer.enabled = ball.transform.position.y > yBound;
		background.enabled = ball.transform.position.y > yBound;

		if(ball.transform.position.y > yBound) {
			transform.position = new Vector2(ball.transform.position.x, yHeight);

			// Keep the chevron from going offscreen horizontally
			if (Mathf.Abs(transform.position.x) > xBound) {
				transform.position = new Vector2(Mathf.Sign(transform.position.x) * xBound, yHeight);
			}
		}
	}

	public void SetColors() {
		GetComponent<SpriteRenderer>().color = ColorSchemeManager.ballColor;
		background.color = ColorSchemeManager.bgColor;
	}

	public void ChangeSpriteToDead() {
		spriteRenderer.sprite = dead;
		spriteRenderer.color = deadColor;
		Invoke("HandleDeath", .5f);
	}

	void HandleDeath() {
		Destroy(gameObject);
	}
}
