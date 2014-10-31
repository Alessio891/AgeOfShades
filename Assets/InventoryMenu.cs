using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventoryMenu : MonoBehaviour{

	public GameObject Grid;
	public GameObject DescriptionPanel;
	public int Slots = 15;
	public int maxSlotsPerRow = 4;

	public int count = 0;

	public bool UpdateNow = false;

	// Use this for initialization
	void Start () {
		GameHelper.HideMenu (gameObject);
	}
	public bool Visible { get { return GetComponentInChildren<Graphic> ().enabled; } }


	public Vector3 GetPositionFor(int slot)
	{
		Vector3 value = Vector3.zero;

		int slotY = Mathf.RoundToInt(slot / maxSlotsPerRow);
		// slot - ( (maxSlotsPerRow * slotY) + slotY )
		int slotX = slot % maxSlotsPerRow;

		value = new Vector3( -109 + (50 * slotX), 224 - (50 * slotY), 0);


		return value;
	}

	int lastSlotCount = 0;
	List<GameObject> SlotsObjects = new List<GameObject>();
	void Update()
	{
		if (lastSlotCount != Slots || UpdateNow)
		{
			foreach(GameObject o in SlotsObjects)
				Destroy(o);
			SlotsObjects.Clear();
			UpdateNow = false;
			for(int i = 0; i < Slots; i++)
			{
				GameObject s = Instantiate( Resources.Load("ItemSlot") ) as GameObject;
				s.transform.parent = Grid.transform;
				s.transform.localPosition = GetPositionFor(i);
				s.transform.localScale = Vector3.one;
				SlotsObjects.Add(s);
				if (GameHelper.GetLocalPlayer() != null)
				{
					PlayerInventory inventory = GameHelper.GetLocalPlayer().GetComponent<PlayerInventory>();
					if (inventory != null)
					{
						if (i < inventory.Inventory.Count)
						{
							s.GetComponent<InventorySlot>().Item = inventory.Inventory[i].Item;
						}

					}
				}
				else
				{
					s.GetComponent<InventorySlot>().Item = new Ascia();
				}
			}
		}

		lastSlotCount = Slots;
	}

	public Vector3 GetAvailablePosition()
	{
		Vector3 _ret = new Vector3 (-326, 26731 + (count * 55), 0);
		return _ret;
	}

	public GameObject GetPanelWithItem(InventoryItem item)
	{
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("PickUpPanel"))
		{
	
		}
		return null;
	}

	public List<InventorySlot> GetSlotsWithItem(InventoryItem Item)
	{
		List<InventorySlot> slots = new List<InventorySlot> ();
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("InventorySlot"))
		{
			if (o.GetComponent<InventorySlot>().Item != null && o.GetComponent<InventorySlot>().Item.ItemName == Item.ItemName)
			{
				slots.Add( o.GetComponent<InventorySlot>() );
			}
		}
		return slots;
	}

}
