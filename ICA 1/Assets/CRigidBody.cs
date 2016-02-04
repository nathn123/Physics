using UnityEngine;
using System.Collections;

public class CRigidBody : MonoBehaviour {

	// Use this for initialization
    public Collision mCollider;
    public Vector3 mForce;
    public Vector3 mAcceleration;
    public Vector3 mVelocity;
	public Vector3 mPosition;
	public Matrix4x4 mInverseInertiaTensor;
	public Vector3 mAngularAcceleration;
	public Vector3 mAngularVelocity;
	public Quaternion mOrientation;
	public Vector3 mTorque;
    public float mMass;
	public float mRestitution;
	public float mStaticFriction;
	public float mDynamicFriction;
	public bool IsGround;
	public Vector3 mMomentum;
	public bool Instanciated = false;
    private Vector3 Gravity = new Vector3(0.0f, -9.81f, 0.0f);
	void Start () {
		mPosition = this.transform.position;
		mCollider = GetComponent<Collision>();
		mMass = Mathf.Pow (mCollider.size, 3);
		if (IsGround)
			mMass = 0;
		Instanciated = true;

		mOrientation = this.transform.rotation;
	}
	public void Instanciate(MyPhysics physics)
	{
		mPosition = this.transform.position;
		mCollider = GetComponent<Collision>();
		mMass = Mathf.Pow (mCollider.size, 3);
		physics.AddBody (this);
		Instanciated = true;
	}
	
	// changed to give control of when it is updated i.e not before colision detection
	public void PhysicsUpdate (float deltatime) {

		if (IsGround)
			return; // if its ground do nothing


		mAcceleration = (mForce/mMass)+ Gravity;
		mVelocity += (mAcceleration * deltatime);
		mPosition += mVelocity*deltatime;

		this.transform.position = mPosition;
		this.transform.rotation = mOrientation;
		mMomentum += mMass * mVelocity;
		//mMomentofInertia += mInverseInertiaTensor * mAngularVelocity;
		mCollider.PhysicsUpdate ();
        // resolve forces

	//	Debug.Log ("test"+mMomentum.ToString());
	}

    public void ApplyForce(Vector3 force)
    {
		mForce += force;
    }
	public void ApplyImpulse(Vector3 Impulse)
	{
		//impusle directly applied to velocity
		if (IsGround)
			return;
		mVelocity +=(Impulse/mMass);
		mMomentum += Impulse;
	}
    public void ApplyFriction(Vector3 NormalForce)
    {
        mForce += (mDynamicFriction*NormalForce);
    }
	public bool CheckStaticFriction(Vector3 NormalForce)
	{

		if (mForce.sqrMagnitude > (mStaticFriction * NormalForce).sqrMagnitude)
		{
			//static friction applies so vel is 0
			mVelocity = new Vector3(0,0,0);
			return false;
		}
		return true;
	}

}
