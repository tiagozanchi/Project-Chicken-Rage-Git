using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilboard : MonoBehaviour {

	private GameObject target;

	// Use this for initialization
	void Start () {
		target = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (target.activeInHierarchy == false) {
			target = Camera.main.gameObject;
		}

		Vector3 dir = target.transform.position - transform.position;
		transform.transform.forward = -dir;
    }
}
