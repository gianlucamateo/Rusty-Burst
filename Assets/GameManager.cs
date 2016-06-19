using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public enum GameState { Intro, BeforeStart, Racing, Finished }

	#region SetFromInspector
	public Player player1, player2;
	public TrackManager trackManager;

	public float IntroTime = 5f;
	public float PrepareTime = 5f;
	public float OutroTime = 5f;

	public int RoundsToFinish = 5;
	#endregion

	private CameraCoordinator camCoordinator;

	public GameState state = GameState.BeforeStart;

	private float startTime, raceStartTime, raceFinishTime, timeSinceStart, timeSinceRaceStart;
	public Player winner = null;

	void Awake() {
		camCoordinator = GetComponent<CameraCoordinator> ();

		state = GameState.Intro;
	}

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceStart = Time.time - startTime;

		if (timeSinceStart > IntroTime && state == GameState.Intro)
			state = GameState.BeforeStart;

		if (timeSinceStart > IntroTime + PrepareTime && state == GameState.BeforeStart) {
			state = GameState.Racing;
			raceStartTime = Time.time - timeSinceStart;
			trackManager.StartRace ();
		}
	}

	public void NotifyWinner(Player p) {
		this.winner = p;
		this.state = GameState.Finished;
		raceFinishTime = Time.time - raceStartTime;
	}

	void OnGUI() {
		// Draw Race Delay Countdown
		if (state == GameState.BeforeStart) {
			var style = new GUIStyle (GUI.skin.GetStyle ("label")) { fontSize = 32, alignment = TextAnchor.MiddleCenter };
			var label = String.Format ("Starting in {0:0.0}", (PrepareTime - timeSinceStart + IntroTime));

			GUI.Box (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), "");
			GUI.Box (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), "");

			GUI.Label (new Rect (Screen.width/2-100, Screen.height/2-50, 200, 100), label, style);
		}

		// Draw Position of players
		if (state == GameState.Racing) {
			DrawPos(player1);
			DrawPos(player2);
		}

		if (state == GameState.Finished) {
			var style = new GUIStyle (GUI.skin.GetStyle ("label")) { fontSize = 32, alignment = TextAnchor.MiddleCenter };
			var label = String.Format ("{0} wins!", winner.GetName());

			GUI.Box (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), "");
			GUI.Box (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), "");

			GUI.Label (new Rect (Screen.width/2-100, Screen.height/2-50, 200, 100), label, style);
		}
	}

	private void DrawPos(Player p) {
		var cam = p.Cam;
		var camRect = cam.pixelRect; // origin is bottom left

		var upLeft = camRect.position + new Vector2(0f, cam.pixelHeight);
		upLeft.y = Screen.height - upLeft.y;

		var drawRect = new Rect (upLeft + new Vector2(5f, 5f), new Vector2 (80f, 40f));
		var style = new GUIStyle (GUI.skin.GetStyle("label")) { fontSize = 18, alignment = TextAnchor.MiddleCenter };

		GUI.Box (drawRect, "");
		var label = p.Rank == 1 ? "1st" : "2nd";
		GUI.Label (drawRect, label, style);
	}
}
