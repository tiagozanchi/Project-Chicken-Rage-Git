using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalChick : Enemy {


	public override void ActionOnHouse ()
	{
		attacking = true;
		anim.SetBool ("attack", true);
		InvokeRepeating ("AttackHouse", 2f, 2f);
		Transform positionToMove = GameController.instance.GetDestructionPoint ();
		transform.position = positionToMove.position;
		transform.forward = positionToMove.forward;
		base.ActionOnHouse ();
	}

	public override void Die ()
	{
		base.Die ();
	}

	void AttackHouse()
	{
		GameController.instance.HouseTakeHit (damage);
	}
}
