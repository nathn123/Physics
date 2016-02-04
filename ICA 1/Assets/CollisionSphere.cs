using UnityEngine;
using System.Collections;

public class CollisionSphere :Collision {

	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.AddComponent<LineRenderer> ();
		lineRenderer.material = myMaterial;
		lineRenderer.SetWidth (0.01f, 0.01f);//thickness of line
		lineRenderer.SetVertexCount (size);
        Position = this.gameObject.transform.position;
		Centre = Position;
		Radius = this.gameObject.transform.localScale.x / 2;
		Points = new System.Collections.Generic.List<Vector3> ();
		Points.Add( new Vector3(Radius*Mathf.Cos(Time.time),Radius*Mathf.Sin(Time.time),0));
		Points.Add( new Vector3(Radius * Mathf.Cos(Time.time + 1), Radius * Mathf.Sin(Time.time + 1), 0));
		Points.Add( new Vector3(Radius * Mathf.Cos(Time.time + 2), Radius * Mathf.Sin(Time.time + 2), 0));
		Points.Add( new Vector3(Radius * Mathf.Cos(Time.time + 3), Radius * Mathf.Sin(Time.time + 3), 0));
		Points.Add( new Vector3(Radius * Mathf.Cos(Time.time + 4), Radius * Mathf.Sin(Time.time + 4), 0));
		Type = Collision.CollisionType.Sphere;
	}
	
	// Update is called once per frame
	public override void PhysicsUpdate()
    {

        Position = this.gameObject.transform.position;
		Centre = Position;
		lineRenderer.SetPosition(0, Points[0] + Position);
		lineRenderer.SetPosition(1, Points[1] + Position);
		lineRenderer.SetPosition(2, Points[2] + Position);
		lineRenderer.SetPosition(3, Points[3] + Position);
		lineRenderer.SetPosition(4, Points[4] + Position);
    }
}
