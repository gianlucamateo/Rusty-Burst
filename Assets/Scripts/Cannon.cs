using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cannon : MonoBehaviour {
	public GameObject projectilePrefab;
	public GameObject Cockpit;
	public GameObject Gun;
	public Rigidbody ChassisRigidB;
	public Animation fireAnim;
	public KeyCode up, down, left, right, fireKey;
	public Texture2D reticle;
	public Texture2D ice, fat, engine, empty;
	public Texture2D targetFrame, targetAq, targetFill;
	public Camera carCam;
	public Vector3 basePos, posOnScreen, iconPosOnScreen;
	public CarController carCtrl;
	public GameObject hitObj;
	public LayerMask mask, mask2;
	public float lockPercentage = 0f;
	public GameObject lockObj;

	private Dictionary<Bullet.Type,Texture2D> imgDict;
	private Player player;

	private bool fire = true;

	static float RETICLE_SIZE = 40f;
	static float TARGET_WIDTH = 95f;
	static float TARGET_HEIGHT = 20f;

	// Use this for initialization
	void Start () {
		ice = new Texture2D (1024,1024);
		ice.LoadImage (System.IO.File.ReadAllBytes("Assets/textures/IceBullet.png"));

		fat = new Texture2D (1024,1024);
		fat.LoadImage (System.IO.File.ReadAllBytes("Assets/textures/fatty.png"));

		engine = new Texture2D (1024,1024);
		engine.LoadImage (System.IO.File.ReadAllBytes("Assets/textures/EngineStunBullet.png"));

		empty = new Texture2D (1024,1024);
		empty.LoadImage (System.IO.File.ReadAllBytes("Assets/textures/empty.png"));

		targetFrame = new Texture2D (1024,1024);
		targetFrame.LoadImage (System.IO.File.ReadAllBytes("Assets/textures/frame.png"));

		targetAq = new Texture2D (1024,1024);
		targetAq.LoadImage (System.IO.File.ReadAllBytes("Assets/textures/target_acquired.png"));

		targetFill = new Texture2D (1024,1024);
		targetFill.LoadImage (System.IO.File.ReadAllBytes("Assets/textures/target_fill.png"));

		imgDict = new Dictionary<Bullet.Type, Texture2D>(){
			{Bullet.Type.NORMAL, empty},
			{Bullet.Type.HEAVY, fat},
			{Bullet.Type.ICE, ice},
			{Bullet.Type.ENGINE_STUN, engine},
		};

		carCtrl = transform.gameObject.GetComponentInParent<CarController> ();
		player = GetComponentInParent<Player> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		

		RaycastHit hitInfo;

		mask = 1 << 8;
		Physics.Raycast (transform.position + 0.25f * transform.up , transform.forward, out hitInfo, 1000f, mask);

		if (hitInfo.collider != null)
			lockObj = hitInfo.collider.gameObject;
		
		if (hitInfo.collider) {
			lockPercentage += 0.5f;
			lockPercentage = Mathf.Min(125f,lockPercentage);
		} else {
			lockPercentage -= 1;
			lockPercentage = Mathf.Max(0f,lockPercentage);
		}

		Debug.DrawRay(transform.position+ 0.25f*transform.up, transform.forward*1000,Color.white);
		//if (Input.GetButtonDown("Fire1")) {
		if (Input.GetKey(fireKey) && fire){
			StartCoroutine(Fire());
    		}

		if (Input.GetKey(left)) {
			Cockpit.transform.Rotate(0.0f, -1.0f, 0.0f);
		}

		if (Input.GetKey(right)) {
			Cockpit.transform.Rotate(0.0f, 1.0f, 0.0f);
		}

		float xtrans = Gun.transform.localEulerAngles.x;

		if (Input.GetKey(down)) {
			if(xtrans >= 320 || xtrans <= 30)
				Gun.transform.Rotate(-1.0f, 0.0f, 0.0f);
		}

		if (Input.GetKey(up)) {
			if(xtrans >= 310 || xtrans <= 20)
				Gun.transform.Rotate(1.0f, 0.0f, 0.0f);
		}
	}

	void OnGUI() {
		var gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		if (gameManager.state == GameManager.GameState.Racing) {
			DrawReticle();
			DrawModifier();
		}
	}

	private void DrawModifier() {
		var modifier = player.ActiveModifier;
		var texture = imgDict [modifier];

		var cam = player.Cam;
		var camRect = cam.pixelRect; // origin is bottom left

		var upRight = camRect.position + new Vector2(cam.pixelWidth, cam.pixelHeight);
		upRight.y = Screen.height - upRight.y;

		var drawRect = new Rect (upRight + new Vector2(-5f - 40f, 5f), new Vector2 (40f, 40f));
		var texRect = new Rect(drawRect.position + new Vector2(5f, 5f), new Vector2(30f, 30f));
		var textRect = new Rect (drawRect.position + new Vector2 (0f, 40f), new Vector2 (40f, 40f));

		GUI.Box (drawRect, "");
		GUI.DrawTexture (texRect, texture);
		GUI.Label (textRect, modifier.ToString());
	}
		
	private void DrawReticle() {
		RaycastHit rcHit;
		mask2 = ~(mask | 1<<2);
		var rayCast = Physics.Raycast(transform.position + 0.2f*transform.up, transform.forward,out rcHit, 1000f, mask2);

		if (rcHit.collider != null)
			hitObj = rcHit.collider.gameObject;
		else
			hitObj = null;

		posOnScreen = carCam.WorldToScreenPoint(transform.position + 0.2f*transform.up + 1000f*transform.forward)- new Vector3(RETICLE_SIZE / 2f,-RETICLE_SIZE / 2f,0f);

		iconPosOnScreen = posOnScreen + new Vector3(RETICLE_SIZE*0.8f,-RETICLE_SIZE*0.8f);

		if (rayCast) {
			posOnScreen = carCam.WorldToScreenPoint (rcHit.point) - new Vector3(RETICLE_SIZE / 2f, -RETICLE_SIZE / 2f ,0f);
		}

		GUI.DrawTexture (new Rect (posOnScreen.x, Screen.height - posOnScreen.y, RETICLE_SIZE, RETICLE_SIZE), reticle);

		var lockIndicatorPos = new Vector2(posOnScreen.x,posOnScreen.y) + new Vector2 (0, -50);

		GUI.DrawTexture (new Rect (lockIndicatorPos.x -TARGET_WIDTH/2 + RETICLE_SIZE/2, Screen.height - lockIndicatorPos.y, TARGET_WIDTH * (Mathf.Min(lockPercentage,100)/100), TARGET_HEIGHT),targetFill);
		if (lockPercentage >= 100f) {
			GUI.Label (new Rect (lockIndicatorPos.x -TARGET_WIDTH/2 + RETICLE_SIZE/2, Screen.height - lockIndicatorPos.y, TARGET_WIDTH, TARGET_HEIGHT),"TARGET LOCK");
		}
	}

	IEnumerator Fire(){
		fire = false;
		Vector3 spawnPos = transform.position + new Vector3 (0.0f, 0.2f, 0.0f);
		GameObject projectile = Instantiate(projectilePrefab, spawnPos, transform.rotation) as GameObject;
       		 //projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 3000));
		projectile.GetComponent<Bullet>().modifier = player.ActiveModifier;
		player.ActiveModifier = Bullet.Type.NORMAL;
		var projectileRB = projectile.GetComponent<Rigidbody>();
		projectileRB.velocity = ChassisRigidB.velocity;
		projectileRB.AddRelativeForce(Vector3.forward * 120, ForceMode.Impulse);
		projectileRB.mass = 50;
		projectileRB.useGravity = false;
		if (lockPercentage > 100f) {
			projectile.GetComponent<Bullet> ().target = lockObj;
			lockPercentage = 0f;
		}
		var globalDir = transform.TransformVector(Vector3.right);

		ChassisRigidB.AddTorque (-globalDir * 9000);

		// change firing rate here
		yield return new WaitForSeconds(0.5f);
		fire = true;
		fireAnim.Play ();
	}
}
