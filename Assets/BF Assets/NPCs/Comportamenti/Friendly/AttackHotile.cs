using UnityEngine;
using System.Collections;
using System.Linq;

public class AttackHostile : BaseBehaviour {
	
	public AttackHostile(GameObject owner) : base(owner)
	{}
	
	bool isMoving = true;
	Vector3 lastPos = Vector3.zero;
	GameObject Target;

	GameObject nearestEnemy
	{
		get
		{
			return (from o in GameObject.FindGameObjectsWithTag("SimpleEnemy") orderby Vector3.Distance( Owner.transform.position, o.transform.position ) select o).FirstOrDefault();
		}
	}

	float DistanceFromTarget
	{
		get { return Vector3.Distance (Owner.transform.position, Target.transform.position); }
	}

	public override void Update ()
	{
		if (lastPos != Owner.transform.position)
			isMoving = true;
		else
			isMoving = false;

		if (!entity.UseMecanim)
		{
			if (isMoving)
			{
				agent.animation.Play(Owner.GetComponent<BasicNPC>().Animations.WalkAnimation.name);
			}
			else
			{
				agent.animation.Play(Owner.GetComponent<BasicNPC>().Animations.IdleAnimation.name);
			}
		}
		if (Target == null && nearestEnemy != null)
		{
			Target = nearestEnemy;
			if (DistanceFromTarget > 30)
				Target = null;
		}
		else if (Target != null)
		{
			if (DistanceFromTarget > 1.5f)
			{
				agent.SetDestination(Target.transform.position + Target.transform.forward);
			}
			else
			{
				agent.Stop();
				if (Target == null)
					return;
				Target.GetComponent<BasicEntity>().Damage(0.1f);
			}
		}

		lastPos = Owner.transform.position;
	}
}
