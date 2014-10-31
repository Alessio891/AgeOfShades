using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BaseArmor : InventoryItem {

	public ArmorSlots ArmorSlot;

	public BaseArmor() {
		Start ();
	}

	protected override void Start ()
	{

	}

	public virtual void ApplyStats()
	{
	}

	public virtual void RemoveStats()
	{
	}

	public virtual void Equip()
	{
		PlayerEquip e = (GameHelper.GetPlayerComponent<PlayerEquip>() as PlayerEquip);
		GameObject pc = GameObject.Find ("PaperdollCharacter");
		
		PaperdollMesh m = (pc != null) ? GameObject.Find ("PaperdollCharacter").GetComponent<PaperdollMesh> () : null;
		Equip (e, m);
	}

	public virtual void UnEquip()
	{
		PlayerEquip e = (GameHelper.GetPlayerComponent<PlayerEquip>() as PlayerEquip);
		GameObject pc = GameObject.Find ("PaperdollCharacter");
		
		PaperdollMesh m = (pc != null) ? GameObject.Find ("PaperdollCharacter").GetComponent<PaperdollMesh> () : null;
		UnEquip (e, m);
	}

	public virtual void Equip(PlayerEquip e, PaperdollMesh m)
	{
		e.EquipArmor(this, ArmorSlot, Tint.Color);
		m.EquipArmor(this, ArmorSlot, Tint.Color);
		ApplyStats();
	}
	public virtual void UnEquip(PlayerEquip e, PaperdollMesh m)
	{
		e.UnEquipArmor(ArmorSlot);
		m.UnEquipArmor(ArmorSlot);
		RemoveStats();
	}

	public override void OnUse ()
	{
		PlayerEquip e = (GameHelper.GetPlayerComponent<PlayerEquip>() as PlayerEquip);
		GameObject pc = GameObject.Find ("PaperdollCharacter");

		PaperdollMesh m = (pc != null) ? GameObject.Find ("PaperdollCharacter").GetComponent<PaperdollMesh> () : null;
		if (e.IsEquipped(this))
		{
			UnEquip(e, m);
		}
		else
		{
			Equip(e,m);
		}

	}

	public void EquipInRightSlot()
	{

	}
}

public enum ArmorSlots
{
	Head = 0,
	Neck,
	Torso,
	Arms,
	Hands,
	Feet,
	Legs,
	Shirt,
	Pants
}