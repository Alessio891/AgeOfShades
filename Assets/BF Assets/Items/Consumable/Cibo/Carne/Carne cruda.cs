using UnityEngine;
using System.Collections;

public class Carne_cruda : InventoryItem {
	protected override void Start ()
	{
		ItemName = "Carne cruda";
		Prefab = Resources.Load("Coniglio") as GameObject;
		Type = InventoryItemTypes.Basic;
	}
	public override void OnUse() {

	}
}
