using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDropCatcher : MonoBehaviour, IDropHandler, IContainer {

	// Use this for initialization
	void Start () {
	
	}

	public void ReceiveItem(InventoryItem item)
	{

		(GameHelper.GetPlayerComponent<PlayerInventory>() as PlayerInventory).PickupObject(item);		
	}

	public void OnDrop(PointerEventData data)
	{
		IDropInContainer p = data.pointerDrag.GetComponent(typeof(IDropInContainer)) as IDropInContainer;
		if (p != null)
		{
			ReceiveItem(p.GetItem());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
