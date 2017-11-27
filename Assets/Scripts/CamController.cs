using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CamController : MonoBehaviour {

	private FreeLookCam lookScript;

	// Use this for initialization
	void Start () {
		lookScript = GetComponent<FreeLookCam> ();
		lookScript.SetTurnSpeed (0);
		Invoke ("EnableTurn", 5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			lookScript.enabled = true;
		}

		if(Input.GetMouseButtonUp (1)) {
			lookScript.enabled = false;
		}
	}

	void DisableCam()
	{
		lookScript.enabled = false;
	}

	void EnableTurn()
	{
		lookScript.SetTurnSpeed (1.5f);
		DisableCam ();
	}
}
