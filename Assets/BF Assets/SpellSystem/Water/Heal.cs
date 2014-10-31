using UnityEngine;
using System.Collections;

public class Heal : EntityTargetSpell {

	public override void OnTargetSelect (object target)
	{
		base.OnTargetSelect (target);
		(GameHelper.GetPlayerComponent<ClickToMove> () as ClickToMove).Targeting = false;
		if (!fizzled)
		{
			GameObject o = (target as GameObject);
			if (o.GetComponent<EntityStatus>() != null)
			{
				Vector3 targetPoint = o.transform.position;
				transform.position = targetPoint;
				effect.enableEmission = true;
				effect.Play ();
				o.GetComponent<EntityStatus>().Life += 1;
				o.GetComponent<EntityStatus>().ApplyBuff(new HealOverTime());
			}
			else
			{
				GameHelper.SystemMessage("Non puoi curare " + o.name + "!", Color.red);
			}
		}
		
		fizzled = false;
		StartCoroutine (disable (effect.duration));


	}
}
