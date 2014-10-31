using UnityEngine;
using System.Collections;

public class ChestItem : BasicItem {

	protected override void Init ()
	{
		base.Init ();

		ItemName = "Baule";
	}

	public override void OnUse ()
	{
		base.OnUse ();
		GameHelper.ShowNotice ("Il baule è vuoto...", GameHelper.GetLocalPlayer ());
	}
}
