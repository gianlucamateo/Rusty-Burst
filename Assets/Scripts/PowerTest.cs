﻿using UnityEngine;
using System.Collections;

public class PowerTest : MonoBehaviour {

	public Rigidbody rb;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		rb.AddTorque (10, 0, 0);
	}
}
