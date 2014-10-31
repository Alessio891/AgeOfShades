using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class QuickSlotButton : MonoBehaviour, IDropHandler, IContainer, ISerializedObject  {

	public InventoryItem Slot;
	Image textureSprite;
	PlayerInventory pi;
	public KeyCode ActivationButton = KeyCode.Alpha1;
	public string uid;

	public SpellInformations spell;

	// Use this for initialization
	void Start () {
		uid = GameHelper.GetUIDForObject (this);

		Slot = null;
		spell = null;
		foreach(Image i in GetComponentsInChildren<Image>())
		{
			if (i.name == "ItemTexture")
			{
				textureSprite = i;
				
				break;
			}
		}
		pi = (PlayerInventory)GameHelper.GetPlayerComponent<PlayerInventory> ();
	}
	object item;
	public object[] Serialize() {
		object[] data = new object[1];
		data [0] = (Slot != null) ? Slot.GetType ().ToString () : "null";
		return data;
	}
	public void DeSerialize(object[] data) {
		if ( (string)data[0] != "null" )
			Slot = (InventoryItem)System.Activator.CreateInstance ( System.Type.GetType( (string)data[0] ) );
	}
	public string GetUID() { return uid; }
	public void OnLoadingFinished() {}

	public void ReceiveItem(InventoryItem item)
	{
	}
	// Update is called once per frame
	void Update () {
		if (Slot != null)
		{
			if (!pi.Has(Slot.ItemName, 1))
			{
				Slot = null;
				return;
			}
			textureSprite.color = new Color(1,1,1,1);
			string path = "Icons/" + Slot.ItemIcon;
			Sprite s = (Sprite)Resources.Load<Sprite>(path);
			textureSprite.sprite =  s;
			if (Input.GetKeyDown(ActivationButton))
			{
				OnClick();
			}
		}
		else if (spell != null)
		{
			textureSprite.color = new Color(1,1,1,1);
			textureSprite.sprite = spell.Icon;
			if (Input.GetKeyDown(ActivationButton))
			{
				OnClick();
			}
		}
		else
		{
			textureSprite.color = new Color(1,1,1,0);
		}
	}

	public void OnDrop(PointerEventData data)
	{
		Debug.Log ("Dropped");
		if (data.pointerDrag != null)
		{
			IDropInContainer d = data.pointerDrag.GetComponent(typeof(IDropInContainer)) as IDropInContainer;
			ISpellDrop sd = data.pointerDrag.GetComponent(typeof(ISpellDrop)) as ISpellDrop;
			if (d != null)
			{
				Slot = (d.GetItem());
			}
			else if (sd != null) {
				spell = sd.SpellInfo;
			}
		}
	}

	public void OnClick()
	{
		if (Slot != null)
		{
			Slot.OnUse();
		}
		else if (spell != null)
		{
			GameHelper.GetPlayerComponent<EntitySpells>().CastSpell(spell.Name);
		}
	}

}
