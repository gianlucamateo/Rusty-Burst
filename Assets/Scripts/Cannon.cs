using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cannon : MonoBehaviour {
	public GameObject projectilePrefab;
	public GameObject Cockpit;
	public GameObject Gun;
	public Rigidbody ChassisRigidB;
	public Animation fireAnim;
	public KeyCode up, down, left, right, fireKey;
	public Texture2D reticle;
	public Camera carCam;
	public Rect carVP;
	public Vector3 basePos, posOnScreen;

	private bool fire = true;

	void LateUpdate(){

		//// Code for movement using mouse
		//float x = Input.GetAxis("Mouse X") * 2;
		//float y = -Input.GetAxis("Mouse Y");

		////vertical
		//float yClamped = transform.eulerAngles.x + y;
		//transform.rotation = Quaternion.Euler(yClamped, transform.eulerAngles.y, transform.eulerAngles.z);

		////horizontal
		//transform.RotateAround(new Vector3(0, 3, 0), Vector3.up, x);
		////test horizontal
		////float xClamped = transform.eulerAngles.y + x;
		////transform.rotation = Quaternion.Euler(transform.eulerAngles.x, xClamped, transform.eulerAngles.z);

		//var angle = Mathf.Clamp(angle, 90, 270);

		if (Input.GetKey(left)) {
			Cockpit.transform.Rotate(0.0f, -1.0f, 0.0f);
		}

		if (Input.GetKey(right)) {
			Cockpit.transform.Rotate(0.0f, 1.0f, 0.0f);
		}

		float xtrans = Gun.transform.localEulerAngles.x;

		if (Input.GetKey(up)) {
			if(xtrans >= 320 || xtrans <= 30)
				Gun.transform.Rotate(-1.0f, 0.0f, 0.0f);
		}

		if (Input.GetKey(down)) {
			if(xtrans >= 310 || xtrans <= 20)
				Gun.transform.Rotate(1.0f, 0.0f, 0.0f);
		}
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position+ 0.2f*transform.up, transform.forward*1000,Color.white);
		//if (Input.GetButtonDown("Fire1")) {
		if (Input.GetKey(fireKey) && fire){
			StartCoroutine(Fire());
    		}
	}

	void OnGUI() {
		RaycastHit rcHit;
		var rayCast = Physics.Raycast(transform.position + 0.2f*transform.up, transform.forward,out rcHit);

		carVP = carCam.rect;
		var offset = carVP.y;//== 0.5? 0f : 0.5f;
		posOnScreen = carCam.WorldToScreenPoint(transform.position + 0.2f*transform.up + 1000f*transform.forward)- new Vector3(50f,-50f,0f);
		// posOnScreen = new Vector2(carCam.pixelWidth/2-50,(carCam.pixelHeight*(3-(offset)))/2-50);


		if (rayCast) {
			posOnScreen = carCam.WorldToScreenPoint (rcHit.point) - new Vector3(50f,-50f,0f);
		}
		GUI.DrawTexture (new Rect (new Vector2(posOnScreen.x,Screen.height - posOnScreen.y), (new Vector2 (100f, 100f))),reticle);
	}

	IEnumerator Fire(){
		fire = false;
		Vector3 spawnPos = transform.position + new Vector3 (0.0f, 0.2f, 0.0f);
		GameObject projectile = Instantiate(projectilePrefab, spawnPos, transform.rotation) as GameObject;
       		 //projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 3000));
		var projectileRB = projectile.GetComponent<Rigidbody>();
		projectileRB.velocity = ChassisRigidB.velocity;
		projectileRB.AddRelativeForce(Vector3.forward * 120, ForceMode.Impulse);
		projectileRB.mass = 100;
		projectileRB.useGravity = false;
		var globalDir = transform.TransformVector(Vector3.right);

		ChassisRigidB.AddTorque (-globalDir * 9000);

		// change firing rate here
		yield return new WaitForSeconds(0.5f);
		fire = true;
		fireAnim.Play ();
	}
}
