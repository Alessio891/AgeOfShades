using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Corpse : BasicItem {

	public LootPack loot;
	public Container LootContainer;
	public List<InventoryItem> itemList;

	protected override void Init ()
	{
		base.Init ();
		ItemName = "Cadavere";
		ActivationDistance = 20;
		CanPickup = false;
		if (loot == null)
		{
			loot = new LootPack();
			//loot.AddLootItem(typeof(Fungo), 3, 1);
		}
		foreach(KeyValuePair<System.Type, int> p in loot.GetRandomizedLoot())
		{
			for(int i = 0; i < p.Value; i++)
				itemList.Add( (InventoryItem)System.Activator.CreateInstance(p.Key) );
		}
	}

	public override void OnUse ()
	{
		//base.OnUse ();
		if (LootContainer == null)
			LootContainer = GameHelper.ShowCorpse (itemList, this);

	}

	void Update()
	{
		if (itemList.Count <= 0)
		{
			Destroy(gameObject);
		}
	}

}
