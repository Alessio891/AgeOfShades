using UnityEngine;
using System.Collections;

public class Door : BasicItem {

	public bool Locked = false;
	public bool Opened = false;
	public GameObject doorMesh;

	public string Key;
	public bool ConsumeKey = true;

	public Vector3 Rotation = new Vector3(270, 0, 0);
	public Vector3 ClosedRotation = new Vector3(270, 90, 0);
	protected override void Init ()
	{

	}
	protected override void UpdateLogic ()
	{

	}

	public override void OnUse ()
	{
		if (!Locked)
		{
			Open ();
		}
		else
		{
			if (!string.IsNullOrEmpty(Key))
			{
				PlayerInventory pi = (PlayerInventory)GameHelper.GetPlayerComponent<PlayerInventory>();
				if (pi.Has(Key, 1))
				{
					GameHelper.ShowNotice("Hai usato la chiave!");
					Locked = false;
					if (ConsumeKey)
						pi.ConsumeObject(Key, 1);
					return;
				}
			}
			GameHelper.ShowNotice("La porta è chiusa a chiave...", gameObject);
		}
	}

	bool wasStayingStill = false;
	Ray ray;
	void OnTriggerStay(Collider c)
	{
		if (Opened)
			return;
		NavMeshAgent agent = GameHelper.GetPlayerComponent<NavMeshAgent>() as NavMeshAgent;
		if (agent.hasPath)
		{
			Ray ray = new Ray(agent.transform.position + new Vector3(0,0.2f, 0), agent.destination - agent.transform.position);
			RaycastHit hit;
			Debug.DrawRay (ray.origin, ray.direction * 0.3f, Color.blue, 0.1f);
			
			if (Physics.Raycast(ray, out hit, 0.3f))
			{
				
				if (hit.collider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
				{

					agent.Stop();
				}
			}
		}
	}

	void OnDrawGizmos()
	{
	}

	void OnTriggerEnter(Collider c)
	{
		if (!Opened || Locked)
		{
			if (c.gameObject.GetComponent<NavMeshAgent>() != null)
				c.gameObject.GetComponent<NavMeshAgent>().Stop();
		}
	}

	void Open()
	{
		if (!Opened)
		{
			iTween.Stop (gameObject);
			iTween.RotateTo (doorMesh, iTween.Hash ("rotation", Rotation, "easeType", "easeInOutSine", "time", 2));
			GetComponent<NavMeshObstacle>().enabled = false;
			Opened = true;
		}
		else
		{
			iTween.Stop (gameObject);
			iTween.RotateTo (doorMesh, iTween.Hash ("rotation", ClosedRotation, "easeType", "easeInOutSine", "time", 2));
			GetComponent<NavMeshObstacle>().enabled = true;
			Opened = false;
		}
	}
}
	