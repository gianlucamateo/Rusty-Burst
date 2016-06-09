using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int playerId;

	public string PowerAxis, SteeringAxis;

	public float InputPower = 0.0f;
	public float InputSteering = 0.0f;
	public bool InputReset = false;

	public CarController Car;

	private bool frozen = false;

	// Use this for initialization
	void Start () {
		PowerAxis = "Joy" + (playerId + 1) + "Power";
		SteeringAxis = "Joy" + (playerId + 1) + "Steering";

		Car = GetComponent<CarController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (frozen)
			return;
		
		InputSteering = Input.GetAxis (SteeringAxis);
		InputReset = Input.GetKeyDown (playerId == 0 ? KeyCode.R : KeyCode.Period);

		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			InputPower = Input.GetAxis (PowerAxis + "Win");
		else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
			InputPower = shift(Input.GetAxis (PowerAxis + "ForwardMac")) - shift(Input.GetAxis (PowerAxis + "ReverseMac"));
		else if (Application.platform == RuntimePlatform.LinuxPlayer)
			InputPower = Input.GetAxis (PowerAxis + "ForwardLinux") - Input.GetAxis (PowerAxis + "ReverseLinux");
	}

	public void Freeze() {
		frozen = true;
		Car.Freeze ();
	}

	public void UnFreeze() {
		frozen = false;
		Car.UnFreeze ();
	}

	private float shift(float input) {
		return (input + 1f) / 2f;
	}
}
