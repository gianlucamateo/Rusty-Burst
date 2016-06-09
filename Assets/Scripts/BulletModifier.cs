using UnityEngine;
using System.Collections;

public class BulletModifier : MonoBehaviour {
	public Bullet.Type modifier;
	public bool active = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider other){
		if (other.isTrigger || !active)
			return;
		CarController carCtrl = other.gameObject.GetComponentInParent<CarController> ();
		if (carCtrl != null) {
			carCtrl.modifier = this.modifier;
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
