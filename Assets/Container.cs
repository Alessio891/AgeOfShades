using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Container : MonoBehaviour, IDropHandler, IContainer, IPointerClickHandler {
	public Corpse Corpse;
	// Use this for initialization
	void Start () {
	
	}

	public virtual void ReceiveItem(InventoryItem item) {
		GameObject i = new GameObject();
		i.AddComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + item.ItemIcon) as Sprite;
		i.AddComponent<CorpseLootItem>().Item = item;
		i.transform.SetParent(gameObject.transform, true);
		i.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 1);
		Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
		Vector3 pos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle (GetComponent<RectTransform> (), Input.mousePosition, c, out pos);
		i.GetComponent<RectTransform> ().position = pos;
		i.GetComponent<CorpseLootItem> ().Corpse = Corpse;
		Corpse.itemList.Add (item);
	}

	public void AutoDestroy()
	{
		Destroy (gameObject);
	}

	public void OnDrop(PointerEventData data)
	{
		IDropInContainer d = data.pointerDrag.GetComponent(typeof(IDropInContainer)) as IDropInContainer;
		if (d != null)
		{
			Debug.Log("Container!");
			ReceiveItem(d.GetItem());
		}
	}
	float lastClickTime = 0;
	bool clicked = false;
	public void OnPointerClick(PointerEventData data)
	{
		if (!clicked)
		{
			clicked = true;
			lastClickTime = data.clickTime;
		}
		else
		{
			if (data.clickTime - lastClickTime < 1)
			{
				Destroy(gameObject);
			}
			clicked = false;
		}
	}

	// Update is called once per frame
	void Update () {

	}
}

public interface IDropInContainer {
	InventoryItem GetItem();
	void SetItem(InventoryItem item);
}

public interface IContainer
{
	void ReceiveItem(InventoryItem item);
}