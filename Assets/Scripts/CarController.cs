using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour {
	public List<AxleInfo> axleInfos; // the information about each individual axle
	public float maxMotorTorque; // maximum torque the motor can apply to wheel
	public float maxSteeringAngle; // maximum steer angle the wheel can have
	public float ratio;
	public Rigidbody chassis;
	public AudioSource carAudio;
	private int lastSkidRearLeft = -1;
	private int lastSkidFrontLeft = -1;
	private int lastSkidRearRight = -1;
	private int lastSkidFrontRight = -1;
	private JointSpring spring, backup;
	private float maxRPM = 2900f;
	public Skidmarks skidmarks;
	public Vector3 worldPose;
	private float slip;
	public float skidSlip;
	public List<GameObject> brakeLights;
	public List<GameObject> tyres;
	private bool inAir;
	public bool iceTyres = false, stunEngine = false;
	public float rpm, BaseDrag;
	public ParticleSystem smoke;
	private Color tyreBaseColor;
	private WheelFrictionCurve frontBaseSide,frontBaseForward, rearBaseSide, rearBaseForward;
	public float dragScale = 1.0f;

	private Dictionary<Bullet.Type,System.Action> actionDict;


	public void Start(){
		axleInfos[0].leftWheel.gameObject.GetComponent<WheelCollider>().ConfigureVehicleSubsteps(5f, 50, 50);
		this.tyreBaseColor = tyres [0].GetComponent<Renderer> ().material.GetColor ("_Color");
		this.frontBaseForward = axleInfos [1].leftWheel.forwardFriction;
		this.frontBaseSide = axleInfos [1].leftWheel.sidewaysFriction;

		this.rearBaseForward = axleInfos [0].leftWheel.forwardFriction;
		this.rearBaseSide = axleInfos [0].leftWheel.sidewaysFriction;
		this.backup = axleInfos [0].leftWheel.suspensionSpring;

		actionDict = new Dictionary<Bullet.Type,System.Action>(){
			{Bullet.Type.NORMAL, () =>{}},
			{Bullet.Type.HEAVY, () =>{}},
			{Bullet.Type.ICE, ActivateIceTyres},
			{Bullet.Type.ENGINE_STUN, ActivateEngineStun},
		};
	}
		
	public void ActivateModifier(Bullet.Type type){
		actionDict [type] ();
	}

	private void ActivateEngineStun(){
		stunEngine = true;
		StartCoroutine (deactivateEngineStun());
	}
	private IEnumerator deactivateEngineStun(){
		yield return new WaitForSeconds (10f);
		stunEngine = false;
	}

	private void ActivateIceTyres(){
		iceTyres = true;
		StartCoroutine (deactivateIceTyres());
	}
	private IEnumerator deactivateIceTyres(){
		yield return new WaitForSeconds (10f);
		iceTyres = false;
	}

	private float GetSteering(){
		return GetComponent<Player> ().InputSteering;
	}

	private float GetPower(){
		return GetComponent<Player> ().InputPower;
	}

	public void Freeze() {
		chassis.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
	}

	public void UnFreeze() {
		chassis.constraints = RigidbodyConstraints.None;	
	}

	public void FixedUpdate()
	{
		float forwards = GetPower ();
		float steeringInput = GetSteering ();
		inAir = !Physics.Raycast (this.transform.position, -this.transform.up, 1f);
		float motor = (maxMotorTorque + dragScale) * forwards;
		if (stunEngine) motor /= 3;


		float steering = maxSteeringAngle * steeringInput;

		handleIceTyres (iceTyres);



		foreach (AxleInfo axleInfo in axleInfos) {
			rpm = axleInfo.leftWheel.rpm;
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				if (Mathf.Abs(axleInfo.leftWheel.rpm) > maxRPM) {
					motor = 0;
				}
				if (motor > 30) {
					axleInfo.leftWheel.brakeTorque = (chassis.velocity.magnitude > 0.3 && axleInfo.leftWheel.rpm > 0) ? motor * axleInfo.brakeScale : 0 ;
					axleInfo.rightWheel.brakeTorque = (chassis.velocity.magnitude > 0.3 && axleInfo.rightWheel.rpm > 0) ? motor * axleInfo.brakeScale : 0 ;
					axleInfo.leftWheel.motorTorque = -400;
					axleInfo.rightWheel.motorTorque = -400;
					foreach (GameObject brakeLight in brakeLights) {
						brakeLight.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.red);
					}
				} else {
					axleInfo.leftWheel.brakeTorque = 0;
					axleInfo.rightWheel.brakeTorque = 0;
					axleInfo.leftWheel.motorTorque = -motor * axleInfo.motorScale;
					axleInfo.rightWheel.motorTorque = -motor* axleInfo.motorScale;
					foreach (GameObject brakeLight in brakeLights) {
						brakeLight.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
					}
				}
			}
			if (inAir) {
				moveAir(forwards,steeringInput);
			}
			if (!iceTyres) {
				checkSkids (axleInfo);
			}

		}

		float rpms = axleInfos[0].leftWheel.rpm;

		ratio =  rpms/2900f;

		carAudio.pitch = 2*ratio + 0.3f;

		chassis.drag = gameObject.GetComponent<Rigidbody>().velocity.magnitude * gameObject.GetComponent<Rigidbody>().velocity.magnitude * BaseDrag * dragScale;

	}

	private void handleIceTyres(bool iceTyres){
		if (iceTyres) {
			foreach (AxleInfo axleInfo in axleInfos) {
				WheelFrictionCurve curve = axleInfo.leftWheel.forwardFriction;
				curve.stiffness = 1f;
				axleInfo.leftWheel.forwardFriction = curve;
				curve = axleInfo.rightWheel.forwardFriction;
				curve.stiffness = 1f;
				axleInfo.rightWheel.forwardFriction = curve;

				curve = axleInfo.leftWheel.sidewaysFriction;
				curve.stiffness = 1f;
				axleInfo.leftWheel.sidewaysFriction = curve;
				curve = axleInfo.rightWheel.sidewaysFriction;
				curve.stiffness = 1f;
				axleInfo.rightWheel.sidewaysFriction = curve;
			}

			foreach (GameObject tyre in tyres) {
				tyre.GetComponent<Renderer> ().material.SetColor ("_Color", new Color (0.75f, 0.95f, 1f));
			}
		} else {
			axleInfos [1].leftWheel.forwardFriction = this.frontBaseForward;
			axleInfos [1].leftWheel.sidewaysFriction = this.frontBaseSide;

			axleInfos [1].rightWheel.forwardFriction = this.frontBaseForward;
			axleInfos [1].rightWheel.sidewaysFriction = this.frontBaseSide;

			axleInfos [0].leftWheel.forwardFriction = this.rearBaseForward;
			axleInfos [0].leftWheel.sidewaysFriction = this.rearBaseSide;	

			axleInfos [0].rightWheel.forwardFriction = this.rearBaseForward;
			axleInfos [0].rightWheel.sidewaysFriction = this.rearBaseSide;

			foreach (GameObject tyre in tyres) {
				tyre.GetComponent<Renderer> ().material.SetColor ("_Color", this.tyreBaseColor);
			}
		}
	}

	private void moveAir(float forward, float steering){
		chassis.AddRelativeTorque (-forward*300,steering*300,0);
	}

	private void checkSkids(AxleInfo axleInfo){
		worldPose = new Vector3 ();
		Quaternion quat = new Quaternion ();
		axleInfo.leftWheel.GetWorldPose (out worldPose, out quat);
		worldPose.y -= 0.35f;
		WheelHit wh;
		axleInfo.leftWheel.GetGroundHit (out wh);
		slip = Mathf.Abs (wh.forwardSlip) + Mathf.Abs (wh.sidewaysSlip);
		if (axleInfos.IndexOf (axleInfo) == 0) {
			if (slip > skidSlip)
				lastSkidFrontLeft = skidmarks.AddSkidMark (worldPose, Vector3.up, slip, lastSkidFrontLeft);
			else
				lastSkidFrontLeft = -1;
		}
		else {
			if (slip > skidSlip) {
				ParticleSystem.EmissionModule em = smoke.emission;
				em.enabled = true;
				lastSkidRearLeft = skidmarks.AddSkidMark (worldPose, Vector3.up, slip, lastSkidRearLeft);
			} else {
				ParticleSystem.EmissionModule em = smoke.emission;
				em.enabled = false;
				lastSkidRearLeft = -1;
			}
		}

		axleInfo.rightWheel.GetWorldPose (out worldPose, out quat);
		worldPose.y -= 0.35f;
		axleInfo.rightWheel.GetGroundHit (out wh);
		slip = Mathf.Abs (wh.forwardSlip) + Mathf.Abs (wh.sidewaysSlip);
		if (axleInfos.IndexOf (axleInfo) == 0) {
			if (slip > skidSlip)
				lastSkidFrontRight = skidmarks.AddSkidMark (worldPose, Vector3.up, slip, lastSkidFrontRight);
			else
				lastSkidFrontRight = -1;
		}
		else {
			if (slip > skidSlip) 
				lastSkidRearRight = skidmarks.AddSkidMark (worldPose, Vector3.up, slip, lastSkidRearRight);
			else
				lastSkidRearRight = -1;
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