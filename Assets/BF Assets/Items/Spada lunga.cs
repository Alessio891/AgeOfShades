using UnityEngine;
using System.Collections;
[System.Serializable]
public class Spada_lunga : BaseWeapon {
	protected override void Start ()
	{
		ItemName = "Spada lunga";
		Prefab = Resources.Load("LongSword") as GameObject;
		ItemIcon = "Ascia";
		Type = InventoryItemTypes.Weapon;
		SkillUsed = "Corpo A Corpo";
		WeaponDamage = new WeaponDamageData (0.2f, 1.5f, "Corpo A Corpo", 35);
	}
	public override void OnUse() {
		GameHelper.GetLocalPlayer ().GetComponent<PlayerEquip> ().Equip (this);
	}
}
