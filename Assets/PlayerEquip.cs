using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerEquip : MonoBehaviour, ISerializedObject {

	public BaseWeapon RightHand;
	public BaseWeapon LeftHand;
	public EquippedArmor Armor = new EquippedArmor();
	string uid;

	// Use this for initialization
	void Start () {
		uid = GameHelper.GetUIDForObject (this);
		ItemDatabase.Init ();
		if (GameHelper.GameIsLoading)
			return;
		if (RightHand == null)
		{
			GetComponent<PlayerCombat> ().AttackRadius = 5;
			GetComponent<PlayerCombat> ().AttackDelay = 0.5f;
			GetComponent<PlayerCombat> ().AttackDamage = 0.5f;
			GetComponent<PlayerCombat>().SkillUsed = "Wrestling";
		}
	}

	void OnDeserialized()
	{
		if (Armor.Arms != null)
			Armor.Arms.OnUse ();
		if (Armor.Feet != null)
			Armor.Feet.OnUse ();
		if (Armor.Torso != null)
			Armor.Torso.OnUse ();
		if (Armor.Legs != null)
			Armor.Legs.OnUse ();
		if (Armor.Neck != null)
			Armor.Neck.OnUse();
		if (Armor.Hands != null)
			Armor.Hands.OnUse();
		if (Armor.Shirt != null)
			Armor.Shirt.OnUse();
		if (Armor.Pants != null)
			Armor.Pants.OnUse();
	}

	public void EquipArmor(BaseArmor armor, ArmorSlots ArmorSlot, Color c)
	{
		PlayerMeshManager m = (GameHelper.GetPlayerComponent<PlayerMeshManager> () as PlayerMeshManager);
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

	}

	public bool IsEquipped(BaseArmor armor)
	{
		foreach(BaseArmor a in Armor.list)
			if (a != null && a.ItemName == armor.ItemName)
				return true;

		return false;
	}

	public void UnEquipArmor(ArmorSlots ArmorSlot)
	{
		PlayerMeshManager m = (GameHelper.GetPlayerComponent<PlayerMeshManager> () as PlayerMeshManager);
		switch(ArmorSlot)
		{
		case ArmorSlots.Arms:
			m.Remove(ref m.Equip.Arms);
			Armor.Arms = null;
			break;
		case ArmorSlots.Feet:
			m.Swap( m.BodyRef.Feet, ref m.Skins.Feet, Color.white );
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

	// Update is called once per frame
	void Update () {
		if (GameHelper.GameIsLoading)
			return;
		if (RightHand != null)
		{
			if (!GameHelper.GetLocalPlayer().GetComponent<PlayerInventory>().Has( RightHand, 1))
			{
				UnEquip(0);
			}
		}
	}

	public GameObject GetRightHoldObject()
	{
		foreach(Transform t in GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Transform>())
		{
			if (t.name == "RightWeaponHold")
				return t.gameObject;
		}
		return null;
	}

	public void Equip(BaseWeapon item)
	{
		GameObject rwh = GetRightHoldObject();
		if (RightHand != null)
		{
			foreach(Transform t in rwh.GetComponentsInChildren<Transform>())
			{
				if (t.parent == rwh.transform)
				{
					Destroy(t.gameObject);
					break;
				}
			}
		}
		RightHand = item;
		GameObject newWeap = Instantiate (item.Prefab) as GameObject;
		newWeap.transform.parent = rwh.transform;
		newWeap.transform.localPosition = Vector3.zero;
		newWeap.transform.localEulerAngles = Vector3.zero;
		newWeap.transform.localScale = Vector3.one;
		GetComponent<PlayerCombat> ().AttackRadius = RightHand.AttackRange;
		GetComponent<PlayerCombat> ().AttackDelay = RightHand.AttackDelay;
		GetComponent<PlayerCombat> ().AttackDamage = RightHand.AttackDamage;
		GetComponent<PlayerCombat>().SkillUsed = RightHand.SkillUsed;
	}

	public void UnEquip(int hand)
	{
		if (hand == 0)
		{
			GameObject rwh = GetRightHoldObject();
			
			if (RightHand != null)
			{
				foreach(Transform t in rwh.GetComponentsInChildren<Transform>())
				{
					if (t.parent == rwh.transform)
					{
						Debug.Log("unequip");
						Destroy(t.gameObject);
						break;
					}
				}
			}
			GetComponent<PlayerCombat> ().AttackRadius = 5;
			GetComponent<PlayerCombat> ().AttackDelay = 0.5f;
			GetComponent<PlayerCombat> ().AttackDamage = 0.5f;
			GetComponent<PlayerCombat>().SkillUsed = "Wrestling";
			RightHand = null;
		}
		else if (hand == 1)
			LeftHand = null;
	}

	object[] serializedWeapons;
	public object[] Serialize()
	{
		object[] data = new object[11];
		data [0] = (RightHand != null) ? RightHand.ItemName : "";
		data [1] = (LeftHand != null) ? LeftHand.ItemName : "";
		int i = 2;
		foreach(BaseArmor b in Armor.list)
		{
			Debug.Log("PlayerEquip here: I'm deserializing " + b.GetType().ToString() + ".");
			data[i] =(b != null) ? b.ItemName : "";
			i++;
		}
		return data;
	}
	public void DeSerialize(object[] data)
	{
		serializedWeapons = data;
	}
	public void OnLoadingFinished()
	{
		List<InventoryEntry> inv = (GameHelper.GetPlayerComponent<PlayerInventory> () as PlayerInventory).Inventory;

		if ( (string)serializedWeapons[0] != "")
		{
			BaseWeapon r = (from i in inv where i.Item.ItemName == (string)serializedWeapons [0] select i).First ().Item as BaseWeapon;
			Equip (r);
		}
		if ( (string)serializedWeapons[1] != "")
		{
			BaseWeapon r = (from i in inv where i.Item.ItemName == (string)serializedWeapons [1] select i).First ().Item as BaseWeapon;
			Equip (r);
		}
		int y = 2;
		foreach(BaseArmor b in Armor.list)
		{
			string ba = (string)serializedWeapons[y];
			if (ba != "")
			{
				BaseArmor bas = (GameHelper.GetPlayerComponent<PlayerInventory>() as PlayerInventory).GetItem(ba) as BaseArmor;
				//EquipArmor(bas, bas.ArmorSlot, bas.Tint);
				bas.Equip();
			}
			y++;
		}
	}
	public string GetUID() { return uid; }



}

[System.Serializable]
public class SavedWeapon
{
	public System.Type WeaponType;
}

[System.Serializable]
public class EquippedArmor
{
	public BaseArmor Head;
	public BaseArmor Neck;
	public BaseArmor Torso;
	public BaseArmor Arms;
	public BaseArmor Hands;
	public BaseArmor Feet;
	public BaseArmor Legs;

	public BaseArmor Shirt;
	public BaseArmor Pants;

	public List<BaseArmor> list { get { return new List<BaseArmor>() { Head, Neck, Torso, Arms, Hands, Feet, Legs, Shirt, Pants }; } }
}