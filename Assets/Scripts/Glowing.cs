using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glowing : MonoBehaviour {

    public Material glow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        glow.color = Color.Lerp(Color.white, Color.gray, Mathf.PingPong(Time.time,1f));
    }
}
