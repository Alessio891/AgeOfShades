using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropInContainer, IPointerEnterHandler, IPointerExitHandler {

	public InventoryItem Item;
	public string DontTouch;
	public int stack = 0;
	UnityEngine.UI.Image textureSprite;

	public void OnPointerEnter(PointerEventData data)
	{
		overing = true;
	}
	public void OnPointerExit(PointerEventData data)
	{
		overing = false;
		_overTimer = 0;
		showing = false;
		shown = false;
		InventoryDescription d = GameObject.Find("InventoryDescriptionPanel").GetComponent<InventoryDescription>();
		d.Hide ();
		d.Selected = null;
	}
	float lastClickTime = 0;
	bool clicked = false;
	public void OnPointerClick(PointerEventData data)
	{
		if (!clicked)
		{
			clicked = true;
			lastClickTime = Time.time;
		}
		else
		{
			Debug.Log(Time.time - lastClickTime);
			if (Time.time - lastClickTime < 0.8f)
			{
				Item.OnUse();
			}
			clicked = false;
		}

	}

	public void MouseClick()
	{
	
	}

	public void SetItem(InventoryItem item)
	{
		Item = item;
	}

	public InventoryItem GetItem()
	{
		return Item;
	}

	// Use this for initialization
	void Start () {
		foreach(Image i in GetComponentsInChildren<Image>())
		{
			if (i.name == "ItemTexture")
			{
				textureSprite = i;

				break;
			}
		}

	}
	public bool remove = false;
	bool createdDuplicate = false;
	public GameObject duplicate = null;
	public bool dragging = false;
	public void OnBeginDrag(PointerEventData data)
	{

		if (!createdDuplicate)
		{
			duplicate = Instantiate(gameObject) as GameObject;
			duplicate.GetComponent<InventorySlot>().Item = Item;
			duplicate.transform.SetParent(GameObject.Find("GlobalUI").transform);
			duplicate.transform.localScale = Vector3.one * 0.7f;
			duplicate.AddComponent<IgnoreRaycast>();
			createdDuplicate = true;
		}
		Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
		Vector3 pos = Input.mousePosition;
		pos.z = 0;
		duplicate.GetComponent<RectTransform> ().position = pos;//new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
	}

	public void OnDrag(PointerEventData data)
	{
		/*
		 * 		m_DraggingPlane = data.pointerEnter.transform as RectTransform;
		
		var rt = m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 globalMousePos;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
			rt.rotation = m_DraggingPlane.rotation;
		}*/
		Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
		Vector3 pos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle (GetComponent<RectTransform> (), Input.mousePosition, c, out pos);
		duplicate.GetComponent<RectTransform> ().position = pos;
	}

	public void OnEndDrag(PointerEventData data)
	{
		createdDuplicate = false;
		if (!EventSystemManager.currentSystem.IsPointerOverEventSystemObject())
		{
			Debug.Log ("DROP!");
			
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
				(GameHelper.GetPlayerComponent<PlayerInventory>() as PlayerInventory).ConsumeObject(Item, 1);
			}
		}
		Destroy (duplicate);
	}


	bool overing = false;
	float _overTimer = 0;
	bool showing = false;
	bool shown = false;
	// Update is called once per frame
	void Update () {
		if (duplicate == null)
			createdDuplicate = false;
	
		if (!string.IsNullOrEmpty(Item.ItemName))
		{
			stack = (GameHelper.GetPlayerComponent<PlayerInventory> () as PlayerInventory).GetAmountOf (Item.GetType ());
			if (textureSprite.sprite == null || (textureSprite.sprite != null && textureSprite.sprite.name != Item.ItemIcon))
			{
				textureSprite.color = new Color(1,1,1,1);
				string path = "Icons/" + Item.ItemIcon;
				Sprite s = (Sprite)Resources.Load<Sprite>(path);

				textureSprite.sprite =  s;
			}
			(GameHelper.GetComponentInChildOf<Text>(gameObject, "ItemStack") as Text).text = stack.ToString();
		}
		else
		{
			textureSprite.color = new Color(1,1,1,0);
			
			if (textureSprite.sprite != null)
			{
				//textureSprite.sprite = null;

			}
		}

		if (overing)
		{
			if (!showing)
			{
				_overTimer += Time.deltaTime;
				if (_overTimer >= 1.2f)
				{
					showing = true;
					_overTimer = 0;
				}
			}
			else
			{
				if (!shown)
				{
					InventoryDescription d = GameObject.Find("InventoryDescriptionPanel").GetComponent<InventoryDescription>();
					d.Show (Item.ItemDescription);
					d.Selected = gameObject;
					shown = true;
				}

			}
		}
	}

	void OnDrag()
	{
		Debug.Log ("Dragging");
	}
}
