using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

[System.Serializable]
public class InventoryItem {

	public string ItemName;
	public string ItemDescription;
	public GameObject Prefab;

	public bool CanStack = false;
	public int MaxStackSize = 10;

	public bool CanPlace = false;

	public string ItemIconAtlas = "ItemIcons";
	public string ItemIcon;

	public BasicCraftable Craftable;

	public float Weight = 1;

	// USED ONLY IF ITEM IS A WEAPON
	public float AttackRange = 3;
	public float AttackDelay = 3;
	public float AttackDamage = 1;
	public WeaponDamageData WeaponDamage;

	public string SkillUsed = "Lumberjacking";

	public InventoryItemTypes Type = InventoryItemTypes.Basic;

	//public TextAsset XMLFile;

	public SerializedColor Tint = new SerializedColor(Color.white);

	// Use this for initialization
	public InventoryItem()
	{
		Start ();
		
	//	ReadXML ();
	}

	protected virtual void Start()
	{
	}



	public virtual void Drop()
	{
		PlayerInventory player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerInventory> ();
		if (Prefab != null)
		{
			GameObject o = PhotonNetwork.Instantiate(Prefab.name, GameObject.FindGameObjectWithTag("Player").transform.position, 
			                                         Quaternion.identity, 0);//player.InstantiateObject (Prefab);
			Debug.Log(Prefab.name);
			Vector3 p = o.transform.position;
			p.y = Terrain.activeTerrain.SampleHeight(p);
			o.transform.position = p;
		}
		OnRemove ();
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerInventory> ().ConsumeObject (this, 1);
	}
	public virtual void OnUse()
	{

	}

	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnPickup()
	{
	}
	public virtual void OnRemove()
	{
	}
}

public enum InventoryItemTypes
{
	Basic = 0,
	Key,
	Consumable,
	Weapon,
	Armor,
	Reagent,
	Other
}
