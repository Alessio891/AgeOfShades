using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillPicker : MonoBehaviour {
	public GameObject SkillList;
	public CreateChar_SkillSlot currentSlot;
	public CreateChar_SkillSlot slot1;
	public CreateChar_SkillSlot slot2;
	public CreateChar_SkillSlot slot3;
	public CreateChar_SkillList skillNames;
	// Use this for initialization

	void Start()
	{

		
	}

	public void SkillListToggle(bool t)
	{
		SkillList.SetActive (t);
		GameHelper.ShowMenu (SkillList);
	}

	public void PickedSkill(string skillPicked, string skillDesc)
	{
		GameObject.Find ("SkillDescription").GetComponent<Text> ().text = "";
		
		skillNames.skills.RemoveAll ( s => s.name == skillPicked );
		skillNames.RecalculateList ();
		currentSlot.skill = skillPicked;
		currentSlot.description = skillDesc;
		SkillList.SetActive (false);
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
