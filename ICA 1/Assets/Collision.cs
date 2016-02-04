using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Collision : MonoBehaviour {
	public enum CollisionType
	{
		Sphere,
		AABB
	};
	public Material myMaterial;
	protected LineRenderer lineRenderer;
    public Vector3 Position;
	public CollisionType Type;
	public int size = 10;
	public List<Vector3> Points;

	public Vector3 Max;
	public Vector3 Min;
	public Vector3 Centre;
	public float Radius; // half length / radius same thing only need one variable

	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = myMaterial;
		lineRenderer.SetWidth (0.01f, 0.01f);//thickness of line
	}

	// Update is called once per frame
	public abstract void PhysicsUpdate ();
}
