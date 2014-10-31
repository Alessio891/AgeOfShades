using UnityEngine;
using System.Collections;

[System.Serializable]
public class Fungo : InventoryItem {
	protected override void Start ()
	{
		ItemName = "Fungo";
		ItemIcon = "Fungo";
		ItemIconAtlas = "ItemIcons2";
		ItemDescription = "Un fungo comune nelle foreste. Ha poteri curativi.";
		Prefab = Resources.Load("FungoCurativo") as GameObject;
		Type = InventoryItemTypes.Consumable;
	}
	public override void OnUse() {
		GameHelper.GetLocalPlayer ().GetComponent<EntityStatus> ().Life += 1.5f;
		GameHelper.GetLocalPlayer ().GetComponent<PlayerInventory> ().ConsumeObject (this, 1);
		PlayerAnimations.Eat ();

	}

}
