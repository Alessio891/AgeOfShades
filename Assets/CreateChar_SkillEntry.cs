using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateChar_SkillEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public string Skill;
	public string Description;
	public SkillPicker skillPicker;

	// Use this for initialization
	void Start () {
	
	}

	public void OnPointerEnter(PointerEventData data)
	{
		GameObject.Find ("SkillDescription").GetComponent<Text> ().text = Description;
	}

	public void OnPointerExit(PointerEventData data)
	{
		GameObject.Find ("SkillDescription").GetComponent<Text> ().text = "";
	}

	public void OnPointerClick(PointerEventData data)
	{
		Debug.Log (Skill);
		skillPicker.PickedSkill (Skill, Description);

	}
	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = Skill;
	}
}
