using UnityEngine;
using System.Collections;

public class RaycastTestBehaviour : BaseBehaviour {

	public RaycastTestBehaviour(GameObject owner) : base(owner) {}

	Ray ray;
	RaycastHit hit;
	Vector3 lastTarget = Vector3.zero;

	public float RayDistance = 10;

	public override void Update ()
	{
		// 1. Traccia raggio
		// 2. Esamina collisione
		// 3. Compi azione adeguata

		// 1. Traccia raggio
		ray = new Ray (Owner.transform.position + Owner.transform.up + (Owner.transform.forward * 0.3f), Owner.transform.forward);
		if (Physics.Raycast(ray, out hit, RayDistance))
		{
			// 2. Esamina collisione

			// 2a. Se c'è un nemico 
			if (hit.collider.gameObject.tag == "SimpleEnemy" || hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
			{
				if (agent.hasPath)
				{
					lastTarget = agent.destination;
				}
				agent.SetDestination(agent.transform.position + agent.transform.right);
			}

		}
	}

	void Move()
	{
	}

}
