using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellInfo : MonoBehaviour {

	public List<Reagent> reagents;

	public GameObject ReagentList;
	public Text Name;
	public Text MPObject;
	public Text Desc; 


	// Use this for initialization
	void Start () {
	
	}

	bool reagentIsNeeded(string reag)
	{
		if (reagents == null)
			return false;
		foreach(Reagent a in reagents)
			if (a.name == reag)
				return true;
		return false;
	}

	public bool UpdateNow = true;
	
	// Update is called once per frame
	void Update () {
		if (UpdateNow)
		{
			foreach(RectTransform r in ReagentList.GetComponentsInChildren<RectTransform>())
			{
				if (reagentIsNeeded(r.name))
					r.GetComponent<ReagentListItem>().Need();
				else if (r.GetComponent<ReagentListItem>() != null)
					r.GetComponent<ReagentListItem>().NotNeeded();
					
			}
			/*
			if (EventSystemManager.currentSystem.currentSelectedObject == null || EventSystemManager.currentSystem.currentSelectedObject.GetComponent<SpellSlot>() == null)
			{
				Debug.Log("I'M NOT OVERING ANY SPELL!");
				reagents.Clear();
				Name.text = "";
				Desc.text = "";
				MPObject.text = "";
			}*/
			UpdateNow = false;
		}
	}
}

[System.Serializable]
public class SpellInformations
{
	public string Name;
	public string MP;
	public string Description;
	public List<Reagent> ReagentsNeeded;
	public Sprite Icon;

}