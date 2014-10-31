using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SpellSlot : MonoBehaviour, 
						 IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,
						 ISpellDrop {

	public SpellInformations spell;
	public Image textureSprite;
	bool wasNull = false;

	bool isOver = false;
	GameObject draggedObject;

	// Use this for initialization
	void Start () {
		textureSprite = GameHelper.GetChildOf (gameObject, "Icon").GetComponent<Image> ();
	}

	public SpellInformations SpellInfo { get { return spell; } }

	public void OnBeginDrag(PointerEventData data)
	{
		Debug.Log ("drag");
		draggedObject = Instantiate (gameObject) as GameObject;
		draggedObject.transform.SetParent (GameObject.Find ("GlobalUI").transform);
		draggedObject.transform.SetAsLastSibling ();
		draggedObject.AddComponent<IgnoreRaycast> ();
		draggedObject.transform.localScale = transform.localScale;
		Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
		Vector3 pos = Input.mousePosition;
		pos.z = 0;
		draggedObject.GetComponent<RectTransform> ().position = pos;
	}
	public void OnEndDrag(PointerEventData data)
	{
		Destroy (draggedObject);
	}
	public void OnDrag(PointerEventData data)
	{
		Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
		Vector3 pos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle (GetComponent<RectTransform> (), Input.mousePosition, c, out pos);
		draggedObject.GetComponent<RectTransform> ().position = pos;
	}


	public void OnPointerClick(PointerEventData data)
	{

		if (spell != null)
		{
			(GameHelper.GetPlayerComponent<EntitySpells>() as EntitySpells).CastSpell(spell.Name);
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		SpellInfo i = GameObject.Find("SpellInfo").GetComponent<SpellInfo>();
		//ISpell s = (GameHelper.GetPlayerComponent<PlayerSpells>() as PlayerSpells).GetSpell(spell.Name);
		i.Name.text = spell.Name;
		i.Desc.text = spell.Description;
		i.MPObject.text = "1";
		i.reagents = new System.Collections.Generic.List<Reagent> (spell.ReagentsNeeded);
		i.UpdateNow = true;
	}

	public void OnPointerExit(PointerEventData data)
	{
		isOver = false;
		SpellInfo i = GameObject.Find("SpellInfo").GetComponent<SpellInfo>();
		i.reagents.Clear ();
		i.Name.text = "";
		i.Desc.text = "";
		i.MPObject.text = "";
		i.UpdateNow = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (spell != null)
		{
			textureSprite.sprite = spell.Icon;
		}
	}
}

public interface ISpellDrop
{
		SpellInformations SpellInfo { get; }
}