using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseWeapon : InventoryItem {

	protected bool Equipped { get { 
			PlayerEquip e = (PlayerEquip)GameHelper.GetPlayerComponent<PlayerEquip>();
			return ( (e.RightHand != null && e.RightHand.ItemName == ItemName) || (e.LeftHand != null && e.LeftHand.ItemName == ItemName)); 
		}
	}
	protected int EquippedInHand { 
		get {
			PlayerEquip e = (PlayerEquip)GameHelper.GetPlayerComponent<PlayerEquip>();
			if (e.RightHand.ItemName == ItemName)
				return 0;
			else
				return 1;
		}
	}

	protected bool canAttack = true;
	protected override void Start ()
	{
		base.Start ();
	}

	public override void OnUse ()
	{
		if (!Equipped)
		{
			(GameHelper.GetPlayerComponent<PlayerEquip> () as PlayerEquip).Equip (this);
		}
		else
		{
			(GameHelper.GetPlayerComponent<PlayerEquip>() as PlayerEquip).UnEquip(EquippedInHand);
		}
	}

	public override void Drop ()
	{

	}

	public virtual void OnAttack(GameObject Target)
	{
		if (WeaponDamage == null)
		{
			Debug.LogError("Weapon Damage Data non assegnato all'arma. Assegnarne uno nel metodo Start()");
			return;
		}

		if (Target.GetComponent<BasicEntity>() == null)
		{
			foreach(Canvas c in Target.GetComponentsInChildren<Canvas>())
			{
				if (c.name == "HealthBar")
				{
					c.enabled = false;
				}
			}
			Target = null;
			return;
		}

		CheckHit(Target);

	}

	public void CheckHit(GameObject hit)
	{
		if (hit.GetComponent<BasicEntity>() != null)
		{
			EntitySkills skills = GameHelper.GetPlayerComponent<EntitySkills>() as EntitySkills;		
			if (skills.SkillCheckSuccessful(SkillUsed, 20, 0.5f))
			{
				hit.GetComponent<BasicEntity>().Damage( WeaponDamage.GetFinalDamage() );
			}
			else
				hit.GetComponent<BasicEntity>().Damage( 0 );
			float inc = skills.SkillIncreaseSuccessful(SkillUsed, 0.5f, 20);

		}
	}


	public virtual void OnBreak()
	{
	}

}

public interface IMeleeWeapon {}
public interface IRangedWeapon {}
public interface IMagicWeapon {}