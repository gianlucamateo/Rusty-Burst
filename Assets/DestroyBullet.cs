using UnityEngine;
using System.Collections;

public class DestroyBullet : MonoBehaviour {

	// Change time to destroy bullets here
	float destructTime = 5.0f;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Destroy(gameObject, destructTime);
	}
}
