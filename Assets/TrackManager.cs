using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour {

	private static float RESET_HEIGHT_ABOVE_TRACK = 4f;
	private static float RESET_SEPARATION_DISTANCE = 2f;

	public List<GameObject> checkpoints = new List<GameObject>();

	public Player player1, player2;

	private int[] nextCheckPoint = { 0, 0 };
    private int[] round = {0, 0};
	public int[] rank = { 0, 0 };

	public float trackLength = 0.0f;
	public float[] distanceToFinish = { 0f, 0f };


	void Start () {
        // Teleport to start
	    AlignAtCheckPoint(0, player1, RESET_SEPARATION_DISTANCE);
        AlignAtCheckPoint(0, player2, -RESET_SEPARATION_DISTANCE);

		// Compute track length
		for (var i = 0; i < checkpoints.Count - 1; i++) {
			trackLength += (checkpoints [i + 1].transform.position - checkpoints [i].transform.position).magnitude;
		}
	}

	void Update () {
	    if (Input.GetKeyDown(KeyCode.R))
	        ResetPlayer(player1, RESET_SEPARATION_DISTANCE);

		if (Input.GetKeyDown(KeyCode.Period))
	        ResetPlayer(player2, -RESET_SEPARATION_DISTANCE);

		// Calculate Player Rank (num rounds + distance to finish)
		distanceToFinish[0] = (player1.transform.position - checkpoints[nextCheckPoint[0]].transform.position).magnitude;
		distanceToFinish[1] = (player2.transform.position - checkpoints[nextCheckPoint[1]].transform.position).magnitude;

		for (var i = nextCheckPoint [0]; i < checkpoints.Count; i++)
			distanceToFinish [0] += (checkpoints [i].transform.position - checkpoints [(i + 1) % checkpoints.Count].transform.position).magnitude;
		for (var i = nextCheckPoint [1]; i < checkpoints.Count; i++)
			distanceToFinish [1] += (checkpoints [i].transform.position - checkpoints [(i + 1) % checkpoints.Count].transform.position).magnitude;

		var score = new int[2] { round [0] * 10, round [1] * 10 };
		score [0] += distanceToFinish [0] < distanceToFinish [1] ? 1 : 0;
		score [1] += distanceToFinish [1] < distanceToFinish [0] ? 1 : 0;

		if (score [0] > score [1]) {
			rank [0] = 1;
			rank [1] = 2;
		}
		else{
			rank [0] = 2;
			rank [1] = 1;
		}

	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 400, 20), String.Format("Player 0: Next CP: {0}, Round: {1}, Rank: {2}", nextCheckPoint[0], round[0], rank[0]));
		GUI.Label(new Rect(0, 20, 400, 20), String.Format("Player 1: Next CP: {0}, Round: {1}, Rank: {2}", nextCheckPoint[1], round[1], rank[1]));
    }

	public void NotifyCheckpoint(Player player, int checkpoint) {
		int playerId = player.playerId;

        Debug.Log(String.Format("Player {0} entered checkpoint {1}", playerId, checkpoint));

        if (nextCheckPoint[playerId] == checkpoint) {
	        nextCheckPoint[playerId] = (nextCheckPoint[playerId] + 1) % checkpoints.Count;

            // Check for completed round
            if (nextCheckPoint[playerId] == 1) {
                round[playerId]++;
				distanceToFinish [playerId] = 0.0f;
            }
	    }
	}

	private void ResetPlayer (Player player, float offset) {
		int previousCP = nextCheckPoint[player.playerId] - 1;
	    if (previousCP < 0) previousCP = 0;

        // Reset Position
        AlignAtCheckPoint(previousCP, player, offset);
		// Reset physical properties
		player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

    private void AlignAtCheckPoint(int checkpoint, Player player, float offset = 0f) {
        var cp = checkpoints[checkpoint];
		var go = player.gameObject;

		go.transform.position = cp.transform.position + new Vector3(offset, RESET_HEIGHT_ABOVE_TRACK, 2f);
		go.transform.rotation = cp.transform.localRotation;
    }
}
