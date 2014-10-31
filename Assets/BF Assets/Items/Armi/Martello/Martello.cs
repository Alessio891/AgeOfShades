using UnityEngine;
using System.Collections;

public class Martello : BaseWeapon {

	public Martello()
	{
	}

	protected override void Start ()
	{
		ItemName = "Martello";
		ItemDescription = "Utensile";
		ItemIcon = "Bastoncini";
		CanPlace = false;
		CanStack = false;
		SkillUsed = "Corpo a corpo";
		Prefab = Resources.Load ("Martello") as GameObject;
	}
	
	public override void OnUse ()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerEquip> ().Equip (this);
	}
}
