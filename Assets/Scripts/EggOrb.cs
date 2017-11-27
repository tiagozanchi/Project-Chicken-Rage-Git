using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggOrb : MonoBehaviour {

	private Transform target;

	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag ("OrbTarget").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, target.position, 200f * Time.deltaTime);

		if (Vector3.Distance (transform.position, target.position) < 1f) {
			AudioManager.instance.PlayOnce (8);
			GameController.instance.UpdateResources (3);
			gameObject.SetActive (false);
		}
	}
}
