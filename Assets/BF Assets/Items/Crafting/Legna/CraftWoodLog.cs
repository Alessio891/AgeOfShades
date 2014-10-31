using UnityEngine;
using System.Collections;

public class CraftWoodLog : BasicCraftable {

	protected override void Init ()
	{
		base.Init ();
		ItemName = "Legna";

		ItemIcon = "WoodLogIcon";
		AddIngredient (new ItemBastoncini (), 6);
		AddResult (new WoodLog ());

	}
 
}
