using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class PlayerInventory : MonoBehaviour, ISerializedObject {

	public bool _AnimationReachedPointOfPickUp = false;
	public GameObject Selected;
	public string uid;

	public void AnimationReachedPointOfPickUp()
	{
		_AnimationReachedPointOfPickUp = true;
	}

	IEnumerator ResetPointOfPickUp()
	{
		yield return new WaitForSeconds(0.2f);
		_AnimationReachedPointOfPickUp = false;
	}

	public int MaxItems = 33;
	public List<InventoryEntry> Inventory = new List<InventoryEntry>();
	public InventoryItem LastPickedupItem;

	public List<GameObject> InventorySlots = new List<GameObject> ();

	public string[] startItems = new string[0];
	public int _size = 0;

	public int GetAmountOf(System.Type itemType)
	{
		foreach(InventoryEntry e in Inventory)
		{
			if (e.Item.GetType() == itemType)
			{
				return e.Amount;
			}
		}
		return -1;
	}

	public bool HasMaterials(BasicCraftable item)
	{
		foreach(InventoryItem i in item.Ingredients)
		{
			if (!Has(i, item.IngredientAmounts[ item.Ingredients.IndexOf(i) ]))
			{
				return false;
			}
		}
		return true;
	}

	public void ConsumeObject(string item, int _amount = 1)
	{
		int amount = _amount;
		int consumed = 0;
		Debug.Log ("Sto per consumare " + amount + " " + item);
		int count = 0;
		foreach(InventoryEntry slot in Inventory)
		{
			if (slot.Item.ItemName == item)
			{
				int remainder = amount -slot.Amount;
				slot.Amount -= amount;
				if (slot.Amount <= 0)
				{
					slot.Remove = true;
				}
				consumed = amount - remainder;
				amount -= consumed;
				count++;
				if (consumed <= 0 || amount <= 0 || remainder <= 0)
					break;
			}
		}

		foreach(InventoryEntry e in (from a in Inventory where a.Remove == true select a))
			e.Item.OnRemove();

		Inventory = (from o in Inventory where o.Remove == false select o).ToList ();
		
		if (GetComponent<PlayerEquip>().RightHand != null && GetComponent<PlayerEquip>().RightHand.ItemName == item)
		{
			GetComponent<PlayerEquip>().UnEquip(0);
		}
		
		GameObject.Find ("Inventory").GetComponent<InventoryMenu> ().UpdateNow = true;

	}


	public void ConsumeObject(InventoryItem item, int _amount = 1)
	{
		int amount = _amount;
		int consumed = 0;
		Debug.Log ("Sto per consumare " + amount + " " + item.ItemName);
		int count = 0;
		foreach(InventoryEntry slot in Inventory)
		{
			if (slot.Item.ItemName == item.ItemName)
			{
				int remainder = amount -slot.Amount;
				slot.Amount -= amount;
				if (slot.Amount <= 0)
				{
					slot.Remove = true;
				}
				consumed = amount - remainder;
				amount -= consumed;
				count++;
				if (consumed <= 0 || amount <= 0 || remainder <= 0)
					break;
			}
		}

		Inventory = (from o in Inventory where o.Remove == false select o).ToList ();

		if (GetComponent<PlayerEquip>().RightHand != null && GetComponent<PlayerEquip>().RightHand.ItemName == item.ItemName)
		{
			GetComponent<PlayerEquip>().UnEquip(0);
		}

		GameObject.Find ("Inventory").GetComponent<InventoryMenu> ().UpdateNow = true;
	}

	void UpdateCraftingLists()
	{
	}

	public bool PickupObject(InventoryItem inventoryItem, int amount = 1, bool showPanel = true)
	{
		// Contiamo quanti oggetti abbiamo...
		GameObject.Find ("Inventory").GetComponent<InventoryMenu> ().UpdateNow = true;
		int count = Inventory.Count - 1;
		// Stiamo portando gli oggetti massimi?
		if (count >= MaxItems)
		{
			// Se l'oggetto non può stackare, comunica che non può essere preso l'oggetto
			if (!inventoryItem.CanStack)
				return false;

			// Cerchiamo tutti gli slot con l'oggetto in questione, dato che può stackare

			bool canPickup = false;
			foreach(InventoryEntry s in Inventory)
			{
				if (s.Amount < s.Item.MaxStackSize)
				{
					canPickup = true;
					break;
				}
			}

			if (!canPickup)
				return false;

		}

		bool placed = false;
		foreach(InventoryEntry slot in Inventory)
		{
			// Lo slot contiene già quest'oggetto?
			if (slot.Item != null && slot.Item.ItemName == inventoryItem.ItemName)
			{
				if (inventoryItem.CanStack)
				{
					if ( (slot.Amount + amount) < slot.Item.MaxStackSize)
					{
						slot.Amount += amount;
						placed = true;
						Debug.Log("stack " + amount);
						if (showPanel)
							ShowPickUpPanel(inventoryItem, amount);
						LastPickedupItem = inventoryItem;
						inventoryItem.OnPickup();
						break;
					}
				}
			}
		}
		
		if (!placed)
		{
			InventoryEntry entry = new InventoryEntry();
			entry.Item = inventoryItem;
			entry.Amount = 1;
			Inventory.Add(entry);
			inventoryItem.OnPickup();
		}

		return true;
	}

	void ShowPickUpPanel(InventoryItem item, int amount)
	{
		/*
		if (LastPickedupItem == null || LastPickedupItem.ItemName != item.ItemName)
		{
			GameObject o = GameObject.Instantiate( Resources.Load("PickUpPanel") ) as GameObject;
			o.transform.parent = GameObject.Find("InventoryAnchor").transform;
			o.GetComponent<UIAnchor>().uiCamera = GameObject.Find("InventoryCamera").GetComponent<Camera>();
			o.transform.localPosition = Vector3.zero; 
			o.GetComponentInChildren<UIPanel>().transform.position = new Vector3(0, 48 * (GameObject.Find("InventoryPanel").GetComponent<InventoryMenu>().count - 1), 0);
			o.transform.localScale = Vector3.one;
			o.GetComponentInChildren<PickUpPanel>().item = item;
			GameObject.Find("InventoryPanel").GetComponent<InventoryMenu>().count++;
			UnityEngine.Debug.Log("New panel");	
			foreach(Transform t in o.GetComponentsInChildren<Transform>())
			{
				if (t.name == "Number")
				{
					if (amount > 1)
					{
						o.GetComponentInChildren<PickUpPanel>().amount = amount;
						t.GetComponent<UILabel>().alpha = 1;
						UnityEngine.Debug.Log(o.GetComponentInChildren<PickUpPanel>().amount.ToString());
						t.GetComponent<UILabel>().text = o.GetComponentInChildren<PickUpPanel>().amount.ToString() + "x";
					}
					else
					{
						t.GetComponent<UILabel>().alpha = 0;
						o.GetComponentInChildren<PickUpPanel>().amount = amount;
					}
					break;
				}
			}
			
		}
		else
		{
			UnityEngine.Debug.Log("Old panel");
			GameObject s = GameObject.Find("InventoryPanel").GetComponent<InventoryMenu>().GetPanelWithItem(item);
			if (s != null)
			{
				foreach(Transform t in s.GetComponentsInChildren<Transform>())
				{
					if (t.name == "Number")
					{
						t.GetComponent<UILabel>().alpha = 1;
						s.GetComponentInChildren<PickUpPanel>().amount += amount;
						UnityEngine.Debug.Log(s.GetComponentInChildren<PickUpPanel>().amount.ToString());
						t.GetComponent<UILabel>().text = s.GetComponentInChildren<PickUpPanel>().amount.ToString() + "x";
						s.GetComponentInChildren<PickUpPanel>().destroy = false;
						break;
					}
				}
				
			}
			
		}*/
	}

	public bool Has(InventoryItem Item, int amount = 1)
	{
		List<InventorySlot> i = GameObject.Find ("Inventory").GetComponent<InventoryMenu> ().GetSlotsWithItem (Item);
		if (i.Count <= 0)
			return false;
		foreach(InventorySlot s in i)
		{
			if (s.stack >= amount)
				return true;
		}
		if (i.Count >= amount)
			return true;
		return false;
	}

	public bool Has(string item, int amount = 1)
	{
		List<InventoryEntry> i = (from o in Inventory where o.Item.ItemName == item select o).ToList ();
		if (i.Count <= 0)
			return false;
		foreach(InventoryEntry s in i)
		{
			if (s.Amount >= amount)
				return true;
		}
		if (i.Count >= amount)
			return true;
		return false;
	}

	public void DropAll()
	{
		Inventory.Clear ();
	}
	BaseArmor shirt;
	BaseArmor pants;
	BaseArmor Boots;
	// Use this for initialization
	void Start () {
		uid = GameHelper.GetUIDForObject (this);
		 shirt = new SimpleShirt();
		 pants = new SimplePants ();
		 Boots = new Boots ();
		//SpellBookItem sb = new SpellBookItem ();
		//PickupObject (sb, 1, false);
		InventorySlots = (from tr in GameObject.Find ("Inventory").GetComponentsInChildren<Transform> () where tr.tag == "InventorySlot" select tr.gameObject).ToList ();
		if (GameObject.FindObjectOfType(typeof(LoadCoroutine)) != null)
		{
			if (GameHelper.GetDataManager().currentGame.firstLoad)
			{
				for(int i = 0; i < startItems.Length; i++)
				{
						
					PickupObject( System.Activator.CreateInstance(System.Type.GetType( startItems[i] )) as InventoryItem, 1, false);
				}
			}
			else
			{

				return;
			}
		}
	

	//	PickupObject (new WoodLog (), 10, false);
	//	PickupObject (new ItemBastoncini (), 10, false);
		if (!string.IsNullOrEmpty(CharManager.manager.character.Name ))
		{
			shirt.Tint = CharManager.manager.character.ShirtColor;
			pants.Tint = CharManager.manager.character.PantsColor;
			Boots.Tint = CharManager.manager.character.BootsColor;
		}
		else
		{
			shirt.Tint = new SerializedColor(Color.cyan);
			pants.Tint = new SerializedColor(Color.gray);
			Boots.Tint = new SerializedColor(Color.blue);
		}

		PickupObject (shirt, 1, false);
		PickupObject (pants, 1, false);
		PickupObject (Boots, 1, false);
		StartCoroutine (equipStuff ());
	}

	public InventoryItem GetItem(string itemname)
	{
		InventoryItem i = (from it in Inventory where it.Item.ItemName == itemname select it).First ().Item;
		return i;
	}

	IEnumerator equipStuff()
	{
		yield return new WaitForSeconds (0.2f);
		shirt.Equip ();
	//	pants.Equip ();
	//	Boots.Equip ();
	}
	
	// Update is called once per frame
	bool isActive = false;
	void Update () {
		}

	public GameObject InstantiateObject(GameObject o)
	{
		GameObject obj = Instantiate (o) as GameObject;
		return obj;
	}

	public object[] Serialize()
	{
		object[] data = new object[1];
		List<SavedInventoryEntry> s = new List<SavedInventoryEntry> ();
		foreach(InventoryEntry i in Inventory)
		{
			s.Add( new SavedInventoryEntry() { ItemType = i.Item.GetType(), amount = i.Amount } );
		}
		data [0] = s;

		return data;
	}

	public void DeSerialize(object[] data)
	{
		List<SavedInventoryEntry> s;
		s = (List<SavedInventoryEntry>)data [0];
		DropAll ();
		foreach(SavedInventoryEntry e in s)
		{
			PickupObject( (InventoryItem)System.Activator.CreateInstance(e.ItemType), e.amount, false);
			//Inventory.Add( new InventoryEntry() { Item = (InventoryItem)System.Activator.CreateInstance(e.ItemType), Amount = e.amount, Remove = false } );
		}
	}

	public void OnLoadingFinished()
	{

	}

	public string GetUID()
	{
		return uid;
	}

};


[System.Serializable]
public class InventoryEntry
{
	public InventoryItem Item;
	public int Amount;
	public bool Remove = false;
}

[System.Serializable]
public class SavedInventoryEntry
{
	public System.Type ItemType;
	public int amount;
}