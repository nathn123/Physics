using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyPhysics : MonoBehaviour {

	public List<CRigidBody> mBodies;
	public float updatetime;
	public float steptime;
	public bool debug;
	float lastframetime;

	// Use this for initialization
	void Start () {

		foreach (var body in FindObjectsOfType<CRigidBody>())
			mBodies.Add (body);
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		foreach (var body in mBodies)
			if (!body.Instanciated)
				body.Instanciate (this);
		var time = Time.deltaTime;
		lastframetime += time; 
		if (debug) {
			if (lastframetime < updatetime)
				return;
			lastframetime = 0;
			time = steptime;
		} 
		for (int i = 0; i< mBodies.Count-1; i++) {
			if (mBodies [i] == null)
				continue;
				for (int j = i+1; j< mBodies.Count; j++) {
					if(mBodies[j] == null)
						continue;
				Vector3 Normal = new Vector3 (0, 0, 0);
				if (BroadphaseTest (mBodies [i], mBodies [j]))
					if (NarrowphaseTest (mBodies [i], mBodies [j], ref Normal))
						CollisionResponse (mBodies [i], mBodies [j], Normal);
			}
		}
		//resolve forces
		foreach (var body in mBodies)
			body.PhysicsUpdate (time);
	
	}
	public void AddBody(CRigidBody newbody)
	{
		mBodies.Add (newbody);
	}
	public void RemoveBody(CRigidBody oldbody)
	{
		mBodies.Remove (oldbody);
	}
	bool BroadphaseTest(CRigidBody body1, CRigidBody body2)
	{
		// not implemented return true

		return true;
	}
	bool NarrowphaseTest(CRigidBody body1, CRigidBody body2,ref Vector3 Normal)
	{
		if (body1.IsGround == true && body2.IsGround == true)
			return false;
		/*//check for square(AABB) to square
		if (body1.mCollider.Type == Collision.CollisionType.AABB && body2.mCollider.Type == Collision.CollisionType.AABB)
			return AABBCollision (body1, body2,ref Normal);
		//else sphere to sphere
		else if (body1.mCollider.Type == Collision.CollisionType.Sphere && body2.mCollider.Type == Collision.CollisionType.Sphere)
			return SphereCollision (body1, body2,ref Normal);
		else
			return AABBSphereCollision (body1, body2,ref Normal);*/
			return SATCollision (body1, body2,ref Normal);
		
	}
	void CollisionResponse(CRigidBody body1, CRigidBody body2, Vector3 Normal)
	{
		bool Test = false;
		// deciede which type of response
		var RelVel = RelativeVelocity (body1.mVelocity, body2.mVelocity,Normal);
		Debug.Log (RelVel.ToString());
		if(RelVel > 0 && RelVel < 1)
			 Test = !Test;
		if (RelVel > 0)
			return; // objects moving away no response needed
		else if (RelVel < 0)
			PenetratingResponse(body1, body2,RelVel,Normal);//objects colliding response needed
		else if (RelVel == 0)
			RestingResponse(body1,body2,Normal);// resting contact
	}
	float RelativeVelocity(Vector3 vel1, Vector3 vel2, Vector3 Normal)
	{
		return Vector3.Dot(Normal,vel2 - vel1);
	}
	void RestingResponse(CRigidBody body1, CRigidBody body2, Vector3 Normal)
	{
		return;

		if (body1.CheckStaticFriction (Normal))
			body1.ApplyFriction (Normal);
		if (body2.CheckStaticFriction (Normal))
			body2.ApplyFriction (Normal);

	}
	void PenetratingResponse(CRigidBody body1, CRigidBody body2,float RelVel, Vector3 Normal)
	{
		var CoefResti = (body1.mRestitution+body2.mRestitution)/2;
		//set numerator

		var Numer = -(1+CoefResti)*RelVel;
		float Denom;
		if(body1.mMass == 0)
			Denom = 1/body2.mMass;
		else if(body2.mMass == 0)
			Denom = 1/body1.mMass;
		else 
			Denom = 1/body1.mMass + 1/body2.mMass;
		var Impulse = Numer / Denom;
		body1.ApplyImpulse (-Normal*Impulse);
		body2.ApplyImpulse (Normal*Impulse);

		// friction
		/*var RV = body2.mVelocity - body1.mVelocity;
		var tangent = RV - Vector3.Dot (RV, Normal) * Normal;
		tangent.Normalize ();
		float mag  = -Vector3.Dot (RV,tangent) / (1/body1.mMass + 1/body2.mMass);

		float staticcoef = Mathf.Sqrt ((body1.mStaticFriction * body1.mStaticFriction) + (body2.mStaticFriction * body2.mStaticFriction));
		if (Mathf.Abs (mag) < Impulse * staticcoef)
		{
			body1.ApplyForce( mag * tangent);
			body2.ApplyForce(mag* -tangent);
		}
		else
		{
			float dynamiccoef = Mathf.Sqrt ((body1.mDynamicFriction * body1.mDynamicFriction) + (body2.mDynamicFriction * body2.mDynamicFriction));
			body1.ApplyForce( -Impulse * tangent * dynamiccoef);
			body2.ApplyForce(Impulse * -tangent * dynamiccoef);
		}*/

	}
	bool AABBCollision(CRigidBody body1, CRigidBody body2, ref Vector3 Normal)
	{
		/*float distance = (body2.mCollider.Centre - body1.mCollider.Centre).magnitude - (body1.mCollider.Radius+body2.mCollider.Radius);
		Debug.Log (distance.ToString ());*/
		if(body1.mCollider.Max.x > body2.mCollider.Min.x &&
		   body1.mCollider.Max.y > body2.mCollider.Min.y &&
		   body1.mCollider.Max.z > body2.mCollider.Min.z &&
		   body1.mCollider.Min.x < body2.mCollider.Max.x &&
		   body1.mCollider.Min.y < body2.mCollider.Max.y &&
		   body1.mCollider.Min.z < body2.mCollider.Max.z)
		{
			Normal = Norm(body1,body2);
			return true;
		}

		return false;
	}
	Vector3 Norm(CRigidBody body1, CRigidBody body2)
	{
		//return (body1.mCollider.Position - body2.mCollider.Position) / (body1.mCollider.Position - body2.mCollider.Position).magnitude; 
		Vector3 Normal = new Vector3 ();
		float penet = 0.0f;
		// if we consider b1 on the bot and b2 on the top 
		var B1MAX = body1.mCollider.Max;
		var B1MIN = body1.mCollider.Min;
		var B2MAX = body2.mCollider.Max;
		var B2MIN = body2.mCollider.Min;
		bool left = false, right= false, top= false, bottom= false, front= false, back= false;
		if (B1MAX.x > B2MIN.x) // check left vs right 1st
			right = true;
		if (B1MAX.y > B2MIN.y) // check up vs down
			top = true;
		if (B1MAX.z > B2MIN.z)
			back = true;
		if (B2MAX.z > B1MIN.z)
			front = true;
		if (B2MAX.y > B1MIN.y)
			bottom = true;
		if (B2MAX.x > B1MIN.x)
			left = true;


		// now find face with the LEAST penetration
		if (left)
		{
			penet = Mathf.Abs(B2MAX.x - B1MIN.x);
			Normal = new Vector3(-1,0,0);
		}
		if (top && penet > Mathf.Abs(B1MAX.y - B2MIN.y))
		{
			penet = Mathf.Abs(B1MAX.y - B2MIN.y);
			Normal = new Vector3(0,1,0);
		}
		if (front && penet > Mathf.Abs(B2MAX.z - B1MIN.z))
		{
			penet = Mathf.Abs(B2MAX.z - B1MIN.z);
			Normal = new Vector3(0,0,-1);
		}
		if (right && penet > Mathf.Abs(B1MAX.x - B2MIN.x))
		{
			penet = Mathf.Abs(B1MAX.x - B2MIN.x);
			Normal = new Vector3(1,0,0);
		}
		if(bottom&& penet >Mathf.Abs(B2MAX.y - B1MIN.y))
		{
			penet = Mathf.Abs(B2MAX.y - B1MIN.y);
			Normal = new Vector3(0,-1,0);
		}
		if(back&& penet >Mathf.Abs(B1MAX.z - B2MIN.z))
		{
			penet = Mathf.Abs(B1MAX.z - B2MIN.z);
			Normal = new Vector3(0,0,1);
		}





		return Normal;

	}
	bool SphereCollision(CRigidBody body1, CRigidBody body2, ref Vector3 Normal)
	{
		Normal = (body1.mCollider.Centre - body2.mCollider.Centre)/ (body1.mCollider.Centre - body2.mCollider.Centre).magnitude;
		bool test = (body1.mCollider.Centre - body2.mCollider.Centre).magnitude <= (body1.mCollider.Radius + body2.mCollider.Radius);
		return test;
	}
	bool AABBSphereCollision(CRigidBody body1, CRigidBody body2, ref Vector3 Normal)
	{
		CRigidBody AABB, Sphere;

		if (body1.mCollider.Type == Collision.CollisionType.AABB)
		{
			AABB = body1;
			Sphere = body2;
		}
		else
		{
			AABB = body2;
			Sphere = body1;
		}


		Vector3 N = AABB.mPosition - Sphere.mPosition;
		Vector3 Closestpoint = N;
		Closestpoint.x = Mathf.Clamp (Closestpoint.x,-AABB.mCollider.Radius, AABB.mCollider.Radius);
		Closestpoint.y = Mathf.Clamp (Closestpoint.y,-AABB.mCollider.Radius, AABB.mCollider.Radius);
		Closestpoint.z = Mathf.Clamp (Closestpoint.z,-AABB.mCollider.Radius, AABB.mCollider.Radius);
		bool inside = false;
		if(N == Closestpoint)
		{
			inside = true;
			// find closest axis
			if(Mathf.Abs(N.x) > Mathf.Abs(N.y) && Mathf.Abs(N.x) > Mathf.Abs(N.z))
			{
				// clam to nearest point
				if(Closestpoint.x > 0)
					Closestpoint.x = AABB.mCollider.Radius;
				else
					Closestpoint.x = -AABB.mCollider.Radius;
			}
			else if (Mathf.Abs(N.y) > Mathf.Abs(N.z))
			{
				if(Closestpoint.y > 0)
					Closestpoint.y = AABB.mCollider.Radius;
				else
					Closestpoint.y = -AABB.mCollider.Radius;
			}
			else
			{
				if(Closestpoint.z > 0)
					Closestpoint.z = AABB.mCollider.Radius;
				else
					Closestpoint.z = -AABB.mCollider.Radius;
			}
		}
		Vector3 normal = N - Closestpoint;
		float dist = normal.sqrMagnitude;

		if (dist > Sphere.mCollider.Radius * Sphere.mCollider.Radius && !inside)
			return false;

		if (inside)
			Normal = -N;
		else
			Normal = N;


		/*var TL = AABB.mCollider.TL;
		var BR = AABB.mCollider.BR;
		var R = Sphere.mCollider.Radius;
		var C = Sphere.mCollider.Centre;
		var RSQ = R * R;
		if(C.x<TL.x)
			RSQ-=(C.x - TL.x)*(C.x - TL.x);
		else if(C.x<BR.x)
			RSQ-=(C.x - BR.x)*(C.x - BR.x);
		if(C.y<TL.y)
			RSQ-=(C.y - TL.y)*(C.y - TL.y);
		else if(C.y<BR.y)
			RSQ-=(C.y - BR.y)*(C.y - BR.y);
		if(C.z<TL.z)
			RSQ-=(C.z - TL.z)*(C.z - TL.z);
		else if(C.z<BR.z)
			RSQ-=(C.z - BR.z)*(C.z - BR.z);*/

		return true;
	}

	bool SATCollision(CRigidBody body1, CRigidBody body2, ref Vector3 Normal)
	{
		//test 

		//Vector3 Axis = new Vector3 (0, 0, 0);

		// first we get the normals to test against
		List<Vector3> Norms = new List<Vector3> ();
		Norms.AddRange (GetNormals (body1));
		Norms.AddRange (GetNormals (body2));

		foreach(var axis in Norms)
		{
			var B1MAX = GetMaxAxis(body1,axis);
			var B1MIN = GetMinAxis(body1,axis);
			var B2MAX = GetMaxAxis(body2,axis);
			var B2MIN = GetMinAxis(body2,axis);

			if(B1MAX < B2MIN || B2MAX < B1MIN)
			{
				Normal = axis;
				return true;
			}

		}

		// if no colisions on any axis
		return false;

	}
	List<Vector3> GetNormals (CRigidBody body)
	{
		List<Vector3> norms = new List<Vector3> ();

		for(int i = 0; i<body.mCollider.Points.Count-1;++i)
		{
			var prenorm = body.mCollider.Points[i+1]-body.mCollider.Points[i];
			var leftnorm = new Vector3(prenorm.x,prenorm.y*-1,prenorm.z);
			norms.Add(leftnorm.normalized);
		}
		return norms;
	}
	float GetMinAxis(CRigidBody body,Vector3 Axis)
	{
		float min = 0;

		foreach(var point in body.mCollider.Points)
		{
			var current = Vector3.Dot(point,Axis);
			if(current < min)
				min = current;
		}
		return min;

	}
	float GetMaxAxis(CRigidBody body,Vector3 Axis)
	{
		float max = 0;
		
		foreach(var point in body.mCollider.Points)
		{
			var current = Vector3.Dot(point,Axis);
			if(current > max)
				max = current;
		}
		return max;
		
	}
	bool ClosestPoints(CRigidBody body1, CRigidBody body2, ref Vector3 Normal)
	{
		return true;
	}


}
