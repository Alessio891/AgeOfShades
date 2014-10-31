using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour {

	List<ISpell> SpellList = new List<ISpell>();
	public GameObject SpellSlotPrefab;
	public GameObject SpellGrid;
	public bool Visible { get { return GetComponentInChildren<Graphic> ().enabled; } }
	public int ShowingCircle = 1;
	// Use this for initialization
	void Start () {
	
	}

	public void Show(List<ISpell> bookSpells)
	{
		RemoveAllSlots ();
		SpellList = new List<ISpell> (bookSpells);
		FillSlots ();
		GameHelper.ShowMenu (gameObject);

	}

	public void RemoveAllSlots()
	{
		foreach(object o in GameObject.FindObjectsOfType(typeof(SpellSlot)))
		{
			Destroy( (o as MonoBehaviour).gameObject );
		}
	}

	public void SwitchCircle(int circle)
	{
		ShowingCircle = circle;
		RemoveAllSlots ();

		FillSlots ();
	}

	public void FillSlots()
	{
		foreach(ISpell spell in SpellList)
		{
			if (spell.Circle != ShowingCircle)
				continue;
			GameObject o = Instantiate(SpellSlotPrefab) as GameObject;

			SpellInformations si = GameHelper.SpellDB.GetSpellInfo(spell.Name);
			o.name = "Spell_" + si.Name;
			o.GetComponent<SpellSlot>().spell = si;

			o.transform.SetParent(SpellGrid.transform);
			o.transform.localScale = Vector3.one;
		}
	}

	public void Hide()
	{
		SpellList.Clear ();
		RemoveAllSlots ();
		GameHelper.HideMenu	 (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
