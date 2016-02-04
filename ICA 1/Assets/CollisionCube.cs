using UnityEngine;
using System.Collections;

public class CollisionCube :Collision {

	// Use this for initialization

	void Start () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = myMaterial;
		lineRenderer.SetWidth (0.01f, 0.01f);//thickness of line
		lineRenderer.SetVertexCount (10);
		Position = this.gameObject.transform.position;
		Centre = Position;
		Radius = this.gameObject.transform.localScale.x / 2;
		Points.Add( new Vector3(-Radius, Radius, -Radius));
		Points.Add( new Vector3(Radius,Radius,-Radius));
		Points.Add( new Vector3(Radius,Radius,Radius));
		Points.Add( new Vector3(-Radius,Radius,Radius));
		Points.Add( new Vector3(-Radius,Radius,-Radius));
		Points.Add( new Vector3(-Radius, -Radius, -Radius));
		Points.Add( new Vector3(Radius,-Radius,-Radius));
		Points.Add( new Vector3(Radius,-Radius,Radius));
		Points.Add( new Vector3(-Radius,-Radius,Radius));
		Points.Add( new Vector3(-Radius,-Radius,-Radius));
		Max = new Vector3 (Radius, Radius, Radius) + Position;
		Min = new Vector3 (-Radius,-Radius,-Radius) + Position;
		Type = Collision.CollisionType.AABB;
	}
	
	// Update is called once per frame
	public override void PhysicsUpdate() {
		Position = this.gameObject.transform.position;
		Centre = Position;
		Max = new Vector3 (Radius, Radius, Radius) + Position;
		Min = new Vector3 (-Radius,-Radius,-Radius) + Position;
		lineRenderer.SetPosition (0, Points[0]+Position);
		lineRenderer.SetPosition (1, Points[1]+Position);
		lineRenderer.SetPosition (2, Points[2]+Position);
		lineRenderer.SetPosition (3, Points[3]+Position);
		lineRenderer.SetPosition (4, Points[4]+Position);
		lineRenderer.SetPosition (5, Points[5]+Position);
		lineRenderer.SetPosition (6, Points[6]+Position);
		lineRenderer.SetPosition (7, Points[7]+Position);
		lineRenderer.SetPosition (8, Points[8]+Position);
		lineRenderer.SetPosition (9, Points[9]+Position);
	
	}
}
