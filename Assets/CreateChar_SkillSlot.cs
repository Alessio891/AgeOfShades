using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateChar_SkillSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public string skill;
	public string description;
	public SkillPicker skillPicker;
	bool changed = false;
	public GameObject SkillList;
	GameObject skillDescription;

	// Use this for initialization
	void Start () {
		skillDescription = GameObject.Find ("SkillDescription");
	}

	public void OnPointerClick(PointerEventData data)
	{
		skillPicker.currentSlot = this;
		//GameHelper.ShowMenu (GameObject.Find ("SkillList"));
		skillPicker.SkillListToggle (true);
	}

	public void OnPointerEnter(PointerEventData data)
	{
		skillDescription.GetComponent<Text> ().text = description;
	}
	public void OnPointerExit(PointerEventData data)
	{
		skillDescription.GetComponent<Text> ().text = "";
	}
	// Update is called once per frame
	void Update () {
		if (skill != null)
		{
			GetComponentInChildren<Text>().text = skill;
		}
		else
			GetComponentInChildren<Text>().text = "";
	}
}

public class SkillInfo
{
	public string name;
	public string description;
	public List<InventoryItem> BonusItems = new List<InventoryItem> ();

	public SkillInfo(string n, string d)
	{ name = n; description = d; }
	public SkillInfo(string n, string d, List<InventoryItem> bonusItems)
	{ name = n; description = d; BonusItems = bonusItems; }
}
