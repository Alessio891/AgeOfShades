using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpellBookItem : InventoryItem {

	public List<ISpell> Spells = new List<ISpell> ();
	SpellsDatabase SpellDB;
	protected override void Start ()
	{
		Prefab = Resources.Load ("SpellBook") as GameObject;
		ItemName = "Spellbook";
		ItemDescription = "Libro del mago";
		ItemIcon = "Spellbook";
		SpellDB = GameHelper.SpellDB;


		Spells.Add (SpellDB.GetSpell ("Shockwave"));
		Spells.Add (SpellDB.GetSpell ("Fireball"));
		Spells.Add (SpellDB.GetSpell ("Cura"));
		Spells.Add (SpellDB.GetSpell ("Bloodsucking"));
		Spells.Add (SpellDB.GetSpell ("Sfera Luminosa"));
		Spells.Add (SpellDB.GetSpell ("Passo Svelto"));

		Weight = 1;
	}

	public override void OnUse ()
	{

		GameObject.Find ("SpellBook").GetComponent<SpellBook> ().Show (Spells);
	}

	public override void OnRemove ()
	{
		base.OnRemove ();

	}

	public override void OnPickup ()
	{
		base.OnPickup ();
	}
}
