using UnityEngine;
using System.Collections;

public class CameraCoordinator : MonoBehaviour {
	private CarController car1, car2;

	private Camera cam;
	private GameManager gm;

	public float camHeight = 10.0f;

	// Use this for initialization
	void Start () {
		cam = gameObject.AddComponent<Camera> ();
	 	gm = GetComponent<GameManager> ();

		car1 = gm.player1.Car;
		car2 = gm.player2.Car;
	}

	// Update is called once per frame
	void Update () {
		if (gm.state == GameManager.GameState.Racing || gm.state == GameManager.GameState.BeforeStart) {
			cam.enabled = false;
			return;
		}

		// Activate coordinator cam
		cam.enabled = true;
			
		// Animate camera
		if (gm.state == GameManager.GameState.Intro) {
			var carCenter = (car1.transform.position + car2.transform.position) / 2f;
			cam.transform.position = carCenter + new Vector3 (8f, 8f, 8f);
			cam.transform.LookAt (carCenter);
			cam.transform.RotateAround (carCenter, Vector3.up, 10f * Time.time);
		} else if (gm.state == GameManager.GameState.Finished) {
			Player winner;

			if (gm.winner != null)
				winner = gm.winner;
			else
				winner = gm.player1;
			
			var targetPos = winner.Car.transform.position;
			cam.transform.position = targetPos + new Vector3 (10f, 10f, 10f);
			cam.transform.LookAt (targetPos);
			cam.transform.RotateAround (targetPos, Vector3.up, 10f * Time.time);
		}
	}
}
