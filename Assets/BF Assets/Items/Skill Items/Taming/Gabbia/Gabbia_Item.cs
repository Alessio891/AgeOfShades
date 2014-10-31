using UnityEngine;
using System.Collections;

public class Gabbia_Item : InventoryItem {
protected override void Start ()
	{
		ItemName = "Gabbia";
		ItemDescription = "Gabbia usata per catturare i conigli.";
		ItemIcon = "Gabbia";
		Prefab = Resources.Load ("Gabbia") as GameObject;
		CanPlace = true;
		CanStack = false;

	}

	public override void OnUse ()
	{
		Drop ();
	}
}
