using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour {

	static float RESET_HEIGH_ABOVE_TRACK = 4f;
	static float RESET_SEPARATION_DISTANCE = 4f;

	public List<GameObject> checkpoints = new List<GameObject>();

	public GameObject player1;
	public GameObject player2;

	// Use this for initialization
	void Start () {
		player1 = player1.transform.GetChild (0).gameObject;
		player2 = player2.transform.GetChild (0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Debug.Log("Resetting Player 1");
			ResetPlayer(player1, RESET_SEPARATION_DISTANCE / 2f);
		}
		if (Input.GetKeyDown (KeyCode.Period)) {
			Debug.Log ("Resetting Player 2");
			ResetPlayer (player2, -RESET_SEPARATION_DISTANCE / 2f);
		}
	}

	void OnGui() {
		GUI.DrawTexture (new Rect (0, 0, 20, 20), new Texture ());
	}

	public void NotifyCheckpoint(int player, int checkpoint) {
		Debug.Log (String.Format("Player {0} entered checkpoing {1}", player, checkpoint));
	}

	private void ResetPlayer (GameObject player, float offset) {
		// Reset Position
		player.transform.position = checkpoints[0].transform.position + Vector3.up * RESET_HEIGH_ABOVE_TRACK + Vector3.right * offset;
		// Reset Rotation
		player.transform.rotation = checkpoints[0].transform.rotation;
		player.transform.Rotate (0, 90, 0);
		// Reset physical properties
		player.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

}
