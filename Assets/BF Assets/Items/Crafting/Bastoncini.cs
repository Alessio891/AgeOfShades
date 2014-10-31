using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bastoncini : BasicCraftable {

	protected override void Init ()
	{
		AddIngredient (new WoodLog ());
		AddResult (new ItemBastoncini (), 3);
		AddRequirement( "Lumberjacking", 12 );
		//CraftingResult.Add (new ItemBastoncini());
		CraftedNear = "WorkingDesk";
		ItemIcon = "Bastoncini";
		ItemName = "Bastoncini";
	}

}
