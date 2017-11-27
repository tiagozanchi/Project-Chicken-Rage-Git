using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour {

	public float seconds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		Invoke ("Disable", seconds);
	}

	void Disable()
	{
		gameObject.SetActive (false);
	}
}
