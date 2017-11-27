using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerChick : Enemy {

	public int healAmount;

	public override void ActionOnHouse ()
	{
		base.ActionOnHouse ();
	}

	public override void Die ()
	{
		base.Die ();
	}

	void OnDisable()
	{
		CancelInvoke ();
	}

	public override void OnEnable()
	{
		InvokeRepeating ("HealAllies", 3f, 3f);
		base.OnEnable ();
	}

	void HealAllies()
	{
		Collider[] allies = Physics.OverlapSphere(transform.position, 10);

		foreach (Collider ally in allies)
		{
			if (ally.tag == "Enemy" && ally.gameObject.activeInHierarchy)
			{
				ally.SendMessageUpwards ("Heal", healAmount);
			}
		}
	}
}
