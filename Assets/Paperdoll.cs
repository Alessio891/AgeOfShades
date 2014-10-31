using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class Paperdoll : MonoBehaviour, IDropHandler, ILoadingFinishedHandler {

	public List<GameObject> EquipSlots = new List<GameObject>();
	public bool Visible { get { return GetComponentInChildren<Graphic> ().enabled; } }
	public GameObject paperdollMesh;
	public static Paperdoll instance;
	void Awake()
	{
		instance = this;
		PlayerEquip e = (GameHelper.GetPlayerComponent<PlayerEquip> () as PlayerEquip);
		if (e == null)
			return;
	/*	foreach(BaseArmor a in e.Armor.list)
		{
			Debug.Log("Paperdoll: Equipping " + a.ItemName);
			if (a != null)
			{
				(GameObject.FindObjectOfType(typeof(PaperdollMesh)) as PaperdollMesh).EquipArmor(a, a.ArmorSlot, a.Tint.Color);
			}
		}*/
		//paperdollMesh.GetComponent<PlayerMeshManager> ().Skins.FromClone (GameHelper.GetPlayerComponent<PlayerMeshManager> ().Skins, paperdollMesh.GetComponent<PlayerMeshManager> ());

	}
	// Use this for initialization
	void Start () {
		foreach(GameObject e in GameObject.FindGameObjectsWithTag("PaperdollSlot"))
		{
			EquipSlots.Add(e);
		}
		Hide ();
	}

	public void OnLoadingFinished()
	{
		Hide ();
		
	}

	public void Hide()
	{
		GameHelper.HideMenu (gameObject);
		paperdollMesh.SetActive (false);
	}
	public void Show()
	{
		GameHelper.ShowMenu (gameObject);
		paperdollMesh.SetActive (true);
	}

	public void OnDrop(PointerEventData data)
	{
		IDropInContainer i = data.pointerDrag.GetComponent(typeof(IDropInContainer)) as IDropInContainer;
		if (i == null || i.GetItem() == null)
		{
			Debug.Log("No item");
			return;
		}

		InventoryItem item = i.GetItem ();
		Debug.Log (item);
		if (item is BaseArmor)
		{
			PlayerEquip e = (PlayerEquip)GameHelper.GetPlayerComponent<PlayerEquip>();
			(item as BaseArmor).OnUse();
			GetComponentInChildren<PaperdollMesh>().EquipArmor( (item as BaseArmor), (item as BaseArmor).ArmorSlot, item.Tint.Color);
			List<GameObject> s = (from o in EquipSlots where o.GetComponent<PaperdollEquip>().Slot == (item as BaseArmor).ArmorSlot select o).ToList();
			foreach(GameObject g in s)
			{
				g.GetComponent<PaperdollEquip>().Item = item as BaseArmor;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
