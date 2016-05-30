using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour {

	private static float RESET_HEIGHT_ABOVE_TRACK = 4f;
	private static float RESET_SEPARATION_DISTANCE = 2f;

	public List<GameObject> checkpoints = new List<GameObject>();

	public GameObject player1;
	public GameObject player2;

	private GameObject player1Body, player2Body;

    private int[] nextCheckPoint = {0, 0};
    private int[] round = {0, 0};

	void Start () {
		// Because "chassis" is the GO with the scripts attached, not the root "car" object :/)
		player1Body = player1.transform.GetChild (0).gameObject;
		player2Body = player2.transform.GetChild (0).gameObject;

        // Teleport to start
	    AlignAtCheckPoint(0, player1Body, RESET_SEPARATION_DISTANCE);
        AlignAtCheckPoint(0, player2Body, -RESET_SEPARATION_DISTANCE);
	}

	void Update () {
	    if (Input.GetKeyDown(KeyCode.Period))
	        ResetPlayer(player1Body, RESET_SEPARATION_DISTANCE);

	    if (Input.GetKeyDown(KeyCode.Period))
	        ResetPlayer(player2Body, -RESET_SEPARATION_DISTANCE);
	}

	void OnGUI() {
        GUI.Label(new Rect(0, 0, 200, 20), String.Format("Player 0: Next CP: {0}, Round: {1}", nextCheckPoint[0], round[0]));
        GUI.Label(new Rect(0, 20, 200, 20), String.Format("Player 1: Next CP: {0}, Round: {1}", nextCheckPoint[1], round[1]));
    }

	public void NotifyCheckpoint(GameObject player, int checkpoint) {
	    int playerId = GetPlayerId(player);

        Debug.Log(String.Format("Player {0} entered checkpoint {1}", playerId, checkpoint));

        if (nextCheckPoint[playerId] == checkpoint) {
	        nextCheckPoint[playerId] = (nextCheckPoint[playerId] + 1) % checkpoints.Count;

            // Check for completed round
            if (nextCheckPoint[playerId] == 1) {
                round[playerId]++;
            }
	    }
	}

	private void ResetPlayer (GameObject player, float offset) {
	    int playerId = GetPlayerId(player);

	    int previousCP = nextCheckPoint[playerId] - 1;
	    if (previousCP < 0) previousCP = 0;

        // Reset Position
        AlignAtCheckPoint(previousCP, player, offset);
		// Reset physical properties
		player.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

    private void AlignAtCheckPoint(int checkpoint, GameObject player, float offset = 0f) {
        var cp = checkpoints[checkpoint];

        player.transform.position = cp.transform.position + new Vector3(offset, RESET_HEIGHT_ABOVE_TRACK, 2f);
        player.transform.rotation = cp.transform.localRotation;
    }

    private int GetPlayerId(GameObject player) {
        return player == player1 ? 0 : 1;
    }

}
