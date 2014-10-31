using UnityEngine;
using System.Collections;

public class NPCWanderBehaviour : BaseBehaviour {

	float WanderDistance = 20;
	
	bool targetFound = false;
	public Vector3 targetPos;
	bool wait = false;
	float _timer = 0;
	Animator animator;
	
	NavMeshAgent agent;
	
	public NPCWanderBehaviour(GameObject owner) : base(owner)
	{
		agent = owner.GetComponent<NavMeshAgent> ();
		animator = owner.GetComponent<Animator> ();
	}
	
	public override void Update ()
	{

		
		if (lastPos != Owner.transform.position)
			isMoving = true;
		else
			isMoving = false;
		lastPos = Owner.transform.position;

		if (isMoving)
		{
			agent.gameObject.animation.Play(Owner.GetComponent<BasicNPC>().Animations.WalkAnimation.name);
		}
		else
		{
			agent.gameObject.animation.Play(Owner.GetComponent<BasicNPC>().Animations.IdleAnimation.name);
		}

		if (!targetFound)
		{
			Vector3 target = Owner.transform.position + new Vector3(Random.Range (-15, 15), 0, Random.Range(-15,15));
			if (CanGoThere(target))
			{
				agent.SetPath(currentPath);
				targetFound = true;
			}
		}
		else
		{
			if (agent.remainingDistance <= 1f && !wait)
			{
				agent.Stop ();
				agent.path.ClearCorners();
				wait = true;
			}
		}
		
		
		if (wait)
		{
			_timer += Time.deltaTime;
			if (_timer >= 5)
			{
				targetFound = false;
				_timer = 0;
				wait = false;
			}
		}
	}
	
	
	
	public override void Think ()
	{
		
	}

}
