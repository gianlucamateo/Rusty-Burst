using UnityEngine;
using System.Collections;

public class DownforceScript : MonoBehaviour {
	public float df, dfScale;
	public Rigidbody Rb;
	public float AoA; //angle of attack
	public GameObject jumpProbe;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		df = Mathf.Pow (Rb.velocity.magnitude, 2) * dfScale * (AoA/30f);

		bool inAir = !Physics.Raycast (jumpProbe.transform.position, -jumpProbe.transform.up, 1f);

		if (inAir) {
			df = 0;
		}

		Rb.AddForce (0, -df, 0);
	}
}
