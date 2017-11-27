using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveChick : Enemy {

	public override void ActionOnHouse ()
	{
		anim.SetBool ("explode", true);
		Invoke ("Explode", 3f);
		base.ActionOnHouse ();
	}

	public override void Die ()
	{
		AudioManager.instance.PlayOnce (10,2f);
		GameObject tempExplosion = PoolManager.instance.GetExplosion ();
		tempExplosion.transform.position = transform.position;
		base.Die ();
	}

	void Explode()
	{
		AudioManager.instance.PlayOnce (10,2f);
		GameController.instance.HouseTakeHit (damage);
		HitKill ();
	}
}
