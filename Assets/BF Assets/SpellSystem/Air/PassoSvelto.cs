using UnityEngine;
using System.Collections;

public class PassoSvelto : EntityTargetSpell {

	public float SpeedIncrease = 4;

	public override void OnTargetSelect (object target)
	{
		base.OnTargetSelect (target);

		if ( (target as GameObject).GetComponent<ClickToMove>() != null)
		{
			(target as GameObject).GetComponent<EntityStatus>().ApplyBuff(new PassoSveltoBuff(SpeedIncrease));
		}
	}
}
