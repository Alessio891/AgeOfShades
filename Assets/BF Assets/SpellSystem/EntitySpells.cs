using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EntitySpells : MonoBehaviour, ICaster {

	// TESTING
	public GameObject testSpell;
	// /TESTING
	public UnityEngine.UI.Image castingBar;
	public Transform Transform { get { return transform; } }
	public bool isPlayer { get { return (GetComponent<EntityStatus> () != null); } }
	public UnityEngine.UI.Image CastingBar { get { return castingBar; } }
	bool isCasting;
	public bool IsCasting { get { return isCasting; } set { isCasting = value; } }
	public List<ISpell> CurrentKnownSpells = new List<ISpell>();
	public ISpell currentSpell;
	// Use this for initialization
	void Start () {
	
	}



	public bool HasEnoughMP(float mp)
	{
		EntityStatus status = GetComponent<EntityStatus> ();
		if (status != null)
		{
			if (status.Mana < mp)
				return false;
		}
		return true;
	}
	public void ConsumeMP(float mp)
	{
		EntityStatus status = GetComponent<EntityStatus> ();
		if (status != null)
		{
			status.Mana -= mp;
			if (status.Mana <= 0)
				status.Mana = 0.1f;
		}
	}



	public bool SkillCheckSuccessful(string skillName, float minvalue)
	{
		return true;
	}

	public void AddSpell(ISpell spell)
	{
		if (GetSpell(spell.Name) != null)
			return;
		CurrentKnownSpells.Add (spell);
		spell.GO.SetActive (false);
	}

	public ISpell GetSpell(string _name)
	{
		return  (from s in CurrentKnownSpells where s.Name == _name select s).FirstOrDefault ();
	}
	public ISpell GetSpell(ISpell spell)
	{
		return (GetSpell (spell.Name));
	}

	public void CastSpell(string spellName)
	{
		GameObject o = Instantiate (GameHelper.SpellDB.GetSpell (spellName).GO) as GameObject;
		ISpell spell = o.GetComponent (typeof(ISpell)) as ISpell;
		if (spell != null)
		{
			currentSpell = spell;
			spell.GO.SetActive(true);
			spell.Cast (this);
		}
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

	ISpell _spell;
	// Update is called once per frame
	void Update () {

	}
}
