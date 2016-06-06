using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour {

	public float lineWidth = .15f;
	public float lineLengthModifier = 1;
	public float scalar = 1.5f;

	public Vector2 throwVector;
	float dragStrength = 0;

	Vector3[] vertices;
	Material mat;
	Mesh lineMesh;

	public GameObject lineContainer;
	public GameObject line;
	public GameObject arrow;

	Ball ball;

	// Use this for initialization
	void Start () {
		ball = GetComponent<Ball>();

		vertices = new Vector3[4];

		lineMesh = line.GetComponent<MeshFilter>().mesh;
		mat = line.GetComponent<MeshRenderer>().material;

		line.GetComponent<MeshRenderer>().sortingLayerName = "UI";
		line.GetComponent<MeshRenderer>().sortingOrder = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if(ball.held) {
			dragStrength = throwVector.magnitude;
		} else {
			dragStrength = 0;
		}

		UpdateRotation();
		UpdateMesh();
		UpdateArrow();

		mat.mainTextureScale = new Vector2(1, dragStrength * lineLengthModifier * scalar);
	}

	void UpdateRotation() {
		Vector3 diff = throwVector;

		if(diff != Vector3.zero) {
	        diff.Normalize();
	        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			lineContainer.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        } else {
			lineContainer.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
	}

	void UpdateMesh() {
		vertices[0] = new Vector3( lineWidth/scalar, dragStrength * lineLengthModifier, 0);
		vertices[1] = new Vector3(-lineWidth/scalar, 0, 0);
		vertices[2] = new Vector3(-lineWidth/scalar, dragStrength * lineLengthModifier, 0);
		vertices[3] = new Vector3( lineWidth/scalar, 0, 0);

		lineMesh.vertices = vertices;
	}

	void UpdateArrow() {
		arrow.transform.localPosition = new Vector2(0, dragStrength * lineLengthModifier);
	}

	public void Reset() {
		throwVector = Vector2.zero;
	}
}
