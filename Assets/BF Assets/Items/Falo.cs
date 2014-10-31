using UnityEngine;
using System.Collections;

public class Falo : BasicWorktable {
	protected override void Init ()
	{
		base.Init ();
		CraftableHere.Add (new Carne_Recipe ());
	}
}
