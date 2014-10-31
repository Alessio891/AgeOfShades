using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CastTimeSpell : BaseSpell {

	public float BaseCastingTime = 0;
	protected float castingTimer = 0;
	bool castEnabled = false;
	protected bool weCanCast = false;
	Vector3 lastCasterPosition;	
	public void UpdateCastingBar()
	{
		Caster.CastingBar.fillAmount = (castingTimer / BaseCastingTime);
	}

	IEnumerator castingTimeCheck()
	{
		Caster.IsCasting = true;
		bool interrupted = false;
		while(castingTimer < BaseCastingTime)
		{
			if (Caster.Transform.position != lastCasterPosition)
			{
				if (CanCastWhileWalking)
					castingTimer += 0.05f;
				else
				{
					GameHelper.HideMenu(Caster.CastingBar.transform.parent.gameObject);
					castingTimer = 0;
					Caster.IsCasting = false;
					weCanCast = false;
					interrupted = true;
					break;
				}
			}
			else
				castingTimer += 0.1f;
			lastCasterPosition = Caster.Transform.position;
			UpdateCastingBar();
			yield return new WaitForSeconds(0.1f);
		}
		if (!interrupted)
		{
			GameHelper.HideMenu(Caster.CastingBar.transform.parent.gameObject);
			castingTimer = 0;
			castEnabled = true;
			weCanCast = true;
			Caster.IsCasting = false;
			Caster.ConsumeMP(requiredMP);
			actualCast = true;
			Cast (Caster);
			PronunceWords ();
		}
		yield return false;
	}

	bool actualCast = false;
	public override void Cast (ICaster caster)
	{
		base.Cast (caster);
		weCanCast = true;

		if (Caster.IsCasting)
		{
			weCanCast = false;
			return;
		}
		if (!actualCast && !Caster.IsCasting)
		{
			if (!CanCast())
			{
				weCanCast = false;
				return;
			}

		}
		if (BaseCastingTime > 0) {
			if (!castEnabled)
			{
				GameHelper.ShowMenu(Caster.CastingBar.transform.parent.gameObject);
				lastCasterPosition = Caster.Transform.position;
				weCanCast = false;
				StartCoroutine(castingTimeCheck());
				return;
			}
		}
		else
		{
			Caster.IsCasting = false;
			actualCast = false;
		}
	}

}
