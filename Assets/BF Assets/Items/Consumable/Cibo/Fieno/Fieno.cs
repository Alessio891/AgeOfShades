using UnityEngine;
using System.Collections;

public class Fieno : BaseWeapon {

	protected override void Start ()
	{
		ItemName = "Fieno";
		ItemDescription = "Fieno: il cibo preferito dai conigli.";
		ItemIcon = "Fieno";
		Prefab = Resources.Load ("Fieno") as GameObject;
		CanStack = true;
	}

	public override void OnUse ()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerEquip> ().Equip (this);
	}

}
