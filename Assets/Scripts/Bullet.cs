using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public enum Type {
		NORMAL, ICE
	}
	// Change time to destroy bullets here
	float destructTime = 5.0f;
	public GameObject explosion;
	public Type modifier;
	
	// Use this for initialization
	void Start () {
		Destroy(gameObject, destructTime);
	}
	
	// Update is called once per frame
	void Update () {
		
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
