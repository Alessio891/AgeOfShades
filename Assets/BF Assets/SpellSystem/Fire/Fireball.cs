using UnityEngine;
using System.Collections;

public class Fireball : GenericTargetSpell {

	// Use this for initialization
	void Start () {
		this._name = "Fireball";
		description = "Asd"; 

		reagents.Add (new Reagent () { name = "Zolfo" });
	}

	public override void OnTargetSelect (object target)
	{
		base.OnTargetSelect (target);
		(GameHelper.GetPlayerComponent<ClickToMove> () as ClickToMove).Targeting = false;

		if (!fizzled)
		{
			Vector3 targetPoint = (Vector3)target;
			transform.position = targetPoint;
			effect.enableEmission = true;
			effect.Play ();
			foreach(BasicEntity e in GameHelper.GetEntityNearPoint(targetPoint, 10))
			{
				if (e is BasicNPC)
				{
					GameHelper.ShowNotice("OUCH!", e.gameObject);
				}
				else
				{
					e.Damage(0.2f);
				}
			}
		}
		fizzled = false;
		StartCoroutine (disable (1));

	}
}
