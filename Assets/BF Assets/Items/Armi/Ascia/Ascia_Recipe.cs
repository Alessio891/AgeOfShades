using UnityEngine;
using System.Collections;

public class Ascia_Recipe : BasicCraftable {

	protected override void Init ()
	{
		base.Init ();

		AddIngredient (new ItemBastoncini(), 3);
		AddIngredient (new Sassi(), 2);

		AddResult (new Ascia (), 1);

		AddRequirement("Lumberjacking", 0);

		ItemIcon = "Ascia";
		ItemIconAtlas = "ItemIcons";
		ItemName = "Ascia";

		CraftedNear = "NoRequirement";
	}
}
