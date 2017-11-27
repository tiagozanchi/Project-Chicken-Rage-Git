using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void GoToScreen()
	{
		AudioManager.instance.PlayOnce (9,0.5f);
		Vector3 screenPos = Camera.main.WorldToViewportPoint (transform.position);
		screenPos = new Vector3 ((screenPos.x * Screen.width) - (Screen.width/2) , (screenPos.y * Screen.height) - (Screen.height/2), 0);
		GameObject tempEggOrb = PoolManager.instance.getEggOrb ();
		tempEggOrb.transform.localPosition = screenPos;
		gameObject.SetActive (false);
	}

	void OnEnable()
	{
		Invoke ("GoToScreen",Random.Range(1f,3f));
	}

	void OnDisable()
	{
		CancelInvoke ();
	}
}
