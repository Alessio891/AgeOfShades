using UnityEngine;
using System.Collections;

public class Carne : InventoryItem {
	protected override void Start ()
	{
		base.Start ();
		ItemName = "Carne";
		ItemIcon = "MeatIcon";
		ItemIconAtlas = "ItemIcons";
		CanStack = true;
		CanPlace = false;
		ItemDescription = "Carne appena cucinata. Diminuisce notevolmente la fame.";
	}

	public override void OnUse ()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<EntityStatus> ().Hunger -= 3;
		if (GameObject.FindGameObjectWithTag ("Player").GetComponent<EntityStatus> ().Hunger < 0)
			GameObject.FindGameObjectWithTag ("Player").GetComponent<EntityStatus> ().Hunger = 0;

		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerInventory> ().ConsumeObject (this, 1);
	}
}
