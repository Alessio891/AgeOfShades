using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum BasicMeleeStates
{
	Standing = 0,
	Wandering,
	Chasing,
	WaitForAttack,
	Attacking,
	Fleeing
}

public class BasicMeleeBehaviour : BaseBehaviour {

	public float thinkingTimer = 0.2f; // Time before the entity reacts to something
	float _timer = 0; // internal timer
	
	public BasicMeleeStates State = BasicMeleeStates.Standing;

	public GameObject PlayerTarget;
	public bool UnderAttack { get { return (GameHelper.GetPlayerComponent<PlayerCombat>().Target != null && GameHelper.GetPlayerComponent<PlayerCombat>().Target.GetInstanceID() == Owner.GetInstanceID());  } }
	public float playerNearbyRange = 7;
	public float MinWanderRange = 0.5f;
	public float MaxWanderRange = 5;
	bool badlyInjured { get { return (entity.Status.Life / entity.Status.MaxLife) < 0.5f; } }
	bool nearDeath { get { return (entity.Status.Life / entity.Status.MaxLife) < 0.2f; } }
	bool playerNearby { get { return (Vector3.Distance (GameHelper.GetLocalPlayer ().transform.position, Owner.transform.position) < playerNearbyRange) && !GameHelper.GetPlayerComponent<EntityStatus>().HasBuff(typeof(GodMode)); } }

	public BasicMeleeBehaviour(GameObject owner) : base(owner)
	{
		agent = owner.GetComponent<NavMeshAgent> ();
		entity = Owner.GetComponent<BasicEntity> ();
		oldPos = Owner.transform.position;
	}

	public override void Update ()
	{
		base.Update ();
		Think ();

		switch(State)
		{
		case BasicMeleeStates.Standing:
			Stand ();
			break;
		case BasicMeleeStates.Wandering:
			Wander ();
			break;
		case BasicMeleeStates.Chasing:
			Chase ();
			break;
		case BasicMeleeStates.Fleeing:
			Flee ();
			break;
		case BasicMeleeStates.Attacking:
			Attack();
			break;
		}

		UpdateAnimation ();
	}
	Vector3 oldPos;
	void UpdateAnimation()
	{
		isMoving = oldPos != agent.transform.position;
		if (State != BasicMeleeStates.Attacking)
		{
			if (isMoving)
			{
				if (!entity.UseMecanim)
				{
					if (State != BasicMeleeStates.Wandering)
					{
						Owner.animation.CrossFade(Owner.GetComponent<BasicEntity>().Animations.RunAnimation.name);
						agent.speed = entity.RunSpeed;
					}
					else
					{
						Owner.animation.CrossFade(Owner.GetComponent<BasicEntity>().Animations.WalkAnimation.name);
						agent.speed = entity.WalkSpeed;
					}
				}
			}
			else
			{
				if (!entity.UseMecanim)
				{
					Owner.animation.CrossFade(Owner.GetComponent<BasicEntity>().Animations.IdleAnimation.name);
				}
			}
		}
		oldPos = Owner.transform.position;



	}

	void AlertOthers()
	{
		List<BasicEntity> nearbyAllies = entity.GetNearbyLikeMe (15);
		foreach(BasicEntity e in nearbyAllies)
		{
			if (e.hasBehaviour(typeof(BasicMeleeBehaviour)) != null)
			{
				(e.hasBehaviour(typeof(BasicMeleeBehaviour)) as BasicMeleeBehaviour).PlayerTarget = PlayerTarget;
				(e.hasBehaviour(typeof(BasicMeleeBehaviour)) as BasicMeleeBehaviour).State = BasicMeleeStates.Attacking;
			}
		}
	}

	float _fleeingTimer = 0;
	bool canFlee = true;
	float _fleeResetTimer = 0;
	void Think()
	{
		if (State == BasicMeleeStates.Fleeing)
		{
			_fleeingTimer += Time.deltaTime;
			if (_fleeingTimer > 4)
			{
				_fleeingTimer = 0;
				State = BasicMeleeStates.Standing;
				_timer = 0;
			}
			return;
		}
		_timer += Time.deltaTime;
		if (!canFlee)
		{
			_fleeResetTimer += Time.deltaTime;
			if (_fleeResetTimer > 15)
			{
				canFlee = true;
				_fleeResetTimer = 0;
			}
		}
		if (_timer > thinkingTimer)
		{ // It's time to think of something to do
			_timer = 0;

			if (State != BasicMeleeStates.Attacking)
			{
				// We are not under attack
				//GameObject[] players = GameHelper.AnyPlayerNearby( Owner );
				if (playerNearby)
				{
					PlayerTarget = GameHelper.GetLocalPlayer();
					AlertOthers();
					State = BasicMeleeStates.Chasing;
				}
				else
				{
					if (!UnderAttack)
					{
						State = BasicMeleeStates.Wandering;
					}
					else
					{
						if (badlyInjured)
						{
							CallForHelp();

						}
						else
						{
							InitiateAttack();
						}
					}
				}
			}

			else
			{
				if (PlayerFleed)
				{
					standing = true;
					State = BasicMeleeStates.Standing;
				}
				else
				{
					if (badlyInjured)
					{
						CallForHelp();
					}
				}
			}
		}
	}

