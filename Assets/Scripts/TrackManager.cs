using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour {

	private static float RESET_HEIGHT_ABOVE_TRACK = 2f;
	private static float RESET_SEPARATION_DISTANCE = 2f;

	public List<GameObject> checkpoints = new List<GameObject>();
	public Player player1, player2;

	private int[] nextCheckPoint = { 0, 0 };
    private int[] round = {0, 0};

	private float trackLength = 0.0f;
	private float[] distanceToFinish = { 0f, 0f };
	private float[] roundTime = { 0f, 0f };
	private float[] startTime = { 0f, 0f };

	private float raceStartTime;
	private bool raceStarted = false;

	public bool showDebugInfo = false;

	private Texture2D bg;

	void Start () {
        // Teleport to start
	    AlignAtCheckPoint(0, player1, RESET_SEPARATION_DISTANCE);
        AlignAtCheckPoint(0, player2, -RESET_SEPARATION_DISTANCE);

		// Freeze player's cars at beginning
		player1.Freeze ();
		player2.Freeze ();

		bg = new Texture2D (1, 1);
		bg.SetPixel (0, 0, new Color(0.8f, 0.8f, 0.8f, 0.8f));
		bg.Apply ();

		// Compute track length
		for (var i = 0; i < checkpoints.Count - 1; i++) {
			trackLength += (checkpoints [i + 1].transform.position - checkpoints [i].transform.position).magnitude;
		}

	}

	public void StartRace() {
		Debug.LogFormat ("Unfreezing Players");
		player1.UnFreeze ();
		player2.UnFreeze ();

		// Set timer times
		startTime[0] = Time.time;
		startTime[1] = Time.time;

		raceStartTime = Time.time;
		raceStarted = true;
	}

	void Update () {
		if (!raceStarted)
			return;

		if (player1.InputReset)
	        ResetPlayer(player1, RESET_SEPARATION_DISTANCE);

		if (player2.InputReset)
	        ResetPlayer(player2, -RESET_SEPARATION_DISTANCE);

		// Calculate Player Rank (num rounds + distance to finish)
		distanceToFinish[0] = (player1.transform.position - checkpoints[nextCheckPoint[0]].transform.position).magnitude;
		distanceToFinish[1] = (player2.transform.position - checkpoints[nextCheckPoint[1]].transform.position).magnitude;
	
		var nextPseudoCp = new int[2];
		nextPseudoCp [0] = nextCheckPoint [0] == 0 ? checkpoints.Count : nextCheckPoint [0];
		nextPseudoCp [1] = nextCheckPoint [1] == 0 ? checkpoints.Count : nextCheckPoint [1];

		var score = new int[2] { round [0] * 1000 + nextPseudoCp[0] * 10, round [1] * 1000 + nextPseudoCp[1] * 10 };
		score [0] += distanceToFinish [0] < distanceToFinish [1] ? 1 : 0;
		score [1] += distanceToFinish [1] < distanceToFinish [0] ? 1 : 0;

		if (score [0] > score [1]) {
			player1.Rank = 1;
			player2.Rank = 2;
			player1.GetComponent<CarController> ().dragScale = 1.2f;
			player2.GetComponent<CarController> ().dragScale = 0.8f;
		}
		else {
			player1.Rank = 2;
			player2.Rank = 1;
			player1.GetComponent<CarController> ().dragScale = 0.8f;
			player2.GetComponent<CarController> ().dragScale = 1.2f;
		}

	}

	void OnGUI() {
		// Draw debug info if desired
		if (showDebugInfo) {
			GUI.Label(new Rect(0, 0, 600, 20), String.Format("Player 0: Next CP: {0}, Round: {1}, Rank: {2}, last Lap: {3:0.00}s, Lap: {4:0.00}s",
				nextCheckPoint[0], round[0], player1.Rank, roundTime[0], Time.time - startTime[0]));
			GUI.Label(new Rect(0, 20, 600, 20), String.Format("Player 1: Next CP: {0}, Round: {1}, Rank: {2}, last Lap: {3:0.00}s, Lap: {4:0.00}s",
				nextCheckPoint[1], round[1], player2.Rank, roundTime[1], Time.time - startTime[1]));
		}
    }
		
	public void NotifyCheckpoint(Player player, int checkpoint) {
		int playerId = player.playerId;

        Debug.Log(String.Format("Player {0} entered checkpoint {1}", playerId, checkpoint));

        if (nextCheckPoint[playerId] == checkpoint) {
	        nextCheckPoint[playerId] = (nextCheckPoint[playerId] + 1) % checkpoints.Count;

            // Check for completed round
            if (nextCheckPoint[playerId] == 1) {
                round[playerId]++;
				player.Rounds = round [playerId];
				distanceToFinish [playerId] = 0.0f;
				roundTime [playerId] = Time.time - startTime[playerId];
				startTime [playerId] = Time.time;
            }

			// Check for Race Finish
			var gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
			if (round [playerId] == gm.RoundsToFinish + 1 && gm.winner == null)
				gm.NotifyWinner (player);
	    }
	}

	private void ResetPlayer (Player player, float offset) {
		int previousCP = nextCheckPoint[player.playerId] - 1;
		if (previousCP < 0) previousCP = checkpoints.Count - 1;

        // Reset Position
        AlignAtCheckPoint(previousCP, player, offset);
		// Reset physical properties
		player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

    private void AlignAtCheckPoint(int checkpoint, Player player, float offset = 0f) {
        var cp = checkpoints[checkpoint];
		var go = player.gameObject;

		go.transform.position = cp.transform.position + cp.transform.TransformDirection(offset, RESET_HEIGHT_ABOVE_TRACK, 4f);
		go.transform.rotation = cp.transform.localRotation;
    }

	public Player GetCurrentLeader() {
		return player1.Rank > player2.Rank ? player1 : player2;
	}

}
