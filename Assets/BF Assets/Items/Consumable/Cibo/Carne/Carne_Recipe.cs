using UnityEngine;
using System.Collections;

public class Carne_Recipe : BasicCraftable {

	protected override void Init ()
	{
		base.Init ();
		ItemName = "Carne cotta";
		ItemIconAtlas = "ItemIcons";
		ItemIcon = "MeatIcon";

		AddIngredient( ItemDatabase.Items[ typeof(Carne_cruda) ], 1 );
		AddResult (new Carne ());

		CraftedNear = "Fuoco";
		AddRequirement ("Cooking", 5);
	}
}
