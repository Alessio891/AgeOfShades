using UnityEngine;
using System.Collections;

public class WorkingDesk : BasicWorktable {

	protected override void Init ()
	{
		base.Init ();

		CraftableHere.Add( new Bastoncini() );
		//CraftableHere.Add (new Recinto_CraftRecipe() );
		CraftableHere.Add ( new Gabbia_CraftingRecipe() );

	}

}
