using UnityEngine;
using System.Collections;

public class Ascia : BaseWeapon {

	protected override void Start ()
	{

		ItemName = "Ascia";
		ItemIcon = "Ascia";
		CanPlace = false;
		CanStack = false;
		Prefab = Resources.Load ("Ascia") as GameObject;
		AttackRange = 9;
		AttackDelay = 1;
		AttackDamage = 2;
		SkillUsed = "Corpo a corpo";

		WeaponDamage = new WeaponDamageData (1.5f, 3f, "Lumberjacking", 15);

	}

	public override void OnUse ()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerEquip> ().Equip (this);
	}
}
