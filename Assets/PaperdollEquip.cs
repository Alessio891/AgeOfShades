using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class PaperdollEquip : MonoBehaviour, IPointerClickHandler, IDropInContainer, IBeginDragHandler, IEndDragHandler, IDragHandler {

	public BaseArmor Item;
	public BaseArmor DraggedItem;
	public ArmorSlots Slot;
	Image itemTexture;

	void Awake()
	{
		//itemTexture.color = new Color (1, 0, 0, 0);
	}
	// Use this for initialization
	void Start () {
		itemTexture = GetComponent<Image> ();
		//itemTexture.CrossFadeAlpha (0, 0.1f, false);
	}

	public void OnEndDrag(PointerEventData data)
	{
		Destroy (duplicate);
		DraggedItem = null;
	}

	public void OnBeginDrag(PointerEventData data)
	{

		duplicate = Instantiate(gameObject) as GameObject;
		//Destroy (duplicate.GetComponent<PaperdollEquip> ());
		duplicate.AddComponent<DraggedEquip> ().SetItem (Item);
		duplicate.transform.SetParent(GameObject.Find("GlobalUI").transform);
		duplicate.transform.SetAsLastSibling ();
		duplicate.transform.localScale = Vector3.one * 0.7f;
		duplicate.AddComponent<IgnoreRaycast>();

		duplicate.name = name + "_dragging";
		//GameObject.Find ("PaperdollCharacter").GetComponent<PaperdollMesh> ().UnEquipArmor (Slot);
		//(GameHelper.GetPlayerComponent<PlayerEquip> () as PlayerEquip).UnEquipArmor (Slot);
		Item.UnEquip ();
		DraggedItem = Item;
		Item = null;
		List<GameObject> s = (from o in GameObject.FindGameObjectsWithTag("PaperdollSlot") where o.GetComponent<PaperdollEquip>().Slot == Slot select o).ToList();
		foreach(GameObject g in s)
		{
			g.GetComponent<PaperdollEquip>().Item = null;
			//g.GetComponent<PaperdollEquip>().Item = item as BaseArmor;
		}
		
	}

	public InventoryItem GetItem()
	{ return (Item != null) ? Item : DraggedItem; }
	public void SetItem(InventoryItem item)
	{ Item = (BaseArmor)item; }
	GameObject duplicate;

	public void OnDrag(PointerEventData data)
	{
		Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
		Vector3 pos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle (GetComponent<RectTransform> (), Input.mousePosition, c, out pos);
		duplicate.GetComponent<RectTransform> ().position = pos;
	
	}


	// Update is called once per frame
	void Update () {
		if (Item != null && !string.IsNullOrEmpty(Item.ItemName))
		{
			//itemTexture.CrossFadeAlpha(1, 0.1f, false);
			itemTexture.color = Color.white;
			string path = "Icons/" + Item.ItemIcon;
			Sprite s = (Sprite)Resources.Load<Sprite>(path);
			
			itemTexture.sprite =  s;
		}
		else
		{
			itemTexture.color = new Color(1,1,1,0);
			//itemTexture.CrossFadeAlpha(1, 0.1f, true);
		}

	}

	public void OnPointerClick(PointerEventData data)
	{
		if (Item != null)
						GameHelper.SystemMessage ("Test click", Color.blue);
				else
						GameHelper.SystemMessage ("No item", Color.red);
	}


}

public class DraggedEquip : MonoBehaviour, IDropInContainer {

	InventoryItem Item;
	public InventoryItem GetItem()
	{ return Item; }
	public void SetItem(InventoryItem item)
	{ Item = item; }

	Image itemTexture { get { return GetComponent<Image> (); } }

	void Update () {
		if (Item != null && !string.IsNullOrEmpty(Item.ItemName))
		{
			//itemTexture.CrossFadeAlpha(1, 0.1f, false);
			itemTexture.color = Color.white;
			string path = "Icons/" + Item.ItemIcon;
			Sprite s = (Sprite)Resources.Load<Sprite>(path);
			
			itemTexture.sprite =  s;
		}
		else
		{
			itemTexture.color = new Color(1,1,1,0);
			//itemTexture.CrossFadeAlpha(1, 0.1f, true);
		}
		
	}
}