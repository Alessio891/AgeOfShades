using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PaperdollMesh : MonoBehaviour {

	public PlayerMeshManager manager;// { get { return GetComponentInChildren<PlayerMeshManager> (); } }
	public bool changed = true;
	public EquippedArmor Armor = new EquippedArmor();
	// Use this for initialization
	void Start () {
		PlayerMeshManager playerManager = (PlayerMeshManager)GameHelper.GetPlayerComponent<PlayerMeshManager> ();
		PlayerEquip e = (PlayerEquip)GameHelper.GetPlayerComponent<PlayerEquip> ();
//		manager.Swap (e.Armor.Head, ref manager.Equip.Head);
//		manager.Swap (e.Armor.Feet, ref manager.Equip.Feet);
//		manager.Swap (e.Armor.Arms, ref manager.Equip.Arms);	
	}

	public void ChangeSkinColor (Color c)
	{

	}

	public void UnEquipArmor(ArmorSlots ArmorSlot)
	{
		PlayerMeshManager m = manager;
		switch(ArmorSlot)
		{
		case ArmorSlots.Arms:
			m.Remove(ref m.Equip.Arms);
			Armor.Arms = null;
			break;
		case ArmorSlots.Feet:
			m.Swap( m.BodyRef.Feet, ref m.Skins.Feet, Color.white);
			Armor.Feet = null;
			break;
		case ArmorSlots.Hands:
			m.Remove(ref m.Equip.Hands);
			Armor.Hands = null;
			break;
		case ArmorSlots.Head:
			m.Remove(ref m.Equip.Head);
			Armor.Head = null;
			break;
		case ArmorSlots.Legs:
			m.Remove(ref m.Equip.Legs);
			Armor.Legs = null;
			break;
		case ArmorSlots.Neck:
			m.Remove(ref m.Equip.Neck);
			Armor.Neck = null;
			break;
		case ArmorSlots.Torso:
			m.Remove(ref m.Equip.Torso);
			Armor.Torso = null;
			break;
		case ArmorSlots.Shirt:
			m.Swap(m.BodyRef.Torso, ref m.Skins.Torso, Color.white);
			Armor.Shirt = null;
			break;
		case ArmorSlots.Pants:
			m.Swap(m.BodyRef.Legs, ref m.Skins.Legs, Color.white);
			Armor.Pants = null;
			break;
		}
	}


	public void EquipArmor(BaseArmor armor, ArmorSlots ArmorSlot, Color c)
	{
		PlayerMeshManager m = manager;
		switch(ArmorSlot)
		{
		case ArmorSlots.Arms:
			m.Swap (armor.Prefab, ref m.Equip.Arms, c);
			Armor.Arms = armor;
			break;
		case ArmorSlots.Feet:
			m.Swap (armor.Prefab, ref m.Skins.Feet, c);
			Armor.Feet = armor;
			break;
		case ArmorSlots.Hands:
			m.Swap (armor.Prefab, ref m.Equip.Hands, c);
			Armor.Hands = armor;
			break;
		case ArmorSlots.Head:
			m.Swap (armor.Prefab, ref m.Equip.Head, c);
			Armor.Head = armor;
			break;
		case ArmorSlots.Legs:
			m.Swap (armor.Prefab, ref m.Equip.Legs, c);
			Armor.Legs = armor;
			break;
		case ArmorSlots.Neck:
			m.Swap (armor.Prefab, ref m.Equip.Neck, c);
			Armor.Neck = armor;
			break;
		case ArmorSlots.Torso:
			m.Swap (armor.Prefab, ref m.Equip.Torso, c);
			Armor.Torso = armor;
			break;
		case ArmorSlots.Shirt:
			m.Swap(armor.Prefab, ref m.Skins.Torso, c);
			Armor.Shirt = armor;
			break;
		case ArmorSlots.Pants:
			m.Swap(armor.Prefab, ref m.Skins.Legs, c);
			Armor.Pants = armor;
			break;
		}

		if (armor is BaseArmor)
		{
			List<GameObject> s = (from o in GameObject.FindGameObjectsWithTag("PaperdollSlot") where o.GetComponent<PaperdollEquip>().Slot == ArmorSlot select o).ToList();
			foreach(GameObject g in s)
			{
				g.GetComponent<PaperdollEquip>().Item = armor;
			}
		}
		
	}
	// Update is called once per frame
	void Update () {

	}
}
