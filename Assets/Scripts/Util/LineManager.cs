using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour {

	public float lineWidth = .15f;
	public float lineLengthModifier = 1;
	public float scalar = 1.5f;


	[System.NonSerialized]
	public float dragDistance;

	Vector3[] vertices;
	Material mat;
	Mesh lineMesh;

	public GameObject line;
	public GameObject arrow;

	// Use this for initialization
	void Start () {
		vertices = new Vector3[4];

		lineMesh = line.GetComponent<MeshFilter>().mesh;
		mat = line.GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMesh();
		UpdateArrow();

		mat.mainTextureScale = new Vector2(1, dragDistance * lineLengthModifier * scalar);
	}

	void UpdateMesh() {
		vertices[0] = new Vector3( lineWidth/scalar, dragDistance * lineLengthModifier, 0);
		vertices[1] = new Vector3(-lineWidth/scalar, 0, 0);
		vertices[2] = new Vector3(-lineWidth/scalar, dragDistance * lineLengthModifier, 0);
		vertices[3] = new Vector3( lineWidth/scalar, 0, 0);

		lineMesh.vertices = vertices;
	}

	void UpdateArrow() {
		arrow.transform.localPosition = new Vector2(0, dragDistance * lineLengthModifier);
	}
}
