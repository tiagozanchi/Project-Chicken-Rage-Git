using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCube : TowerProjectile {

	public override void Action(Collider other)
	{
		AudioManager.instance.PlayOnce (4);
		other.SendMessageUpwards ("Poisoned", damage);
		base.Action (other);
	}
}