	bool PlayerFleed { get { return (Vector3.Distance (PlayerTarget.transform.position, Owner.transform.position) > 16); } }
	void CallForHelp() {
		if (nearDeath)
		{
			InitiateFlee();
		}
		else
		{
			InitiateAttack();
		}
	}
	void InitiateFlee() { 
		if (canFlee)
		{
			State = BasicMeleeStates.Fleeing; 
			canFlee = false;
		}
	}
	void InitiateAttack() { State = BasicMeleeStates.Attacking; }

	/* STANDING */
	public float standingTimer = 6;
	public float _stndngTmr = 0;
	bool standing = false;
	public void Stand()
	{
		_stndngTmr += Time.deltaTime;
		if (_stndngTmr > standingTimer)
		{
			_stndngTmr = 0;
			standing = false;
		}
	}
	/* END STANDING */

	/* WANDERING */
	NavMeshPath currentWanderingPath;
	public float WanderDistance = 5;
	bool targetFound = false;
	bool onPath = false;
	bool waitArrival = false;
	float _waitTimer = 0;
	Transform[] wayPoints;
	IEnumerator waitForArrival()
	{		

		yield return true;
	}

	public void Wander()
	{
		return;
		if (!targetFound)
		{
			_waitTimer += Time.deltaTime;
			if (_waitTimer > 2)
			{
				_waitTimer = 0;
				wayPoints = (from o in GameObject.Find("WanderingWayPoints").GetComponentsInChildren<Transform>() orderby Vector3.Distance(o.transform.position, Owner.transform.position) select o).ToArray();
				NpcWaypoint currentWayPoint = wayPoints[0].GetComponent<NpcWaypoint>();
				if (currentWayPoint == null)
					return;
				Vector3 t = currentWayPoint.ConnectsTo[ Random.Range(0, currentWayPoint.ConnectsTo.Count-1) ].transform.position;
				Vector3 dir = t - entity.transform.position;
				dir.Normalize();
				agent.SetDestination( agent.transform.position + dir * Random.Range(MinWanderRange, MaxWanderRange));
				targetFound = true;
				waitArrival = true;
			}

		}
		else if (targetFound)
		{
			if (agent.hasPath && agent.remainingDistance < 1)
			{
				Debug.Log("Sono arrivato!");
				targetFound = false;
				onPath = false;
				standing = true;
				State = BasicMeleeStates.Standing;
			}

		}
	}
	/* END WANDERING */

	/* CHASING */
	public float chasingDistance = 2;
	public float abandonDistance = 15;
	public void Chase()
	{
		if (GameHelper.GetPlayerComponent<EntityStatus>().HasBuff(typeof(HiddenBuff)))
		{
			standing = true;
			State = BasicMeleeStates.Standing;
			return;
		}

		if (Vector3.Distance(PlayerTarget.transform.position, Owner.transform.position) > abandonDistance)
		{
			standing = true;
			State = BasicMeleeStates.Standing;
		}
		else if (Vector3.Distance(PlayerTarget.transform.position, Owner.transform.position) > chasingDistance)
		{
			Vector3 targetPoint = PlayerTarget.transform.position + PlayerTarget.GetComponent<NavMeshAgent>().velocity * 0.4f;
			agent.SetDestination(targetPoint);
		}
		else
		{
			InitiateAttack();
		}

	}
	/* END CHASING */

	/* FLEE */

	void Flee()
	{
		if (Vector3.Distance(Owner.transform.position, GameHelper.GetLocalPlayer().transform.position) < abandonDistance)
		{
			Vector3 t = Owner.transform.position - (GameHelper.GetLocalPlayer().transform.position - Owner.transform.position).normalized * 2;
			agent.SetDestination(t);
		}
		else
		{
			agent.Stop ();
			standing = true;
			State = BasicMeleeStates.Standing;
		}
	}

	/* END FLEE */

	/* ATTACKING */
	float _attackTimer = 0;
	public float AttackDelay = 2;
	bool canAttack = true;
	void WaitForAttack()
	{
		_attackTimer += Time.deltaTime;
		if (_attackTimer > AttackDelay)
		{
			_attackTimer = 0;
			canAttack = true;
		}
	}

	public void Attack()
	{
		agent.Stop ();
		if (PlayerTarget == null)
			PlayerTarget = GameHelper.GetLocalPlayer();
		if (PlayerTarget.GetComponent<EntityStatus>().HasBuff(typeof(HiddenBuff)))
		{
			State = BasicMeleeStates.Standing;
			return;
		}
		if (Vector3.Distance(Owner.transform.position, PlayerTarget.transform.position) < chasingDistance)
		{
			if (canAttack)
			{
				if (!entity.UseMecanim)
				{
					Owner.animation.CrossFade(Owner.GetComponent<BasicEntity>().Animations.AttackAnimation.name);
				}
				BaseWeapon r = entity.RightHand;
				EntitySkills sk = entity.GetComponent<EntitySkills>();
				if (sk != null)
				{
					if (sk.SkillCheckSuccessful( r.WeaponDamage.skillUsed, r.WeaponDamage.skillNeeded, 1))
					{
						float d = r.WeaponDamage.GetFinalDamage();
						PlayerTarget.GetComponent<EntityStatus>().Damage(d);
					}
					else
						PlayerTarget.GetComponent<EntityStatus>().Damage(0);

				}

				canAttack = false;
			}
			else
			{
				WaitForAttack();
			}
		}
		else
		{
			State = BasicMeleeStates.Chasing;

		}
	}

	/* END ATTACKING */
}
