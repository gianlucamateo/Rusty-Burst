using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour {

	public List<GameObject> checkpoints = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGui() {
		GUI.DrawTexture (new Rect (0, 0, 20, 20), new Texture ());
	}
}
