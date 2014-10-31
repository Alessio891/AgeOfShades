using UnityEngine;
using System.Collections;

public class Sciabola : BaseWeapon {
	protected override void Start ()
	{
		ItemName = "Sciabola";
		Prefab = Resources.Load("Sciabola_Low") as GameObject;
		Type = InventoryItemTypes.Basic;
		WeaponDamage = new WeaponDamageData (0.5f, 2, "Swordsmanship", 25);
	}
	public override void OnUse() {
		GameHelper.GetLocalPlayer ().GetComponent<PlayerEquip> ().Equip (this);
	}
}
