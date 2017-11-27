using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour {

	public int damage;
	public float speed;
	private Transform target;

	// Use this for initialization
	void Start () {
		Invoke("Die", 10f);
	}

	// Update is called once per frame
	void Update () {
		if(target.gameObject.activeInHierarchy)
		{
			transform.LookAt(target);
			transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
		}
		else
		{
			Die();
		}
	}

	public void SetTarget(Transform targetToTarget)
	{
		target = targetToTarget;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag.Equals("Enemy") && other.gameObject.activeInHierarchy && other.transform == target)
		{
			Action(other);
		}
	}

	void Die()
	{
		Destroy(gameObject);
	}

	public virtual void Action(Collider other)
	{
		Destroy (gameObject);
	}
}
