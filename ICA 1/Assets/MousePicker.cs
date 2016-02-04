using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MousePicker : MonoBehaviour {

	// Use this for initialization
	public bool SomethingClicked;
	public GameObject ClickedObject;
	public GameObject Cube, Sphere;
	public Text Masstext;
	public Text Acceltext;
	public Text Veloctext;
	public Text Forcetext;
	public Vector3 Displacement;
	public bool OnX;
	public bool OnY;
	public int K;
	MyPhysics physics;
	float cooldown;
	Vector3 prevmousepos;
	public Vector3 springforce;
	GameObject RightClickedObject;
	Vector3 currentmousepos;
	void Start () {
	
		physics = FindObjectOfType<MyPhysics> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100.0f)){
				SomethingClicked = true;
				ClickedObject = hit.collider.gameObject;
			}
		}
		if(ClickedObject != null)
		{
			var body = ClickedObject.GetComponent<CRigidBody>();


			Masstext.text = "Mass:"+ body.mMass.ToString();
			Acceltext.text = "Acceleration:"+body.mAcceleration.ToString();
			Veloctext.text = "Velocity:"+body.mVelocity.ToString();
			Forcetext.text = "Momentum :"+body.mMomentum.ToString();
		}

		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			Ray rightray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (rightray, out hit, 100.0f)) {
				SomethingClicked = true;
				RightClickedObject = hit.collider.gameObject;
				prevmousepos = Input.mousePosition;
			}
		}
		
		if(Input.GetMouseButtonUp(1))
		{
			currentmousepos = Input.mousePosition;
			Displacement = prevmousepos - currentmousepos;
			Displacement.z = 0;
			springforce = -K*Displacement;
	
			RightClickedObject.GetComponent<CRigidBody>().ApplyForce(springforce);
		}
		if (Input.GetKey (KeyCode.L) && Time.timeSinceLevelLoad > cooldown)
		{
			var newobje  = (GameObject)Instantiate (Cube, new Vector3 (0,20, 0), Quaternion.identity);
			physics.AddBody(newobje.GetComponent<CRigidBody>());
			cooldown = Time.timeSinceLevelLoad + 1.0f;
		}
		if (Input.GetKey (KeyCode.P)&& Time.timeSinceLevelLoad > cooldown)
		{
			var newobje  = (GameObject)Instantiate (Sphere, new Vector3 (Camera.main.ScreenPointToRay (Input.mousePosition).origin.x, Camera.main.ScreenPointToRay (Input.mousePosition).origin.y, 0), Quaternion.identity);
			physics.AddBody(newobje.GetComponent<CRigidBody>());
			cooldown = Time.timeSinceLevelLoad + 1.0f;
		}

	
	}

}
