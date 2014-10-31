using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;
public class CorpseLootItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropInContainer {
	public InventoryItem Item;
	public Corpse Corpse;
	// Use this for initialization
	void Start () {
	
	}

	public InventoryItem GetItem() {
				return Item;
	}
	public void SetItem(InventoryItem item) {
				Item = item;
		}

	public void OnBeginDrag(PointerEventData data)
	{
		gameObject.transform.SetParent (GameObject.Find ("GlobalUI").transform);
	//	gameObject.GetComponent<RectTransform> ().SetAsFirstSibling ();
		gameObject.AddComponent<IgnoreRaycast> ();
	}


	public void OnEndDrag(PointerEventData data)
	{
		if (!EventSystemManager.currentSystem.IsPointerOverEventSystemObject())
		{
			if (Item.Prefab != null)
			{
				GameObject o = Instantiate(Item.Prefab) as GameObject;
				Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] hit;
				hit = Physics.RaycastAll(r);
				Vector3 t = GameHelper.GetLocalPlayer().transform.position;
				foreach(RaycastHit h in hit)
				{
					if (h.collider.name == "Terrain")
					{
						t = h.point;
					}
				}
				o.transform.position = t;
			}
		}
		int i = 0;
		foreach(InventoryItem item in Corpse.itemList)
		{
			if (item.ItemName == Item.ItemName)
			{
				break;
			}
			i++;
		}
		Corpse.itemList.RemoveAt (i);
		Destroy (gameObject);
	}

	public void OnDrag(PointerEventData data)
	{

		Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
		Vector3 pos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle (GetComponent<RectTransform> (), Input.mousePosition, c, out pos);
		GetComponent<RectTransform> ().position = pos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
