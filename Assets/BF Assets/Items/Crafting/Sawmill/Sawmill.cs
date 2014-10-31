using UnityEngine;
using System.Collections;

public class Sawmill : BasicWorktable {

	protected override void Init ()
	{
		base.Init ();
		CraftableHere.Add (new CraftWoodLog ());

	}
}
