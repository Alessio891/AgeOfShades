using UnityEngine;
using System.Collections;

public class Chiave : InventoryItem {
	protected override void Start ()
	{
		ItemName = "Chiave";
		Prefab = Resources.Load("null") as GameObject;
		Type = InventoryItemTypes.Key;
	}
	public override void OnUse() {

	}
}
