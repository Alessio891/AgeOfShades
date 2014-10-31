using UnityEngine;
using System.Collections;

public interface FriendlyBehaviour
{

}

public interface HostileBehaviour
{
}

public class BaseBehaviour {

	public GameObject Owner;
	protected bool isMoving = false;
	protected Vector3 lastPos;
	public bool RemoveAtTheEndOfFrame = false;
	protected NavMeshAgent agent;
	protected BasicEntity entity;

	public BaseBehaviour() {}

	public BaseBehaviour(GameObject owner)
	{
		Owner = owner;
		OnBehaviourStart ();
		lastPos = owner.transform.position;
		agent = Owner.GetComponent<NavMeshAgent> ();
		entity = Owner.GetComponent<BasicEntity> ();
	}

	public bool CanSeePlayer { 
		get 		
		{
			
			Ray r = new Ray(Owner.transform.position + new Vector3(0, 0.3f, 0), GameHelper.GetLocalPlayer().transform.position - Owner.transform.position);
			float dist = Vector3.Distance(Owner.transform.position, GameHelper.GetLocalPlayer().transform.position);
			Debug.DrawRay(r.origin, r.direction * dist, Color.red);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit, Vector3.Distance(Owner.transform.position, GameHelper.GetLocalPlayer().transform.position)))
			{
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BlockVisibility"))			
					return false;
			}
			return true;
		}
	}

	protected NavMeshPath currentPath = new NavMeshPath();

	public bool CanGoThere(Vector3 point)
	{
		bool value = false;
		
		agent.CalculatePath (point, currentPath);
		
		return (agent.path.status != NavMeshPathStatus.PathPartial);
		
	}

	public virtual void Update()
	{
		isMoving = lastPos.AlmostEquals (Owner.transform.position, 0.1f);
		
		lastPos = Owner.transform.position;
	}

	public void FollowPath(Vector3[] path)
	{

	}

	public virtual void Think()
	{
	}

	public virtual void OnBehaviourStart()
	{
	}

	public virtual void OnBehaviourEnd()
	{
	}

}
