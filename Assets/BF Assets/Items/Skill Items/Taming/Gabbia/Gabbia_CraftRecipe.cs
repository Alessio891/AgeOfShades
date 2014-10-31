using UnityEngine;
using System.Collections;

public class Gabbia_CraftingRecipe : BasicCraftable {

	protected override void Init ()
	{
		base.Init ();
		Tab = CraftingTabs.Spell;
		ItemName = "Gabbia";
		ItemIcon = "Gabbia";
	//	AddIngredient (new ItemBastoncini(), 5);
		AddIngredient (new WoodLog (), 2);
		//Addingredient corda
		AddResult (new Gabbia_Item(), 1);

		CraftedNear = "WorkingDesk";
		AddRequirement ("Lumberjacking", 10.0f);	
	}
}
