using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCube : TowerProjectile {

	public override void Action(Collider other)
	{
		AudioManager.instance.PlayOnce (1);
		other.SendMessageUpwards ("TakeHit", damage);
		base.Action (other);
	}
    
}
