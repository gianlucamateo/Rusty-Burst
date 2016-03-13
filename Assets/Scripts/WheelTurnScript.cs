using UnityEngine;
using System.Collections;

public class WheelTurnScript : MonoBehaviour {

	public GameObject wheel;
	float axisRotation = 0;
	public Vector3 center;
	public float speed;
	public GameObject centerMarker;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		WheelCollider wCollider = wheel.gameObject.GetComponent<WheelCollider> ();
		speed = wCollider.rpm / 60 * 6;
		axisRotation += speed;

		//Vector3 prevAngles = transform.localRotation.eulerAngles;

		//transform.localRotation = Quaternion.Euler(prevAngles.x+speed,wCollider.steerAngle,prevAngles.z);
		Quaternion empty = new Quaternion();
		//transform.Rotate(new Vector3 (0, speed, 0));
		wCollider.GetWorldPose(out center, out empty);
		center.y -= centerMarker.transform.position.y;

		transform.localPosition = new Vector3 (transform.localPosition.x,center.y,transform.localPosition.z);
		transform.localRotation = Quaternion.Euler(0,0,90)*Quaternion.Euler (wCollider.steerAngle, 0, 0)*Quaternion.Euler (0, -axisRotation, 0);
	}
}
