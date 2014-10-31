using UnityEngine;
using System.Collections;

public class ItemBastoncini : InventoryItem {

	public ItemBastoncini()
	{
	}

	protected override void Start ()
	{
		base.Start ();
		ItemName = "Bastoncini";
		ItemIcon = "Bastoncini";
		ItemDescription = "Dei pezzi di legno ricavati da un tronco. Utili per costruire vari utensili.";
		CanStack = true;
	}

}
