using UnityEngine;
using System.Collections;

public class BulletModifier : MonoBehaviour {
	public Bullet.Type modifier;
	public bool active = true;
	float baseY;
	// Use this for initialization
	void Start () {
		baseY = transform.position.y;
		Color color = Color.white;
		switch (modifier) {
		case Bullet.Type.ICE:
			color = Color.cyan;
			break;
		case Bullet.Type.HEAVY:
			color = Color.green;
			break;
		case Bullet.Type.ENGINE_STUN:
			color = Color.red;
			break;
		}
		var renderer = GetComponent<Renderer> ();
		renderer.material.color = color;
		renderer.material.SetColor ("_EmissionColor", color);
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		transform.position = new Vector3(transform.position.x,baseY + Mathf.Sin (2*Time.time),transform.position.z);

	}

	void OnTriggerEnter(Collider other){
		if (other.isTrigger || !active)
			return;
		var player = other.gameObject.GetComponentInParent<Player> ();
		if (player != null) {
			player.ActiveModifier = this.modifier;
			active = false;
			gameObject.GetComponent<Renderer> ().enabled = false;
			StartCoroutine (reactivate ());
		}
	}

	private IEnumerator reactivate(){
		yield return new WaitForSeconds (10f);
		gameObject.GetComponent<Renderer> ().enabled = true;	
		active = true;
	}

}
