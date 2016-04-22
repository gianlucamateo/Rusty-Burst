using UnityEngine;
using System.Collections;

public class TachoScript : MonoBehaviour {
	public Rigidbody car;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		TextMesh text = GetComponent<TextMesh> ();
		string speed = "" + Mathf.Round(car.velocity.magnitude*3.6f)+" km/h";
		text.text = speed;
	}
}
