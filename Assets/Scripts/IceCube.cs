using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCube : TowerProjectile {

	public override void Action(Collider other)
	{
		AudioManager.instance.PlayOnce (3);
		other.SendMessageUpwards ("Freeze", damage);
		base.Action (other);
	}
}
