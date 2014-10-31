using UnityEngine;
using System.Collections;

public class AsciaOsso : BaseWeapon {
	
	protected override void Start ()
	{
		ItemName = "Ascia d'osso";
	/*	ItemDescription = "Un'ascia costruita dalle osse di un animale";
		ItemIcon = "BoneAxeIcon";
		ItemIconAtlas = "ItemIcons2";
		CanPlace = false;
		CanStack = false;*/
		Prefab = Resources.Load ("BoneAxe") as GameObject;
		/*
		AttackRange = 9;
		AttackDelay = 1;
		AttackDamage = 3;*/
		SkillUsed = "Corpo a corpo";
	}
	
	public override void OnUse ()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerEquip> ().Equip (this);
	}
}
