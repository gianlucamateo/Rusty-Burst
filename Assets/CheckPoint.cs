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
		if (IsPlayer(other))
			manager.NotifyCheckpoint (other.gameObject.transform.root.gameObject, index);
	}

	bool IsPlayer(Collider collider) {
		var car = collider.gameObject.transform.root.gameObject;
		return car == manager.player1 || car == manager.player2;
	}
}
