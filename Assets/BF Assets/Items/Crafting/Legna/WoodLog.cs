using UnityEngine;
using System.Collections;

public class WoodLog : InventoryItem {

	// Use this for initialization
	protected override void Start () {
		ItemName = "Legna";
		ItemDescription = "Legna comune utilizzata per costruire strumenti base.";
		CanStack = true;
		CanPlace = true;
		ItemIcon = "WoodLogIcon";
		Prefab = Resources.Load ("BasicWoodLog") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnUse ()
	{
		base.OnUse ();
		Drop ();
	}

	[RPC]
	void Dropped(Vector3 position)
	{

	}
}
