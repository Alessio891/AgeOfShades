using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellsDatabase : MonoBehaviour {

	public List<GameObject> Spells = new List<GameObject>();

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}

	public ISpell GetSpell(string spellName)
	{
		foreach(GameObject o in Spells)
		{
			ISpell s = o.GetComponent(typeof(ISpell)) as ISpell;
			if (s.Name == spellName)
			{
				return s;
			}
		}
		return null;
	}

	public SpellInformations GetSpellInfo(string spellName)
	{
		Debug.Log ("Gathering " + spellName + " infos");
		SpellInformations si = new SpellInformations();
		ISpell spell = GetSpell (spellName);
		si.Name = spell.Name;
		si.Description = spell.Description;
		si.MP = "1";
		si.ReagentsNeeded = new List<Reagent>(spell.ReagentsNeeded);
		si.Icon = spell.Icon;
		return si;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
