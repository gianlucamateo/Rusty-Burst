using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public enum Type {
		NORMAL, ICE, ENGINE_STUN, HEAVY
	}
	// Change time to destroy bullets here
	float destructTime = 5.0f;
	public GameObject explosion;
	public Type modifier;
	public GameObject target;
	public Rigidbody rb;
	
	// Use this for initialization
	void Start () {
		Destroy(gameObject, destructTime);
		rb = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (modifier == Type.HEAVY) {
			rb.mass = 50000;
			this.transform.localScale = new Vector3 (10f, 10f, 10f);
		}
		if (target) {
			var magnitude = rb.velocity.magnitude;
			rb.velocity = (rb.velocity + 3*(target.transform.position - transform.position)).normalized * magnitude;
		}
	}

	void OnCollisionEnter(Collision col){
		var exp = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		Destroy (exp, 1f);
		var carContrl = col.gameObject.GetComponent<CarController> ();
		if(carContrl != null)
			carContrl.ActivateModifier(modifier);
		Destroy (gameObject);
	}
}
