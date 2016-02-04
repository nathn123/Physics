using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {

	// Use this for initialization
	public int K;
	public GameObject cube;
	public bool keyrelease = false;
	public Vector3 Displacement;
	Vector3 prevmousepos;
	Vector3 currentmousepos;
	public float minlength;
	public float current_length;
	public float rest_length;
	public Vector3 springforce;
	MyPhysics physics;
	void Start () {
		physics = FindObjectOfType<MyPhysics> ();
	}
	
	// Update is called once per frame
	void Update () {

		/*if(Input.GetMouseButtonDown(1))
		{
			//increase spring power
			Displacement.x += (rest_length - minlength) / 10; // 10 secs for max strength
			if(current_length<minlength)
				current_length = minlength;

			keyrelease = true;
		}
		if (Input.GetMouseButtonDown (1))
			prevmousepos = Camera.main.ScreenPointToRay (Input.mousePosition).origin;


		if(Input.GetMouseButtonUp(1))
		{
			currentmousepos = Camera.main.ScreenPointToRay (Input.mousePosition).origin;
			Displacement = prevmousepos - currentmousepos;
			springforce = -K*Displacement;
			RightClickedObject.GetComponent<CRigidBody>().ApplyForce(springforce);
		}*/
	
	}
}
