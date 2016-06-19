using UnityEngine;
using System.Collections;

public class CameraCoordinator : MonoBehaviour {
	public enum CameraState { BeforeStart, Racing, AfterFinish }
	public CameraState cameraState = CameraState.BeforeStart;

	public CarController car1, car2;

	public Camera cam;
	public GameManager gameManager;

	public float camHeight = 10.0f;

	// Use this for initialization
	void Start () {
		cam = gameObject.AddComponent<Camera> ();
	 	gameManager = GetComponent<GameManager> ();

		car1 = gameManager.player1.Car;
		car2 = gameManager.player2.Car;
	}

	// Update is called once per frame
	void Update () {
		if (cameraState == CameraState.Racing) {
			cam.enabled = false;
			return;
		}

		// Activate coordinator cam
		cam.enabled = true;
			
		// Animate camera
		if (cameraState == CameraState.BeforeStart) {
			var carCenter = (car1.transform.position + car2.transform.position) / 2f;
			cam.transform.position = carCenter + new Vector3 (10f, 10f, 10f);
			cam.transform.LookAt (carCenter);
			cam.transform.RotateAround (carCenter, Vector3.up, 1.0f);
		} else if (cameraState == CameraState.AfterFinish) {
			Player winner;

			if (gameManager.winner != null)
				winner = gameManager.winner;
			else
				winner = gameManager.player1;
			
			var targetPos = winner.Car.transform.position;
			cam.transform.position = targetPos + new Vector3 (10f, 10f, 10f);
			cam.transform.LookAt (targetPos);
			cam.transform.RotateAround (targetPos, Vector3.up, 1.0f);
		}
	}
}
