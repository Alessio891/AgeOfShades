using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FriendlyStates
{
	Idle = 0,
	Talking,
	Walking,
	Busy
}

[System.Serializable]
public class BasicNPC : BasicEntity, INeutral {
	protected override void Init ()
	{
		if (GameHelper.GameIsLoading)
			return;
		//Behaviours.Add (new NPCWanderBehaviour (gameObject));
	//	Behaviours.Add (new FollowPlayer (gameObject));
	//	Behaviours.Add (new AttackHostile (gameObject));
		foreach(Canvas c in GetComponentsInChildren<Canvas>())
		{
			if (c.name == "HealthBar")
			{
				c.enabled = true;
			}
		}
	}

	public override void OnSelect ()
	{
		base.OnSelect ();
		GameHelper.ShowNotice ("[NPC] " + EntityName, gameObject);
	}
	
	protected override void UpdateLogic ()
	{

	}

	public virtual void AcceptDialog()
	{
	}
}