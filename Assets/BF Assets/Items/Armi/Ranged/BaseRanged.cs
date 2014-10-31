using UnityEngine;
using System.Collections;

public class BaseRanged : BaseWeapon {

	public GameObject Arrow;
	public InventoryItem ArrowItem;

	protected override void Start ()
	{
		base.Start ();

		Arrow = Resources.Load ("Elven Long Bow Arrow") as GameObject;
		AttackRange = 20;
		AttackDelay = 1;
	}

	public override void OnAttack (GameObject Target)
	{
		Vector3 targetDir = Target.transform.position - GameHelper.GetLocalPlayer().transform.position;
		Vector3 forward = GameHelper.GetLocalPlayer().transform.forward;
		float angle = Vector3.Angle(targetDir, forward);
		Debug.Log (angle);
		if (angle > 45)
		{
			return;
		}
		PlayerCombat e = (PlayerCombat)GameHelper.GetPlayerComponent<PlayerCombat> ();
		GameObject a = GameObject.Instantiate (Resources.Load("Elven Long Bow Arrow")) as GameObject;
		BaseArrow arr = a.GetComponent<BaseArrow> ();
		a.transform.position = (GameHelper.GetPlayerComponent<PlayerEquip> () as PlayerEquip).GetRightHoldObject ().transform.position - GameHelper.GetLocalPlayer().transform.right * 0.2f;
		arr.direction = e.Target.transform.position - e.transform.position;
		arr.speed = 3.5f;
		arr.Shoot ();
	}

}
