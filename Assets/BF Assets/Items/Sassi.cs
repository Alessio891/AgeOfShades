using UnityEngine;
using System.Collections;

public class Sassi : InventoryItem {
	protected override void Start ()
	{
		ItemName = "Sassi";
		Prefab = Resources.Load("Roccia") as GameObject;
		Type = InventoryItemTypes.Basic;
	}
	public override void OnUse() {

	}
}
