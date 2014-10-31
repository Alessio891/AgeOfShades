using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateChar_SkillList : MonoBehaviour {

	public SkillPicker skillPicker;
	public GameObject SkillSlotPrefab;
	public List<SkillInfo> skills = new List<SkillInfo>();
	List<GameObject> skillEntries = new List<GameObject>();
	// Use this for initialization
	void Start () {
		skills.Clear ();
		skills.Add (new SkillInfo("Corpo A Corpo", "Abilita base nel combattimento corpo a corpo"));
		skills.Add (new SkillInfo("Cultura", "Aumenta la riuscita delle abilita"));
		skills.Add (new SkillInfo("Magia", "Abilita nelle magie"));
		skills.Add (new SkillInfo("Tiro con l'Arco", "Aumenta velocita e danno di archi e balestre"));

		foreach(SkillInfo s in skills)
		{
			if (s.name == skillPicker.slot1.skill || s.name == skillPicker.slot2.skill || s.name == skillPicker.slot3.skill)
				continue;
			GameObject o = Instantiate(SkillSlotPrefab) as GameObject;
			o.GetComponent<CreateChar_SkillEntry>().Skill = s.name;
			o.GetComponent<CreateChar_SkillEntry>().Description = s.description;
			o.GetComponent<CreateChar_SkillEntry>().skillPicker = skillPicker;
			o.transform.SetParent(gameObject.transform);
			o.transform.localEulerAngles = Vector3.zero;
			o.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			o.transform.localPosition = Vector3.zero;
			skillEntries.Add(o);
		}

		skillPicker.SkillListToggle (false);
		
	}

	public void RecalculateList()
	{
		foreach(GameObject o in skillEntries)
			Destroy(o);
		foreach(SkillInfo s in skills)
		{
			if (s.name == skillPicker.slot1.skill || s.name == skillPicker.slot2.skill || s.name == skillPicker.slot3.skill)
				continue;
			GameObject o = Instantiate(SkillSlotPrefab) as GameObject;
			o.GetComponent<CreateChar_SkillEntry>().Skill = s.name;
			o.GetComponent<CreateChar_SkillEntry>().Description = s.description;
			o.GetComponent<CreateChar_SkillEntry>().skillPicker = skillPicker;
			o.transform.SetParent(gameObject.transform);
			o.transform.localEulerAngles = Vector3.zero;
			o.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			o.transform.localPosition = Vector3.zero;
			skillEntries.Add(o);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
