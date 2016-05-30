using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	public int index;
	public TrackManager manager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Report player entering checkpoint to Manager
	void OnTriggerEnter(Collider other) {
		manager.NotifyCheckpoint (0, index);
	}
}
