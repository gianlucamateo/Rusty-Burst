using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cannon : MonoBehaviour {
	public GameObject projectilePrefab;
	public GameObject Cockpit;
	public GameObject Gun;
	public Rigidbody ChassisRigidB;

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

		if (Input.GetKey(KeyCode.A)) {
			Cockpit.transform.Rotate(0.0f, -1.0f, 0.0f);
		}

		if (Input.GetKey(KeyCode.D)) {
			Cockpit.transform.Rotate(0.0f, 1.0f, 0.0f);
		}
		
		if (Input.GetKey(KeyCode.W)) {
			Gun.transform.Rotate(-1.0f, 0.0f, 0.0f);
		}

		if (Input.GetKey(KeyCode.S)) {
			Gun.transform.Rotate(1.0f, 0.0f, 0.0f);
		}
	}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetButtonDown("Fire1")) {
		if (Input.GetKey(KeyCode.Space) && fire){
			StartCoroutine(Fire());
    		}
	}

	IEnumerator Fire(){
		fire = false;
		Vector3 spawnPos = transform.position + new Vector3 (0.0f, 0.2f, 0.0f);
		GameObject projectile = Instantiate(projectilePrefab, spawnPos, transform.rotation) as GameObject;
       		 //projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 3000));
		projectile.GetComponent<Rigidbody>().velocity = ChassisRigidB.velocity;
		projectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 60, ForceMode.Impulse);
		// change firing rate here
		yield return new WaitForSeconds(0.5f);
		fire = true;
	}
}
