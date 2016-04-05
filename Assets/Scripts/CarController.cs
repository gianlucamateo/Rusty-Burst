using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour {
	public List<AxleInfo> axleInfos; // the information about each individual axle
	public float maxMotorTorque; // maximum torque the motor can apply to wheel
	public float maxSteeringAngle; // maximum steer angle the wheel can have


	public void Start(){
		axleInfos[0].leftWheel.gameObject.GetComponent<WheelCollider>().ConfigureVehicleSubsteps(5f, 50, 50);
	}


	public void FixedUpdate()
	{
		float motor = maxMotorTorque * (Input.GetAxis("Triggers")-Input.GetAxis("Vertical"));
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				if (motor > 30) {
					axleInfo.leftWheel.brakeTorque = motor * axleInfo.brakeScale ;
					axleInfo.rightWheel.brakeTorque = motor * axleInfo.brakeScale ;
					axleInfo.leftWheel.motorTorque = 0;
					axleInfo.rightWheel.motorTorque = 0;
				} else {
					axleInfo.leftWheel.brakeTorque = 0;
					axleInfo.rightWheel.brakeTorque = 0;
					axleInfo.leftWheel.motorTorque = -motor * axleInfo.motorScale;
					axleInfo.rightWheel.motorTorque = -motor* axleInfo.motorScale;
				}

			}
		}

	}
}

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor; // is this wheel attached to motor?
	public bool steering; // does this wheel apply steer angle?
	public float motorScale;
	public float brakeScale;
}