using UnityEngine;
using System.Collections;

public class BulletModifier : MonoBehaviour {
	public Bullet.Type modifier;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other){
		CarController carCtrl = other.gameObject.GetComponentInParent<CarController> ();
		if(carCtrl != null)
			carCtrl.modifier = this.modifier;
	}

}
