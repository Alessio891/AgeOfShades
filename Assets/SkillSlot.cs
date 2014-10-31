using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour {

	public Skill Skill;
	GameObject player;
	GameObject button;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		button = GameHelper.GetChildOf (gameObject, "UseButton");
	}

	public void OnClick()
	{
		Skill.OnUse ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Skill.CanBeUsed)
		{
			button.SetActive(true);
		}
		else
		{
			button.SetActive(false);
		}
		foreach(RectTransform t in GetComponentsInChildren<RectTransform>())
		{
			if (t.name == "SkillValue")
			{
				t.GetComponent<Text>().text =  (GameHelper.GetPlayerComponent<EntitySkills>() as EntitySkills).GetSkillValueText(Skill.Name);
				break;
			}
		}
	}
}
