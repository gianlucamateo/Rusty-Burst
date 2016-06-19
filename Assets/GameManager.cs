using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
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
	public GUISkin skin;
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
			raceStartTime = Time.time;
			trackManager.StartRace ();
		}

		// Show start menu when outro is over
		if (state == GameState.Finished && (Time.time - raceFinishTime) > OutroTime) {
			SceneManager.LoadScene ("Startmenu");
		}
	}

	public void NotifyWinner(Player p) {
		this.winner = p;
		this.state = GameState.Finished;
		raceFinishTime = Time.time;
	}

	void OnGUI() {
		// Set GUI Skin
		GUI.skin = skin;

		// Draw Race Delay Countdown
		if (state == GameState.BeforeStart) {
			var style = new GUIStyle (GUI.skin.GetStyle ("label")) { fontSize = 32, alignment = TextAnchor.MiddleCenter };
			var startingInTime = PrepareTime - timeSinceStart + IntroTime;
			var label = String.Format ("Starting in {0}", (int)(startingInTime + 1f));

			if (startingInTime < (PrepareTime - 2f)) {
				var drawRect = new Rect (Screen.width / 2 - 200, Screen.height / 2 - 50, 400, 100);
				GUI.Box (drawRect, "");

				GUI.Label (drawRect, label, style);
			}
		}

		// Draw Position of players
		if (state == GameState.Racing) {
			DrawRank(player1);
			DrawRank(player2);
			DrawRounds (player1);
			DrawRounds (player2);
		}

		if (state == GameState.Finished) {
			var style = new GUIStyle (GUI.skin.GetStyle ("label")) { fontSize = 32, alignment = TextAnchor.MiddleCenter };
			var label = String.Format ("{0} wins!", winner.GetName());

			GUI.Box (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), "");

			GUI.Label (new Rect (Screen.width/2-100, Screen.height/2-50, 200, 100), label, style);
		}
	}

	private void DrawRank(Player p) {
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

	private void DrawRounds(Player p) {
		var cam = p.Cam;
		var camRect = cam.pixelRect; // origin is bottom left

		var upCenter = camRect.position + new Vector2(cam.pixelWidth / 2, cam.pixelHeight);
		upCenter.y = Screen.height - upCenter.y;

		var drawRect = new Rect (upCenter + new Vector2(5f - 80f / 2, 5f), new Vector2 (80f, 40f));
		var style = new GUIStyle (GUI.skin.GetStyle("label")) { fontSize = 16, alignment = TextAnchor.MiddleCenter };

		GUI.Box (drawRect, "");
		var label = String.Format ("Lap\n{0}/{1}", p.Rounds, RoundsToFinish);
		GUI.Label (drawRect, label, style);

	}
}
