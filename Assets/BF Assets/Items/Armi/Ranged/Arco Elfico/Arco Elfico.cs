using UnityEngine;
using System.Collections;

public class Arco_Elfico : BaseRanged {
	protected override void Start ()
	{
		ItemName = "Arco Elfico";
		Prefab = Resources.Load("Elven Long Bow") as GameObject;
		Type = InventoryItemTypes.Weapon;
		WeaponDamage = new WeaponDamageData (0.2f, 0.9f, "Wrestling", 10, Elements.Neutral);
	}

	public override void OnAttack (GameObject Target)
	{
		base.OnAttack (Target);
	}
}
