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
		other.gameObject.GetComponentInParent<CarController> ().modifier = this.modifier;
	}

}
