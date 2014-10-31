using UnityEngine;
using System.Collections;

public class Lars_BuildCamp : BaseBehaviour, FriendlyBehaviour {

	float _timer = 0;
	float _buildTimer = 0;
	NavMeshAgent agent;
	Animator animator;
	Vector3 lastPos;
	bool isMoving = false;
	string currentAction = "None";

	public Lars_BuildCamp(GameObject owner) : base(owner)
	{
		agent = owner.GetComponent<NavMeshAgent> ();
		lastPos = owner.transform.position;
		animator = owner.GetComponent<Animator> ();
	}

	bool woodPlaced = false;
	GameObject wood;
	public override void Update ()
	{
		if (lastPos != Owner.transform.position)
			isMoving = true;
		else
			isMoving = false;

		_timer += Time.deltaTime;

			Think();

		lastPos = Owner.transform.position;

		if (!isMoving)
		{
			if (animator.GetLayerWeight(1) > 0.8f)
				animator.SetLayerWeight(1, 0);
		}
		else
		{
			if (animator.GetLayerWeight(1) > 0.8f)
				animator.SetLayerWeight(1, 0);
		}

		if (currentAction== "Running")
		{
			if (animator.GetLayerWeight(1) > 0.8f)
				animator.SetLayerWeight(1, 0);
		}
		else if (currentAction == "Build")
		{
			if (animator.GetLayerWeight(1) < 0.1f)
				animator.SetLayerWeight(1, 1);

		}

	}

	public override void Think ()
	{
		switch (currentAction)
		{
		case "None":
			Vector3 pos;
			pos = GameObject.Find("NPCWayPoint" + Random.Range(0, 3).ToString()).transform.position;
			while(Vector3.Distance(Owner.transform.position, pos) < 0.2f)
			{
				pos = GameObject.Find("NPCWayPoint" + Random.Range(0, 3).ToString()).transform.position;
				
			}

			agent.SetDestination( pos);
			currentAction = "Walking";
			break;
		case "Walking":
			if (woodPlaced)
			{
				GameObject.Destroy(wood, 1);
				woodPlaced = false;
			}
			if (agent.remainingDistance == 0)
			{
				currentAction = "Build";
			}
			break;
		case "Build":
			if (!woodPlaced)
			{
				woodPlaced = true;
				wood = GameObject.Instantiate( Resources.Load("BasicWoodLog") ) as GameObject;
				wood.transform.position = Owner.transform.position + Owner.transform.forward * 2;
			}

			_buildTimer+= Time.deltaTime;
			if (_buildTimer > 8)
			{
				_buildTimer = 0;
				currentAction = "None";
			}
			break;
		}
	}
}
