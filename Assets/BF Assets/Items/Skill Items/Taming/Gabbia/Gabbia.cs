using UnityEngine;
using System.Collections;

public class Gabbia : BasicItem {

	public bool FoodInside = false;
	public bool Captured = false;
	public GameObject capturedWhat;
	public int Uses = 3;

	protected override void Init ()
	{
		base.Init ();
		ItemName = "Gabbia";
		CanPickup = false;
		gameObject.tag = "Gabbia";
	}

	public override void OnUse ()
	{

		if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquip>().RightHand != null &&
		    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquip>().RightHand.ItemName == "Fieno")
		{
			if (!FoodInside)
			{
				FoodInside = true;
				GameHelper.ShowNotice("Hai messo il cibo nella gabbia.");
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().ConsumeObject( ItemDatabase.Items[typeof(Fieno)], 1 );
				if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().Has( ItemDatabase.Items[typeof(Fieno)]))
				{
					GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquip>().UnEquip(0);
				}
				return;
			}
		}


		if (Captured)
		{
			GameHelper.ShowNotice("Hai preso un " + capturedWhat.name + "!");
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().PickupObject( new Carne(), 2 );
			Uses--;
			if (Uses <= 0)
			{
				if (PhotonNetwork.offlineMode)
				{
					Destroy(gameObject);	
				}
				else if (!PhotonNetwork.offlineMode && PhotonNetwork.inRoom)
				{
					PhotonNetwork.Destroy(gameObject);
				}
			}
			if (PhotonNetwork.offlineMode)
			{
				Destroy(capturedWhat);
			}
			else
			{
				if (PhotonNetwork.inRoom)
				{
					PhotonNetwork.Destroy(capturedWhat);
				}
			}
			Captured = false;
		}
		else
		{
			GameHelper.ShowNotice("La gabbia è vuota...");
		}
	}
}
