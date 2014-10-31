using UnityEngine;
using System.Collections;

public class QuickSlots : MonoBehaviour {
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.UpArrow)) 
			ActivateSlot("top");
		else
			DeactivateSlot("top");

		if (Input.GetKeyDown (KeyCode.DownArrow)) 
			ActivateSlot("bot");
		else
			DeactivateSlot("bot");

		if (Input.GetKeyDown (KeyCode.RightArrow)) 
			ActivateSlot("right");
		else
			DeactivateSlot("right");
		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
			ActivateSlot("left");
		else
			DeactivateSlot("left");
	}

	void ActivateSlot(string name)
	{
		if (GetChild (name).GetComponentInChildren<QuickSlotButton> ().Slot == null)
			return;

		GetChild (name).GetComponentInChildren<QuickSlotButton> ().Slot.OnUse ();
		InventoryItem item = GetChild (name).GetComponentInChildren<QuickSlotButton> ().Slot;
		InventoryMenu inventory = GameObject.Find ("InventoryPanel").GetComponent<InventoryMenu> ();
		byte consumed = 0;
		bool theresanotherslot = false;
		//player.GetComponent<PlayerInventory>().ConsumeObject( item, 1 );
		if (inventory.GetSlotsWithItem(item).Count <= 0)
		{
			string itemname = GetChild (name).GetComponentInChildren<QuickSlotButton> ().Slot.ItemName;
			foreach(Transform t in GetComponentsInChildren<Transform>())
			{
				if (t.GetComponent<QuickSlotButton>() != null)
				{
					InventoryItem i = t.GetComponent<QuickSlotButton>().Slot;
					if (i != null && i.ItemName == itemname)
					{
						t.GetComponent<QuickSlotButton>().Slot = null;
					}
					    
				}
			}
		}
	}
	void DeactivateSlot(string name)
	{
		//GetChild (name).GetComponentInChildren<UISprite> ().color = Color.white;
	}

	GameObject GetChild(string name)
	{
		foreach(Transform t in GetComponentsInChildren<Transform>())
		{
			if (t.name == name)
				return t.gameObject;
		}
		return null;
	}
}
