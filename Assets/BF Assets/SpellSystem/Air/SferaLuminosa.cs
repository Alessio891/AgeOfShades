using UnityEngine;
using System.Collections;

public class SferaLuminosa : GenericTargetSpell {

	public GameObject LightOrb;

	public override void OnTargetSelect (object target)
	{
		base.OnTargetSelect (target);
		(GameHelper.GetPlayerComponent<ClickToMove> () as ClickToMove).Targeting = false;
		if (!fizzled)
		{
			Vector3 targetPoint = (Vector3)target;
			GameObject o = Instantiate(LightOrb) as GameObject;
			o.transform.position = targetPoint + Vector3.up;
			Destroy(o, 10);
		}

	}
}
