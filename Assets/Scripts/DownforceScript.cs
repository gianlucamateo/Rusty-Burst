using UnityEngine;
using System.Collections;

public class DownforceScript : MonoBehaviour {
	public float df, dfScale;
	public Rigidbody Rb;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		df = Mathf.Pow (Rb.velocity.magnitude, 2) * dfScale;
		Rb.AddForce (0, -df, 0);
	}
}
