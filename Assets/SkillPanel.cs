using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour {

	GameObject Player;
	public GameObject SkillSlotPrefab;
	public bool Visible { get { return GetComponentInChildren<Graphic> ().enabled; } }
	
	// Use this for initialization
	void Start () {
		EntitySkills skills = (EntitySkills)GameHelper.GetPlayerComponent<EntitySkills> ();
		int i = 0;
		foreach(KeyValuePair<string,Skill> skill in skills.Skills)
		{
			if (skill.Value.MustBeUnlocked)
			{
				bool unlocked = true;
				foreach(KeyValuePair<Skill, float> s in skill.Value.SkillsNeeded)
				{
					if ( skills.Skills[s.Key.Name].Value < s.Value )
					{
						unlocked = false;
						break;
					}
				}
				if (!unlocked)
				{
					continue;
				}
			}
			GameObject o = Instantiate(SkillSlotPrefab) as GameObject;
			foreach(RectTransform t in o.GetComponentsInChildren<RectTransform>())
			{
				if (t.name == "SkillName")
				{
					t.GetComponent<Text>().text = skill.Value.Name;
				}
				else if (t.name == "SkillValue")
				{
					if (skill.Value.Value % 1 == 0)
					{
						t.GetComponent<Text>().text = skill.Value.Value.ToString() + ".0";
					}
					else
					{
						t.GetComponent<Text>().text = skill.Value.Value.ToString();						
					}
				}	
			}
			o.transform.SetParent(GameObject.Find("SkillList").transform);
		//	o.transform.localPosition = GetValidPosition(i);
			o.transform.localScale = Vector3.one;
			o.GetComponent<SkillSlot>().Skill = skill.Value;
			i++;
		}
	}

	Vector3 GetValidPosition(int index)
	{
		Vector3 value = Vector3.zero;

		value.x = 32;
		value.y = (30 * index) + 30;

		return value;
	}

	bool panelActive = false;
	
	// Update is called once per frame
	void Update () {

	}
}
