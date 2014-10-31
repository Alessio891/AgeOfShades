using UnityEngine;
using System.Collections;

public class Coltellino : BaseWeapon {
	protected override void Start ()
	{
		ItemName = "Coltellino";
		ItemDescription = "Coltello usato per tagliare il legno. Può essere usato come arma.";
		ItemIcon = "Coltellino";
		CanPlace = false;
		CanStack = false;
		Prefab = Resources.Load ("Knife") as GameObject;
		AttackRange = 4;
		AttackDelay = 0.5f;
		AttackDamage = 1.5f;
		SkillUsed = "Fencing";
	}
	public override void OnUse ()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerEquip> ().Equip (this);
	}
}
