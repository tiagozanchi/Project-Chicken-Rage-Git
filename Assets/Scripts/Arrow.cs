using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public int damage;
    private Rigidbody rb;
    private BoxCollider bc;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!rb.isKinematic)
        {
            transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime * 10);
        }
    }

	private void OnCollisionEnter(Collision other)
    {
		int audioImpact = 1;

        if(other.gameObject.activeInHierarchy)
        {
            if (other.transform.tag == "Enemy")
            {
                other.transform.SendMessageUpwards("TakeHit", damage);
                gameObject.SetActive(false);
            }
            else if (other.transform.tag == "WeakSpot")
            {
                audioImpact = 2;
                other.transform.SendMessageUpwards("TakeHit", damage * 3);
                gameObject.SetActive(false);
            }
            else if (other.transform.tag == "HitKill")
            {
                audioImpact = 2;
                other.transform.SendMessageUpwards("HitKill");
                gameObject.SetActive(false);
            }
        }
		
		AudioManager.instance.PlayOnce (audioImpact);
        bc.enabled = false;
        rb.isKinematic = true;
    }
}
