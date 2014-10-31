using UnityEngine;
using System.Collections;

public class FollowPlayer : BaseBehaviour {



	public FollowPlayer(GameObject owner) : base(owner)
	{
		entity.OverrideMecanim = true;
	}

	bool isMoving = true;
	Vector3 lastPos = Vector3.zero;

	public override void Update ()
	{
		if (lastPos != Owner.transform.position)
			isMoving = true;
		else
			isMoving = false;
		if (isMoving)
		{
			if (!entity.UseMecanim)
				agent.animation.Play(Owner.GetComponent<BasicNPC>().Animations.WalkAnimation.name);
			else
			{
				if (Vector3.Distance(Owner.transform.position, GameHelper.GetLocalPlayer().transform.position) > 5)
				{
					agent.speed = 4;
					entity.animator.Play(Animator.StringToHash("Base Layer.Running"));
				}
				else
				{
					agent.speed = 3;
					entity.animator.Play(Animator.StringToHash("Base Layer.Walking"));
				}
			}
		}
		else
		{
			if (!entity.UseMecanim)
				agent.animation.Play(Owner.GetComponent<BasicNPC>().Animations.IdleAnimation.name);
			else
				entity.animator.Play(Animator.StringToHash("Base Layer.Idle"));
				
		}
		
		if (Vector3.Distance(Owner.transform.position, GameHelper.GetLocalPlayer().transform.position) > 2)
		{
			agent.SetDestination(GameHelper.GetLocalPlayer().transform.position - GameHelper.GetLocalPlayer().transform.forward * 0.5f);
		}
		else
		{
			Vector3 angles = agent.transform.eulerAngles;
			
			agent.transform.LookAt(GameHelper.GetLocalPlayer().transform.position);
			agent.Stop();
			agent.transform.eulerAngles = new Vector3(angles.x, agent.transform.eulerAngles.y, angles.z);
		}

		lastPos = Owner.transform.position;
	}
}
