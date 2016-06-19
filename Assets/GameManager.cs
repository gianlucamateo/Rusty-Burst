using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	#region SetFromInspector
	public TrackManager trackManager;
	public float IntroTime = 10f;
	public float OutroTime = 5f;
	#endregion

	public CameraCoordinator camCoordinator;

	public enum GameState { BeforeStart, Racing, AfterFinish }

	public Player player1, player2;
	public GameState state = GameState.BeforeStart;

	private float startTime, raceStartTime, raceFinishTime;
	public Player winner = null;

	void Awake() {
		camCoordinator = GetComponent<CameraCoordinator> ();
		camCoordinator.cameraState = CameraCoordinator.CameraState.BeforeStart;
	}

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		var elapsed = Time.time - startTime;

		if (elapsed < IntroTime) {
			state = GameState.BeforeStart;

			if (elapsed > IntroTime / 2f)
				camCoordinator.cameraState = CameraCoordinator.CameraState.Racing;
		} else {
			if (winner == null) {
				state = GameState.Racing;
				camCoordinator.cameraState = CameraCoordinator.CameraState.Racing;
			} else {
				state = GameState.AfterFinish;
				camCoordinator.cameraState = CameraCoordinator.CameraState.AfterFinish;
				raceFinishTime = Time.time;
			}
		}
			
	}

	void NotifyWinner(Player p) {
		this.winner = p;
	}
}
