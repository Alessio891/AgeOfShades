using UnityEngine;
using System.Collections;

public class Falo_spento : InventoryItem {
	protected override void Start ()
	{
		ItemName = "Falo spento";
		Type = InventoryItemTypes.Basic;
		ItemDescription = "Tutto il necessario per poter accendere un fuoco.";
	}
	public override void OnUse() {
		Drop ();
		GameHelper.GetLocalPlayer ().GetComponent<PlayerInventory> ().ConsumeObject (this);
	}
}
