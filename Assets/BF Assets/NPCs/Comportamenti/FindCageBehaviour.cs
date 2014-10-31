using UnityEngine;
using System.Collections;
using System.Linq;

public class FindCageBehaviour : BaseBehaviour {

	float distance = 30;
	GameObject[] cages;
	float _timer = 0;
	public FindCageBehaviour(GameObject owner) : base(owner)
	{
		cages = (from o in GameObject.FindGameObjectsWithTag ("Gabbia") orderby Vector3.Distance (Owner.transform.position, o.transform.position) select o).ToArray ();
	}

	bool cageFound = false;

	public override void Update ()
	{
		base.Update ();
		_timer += Time.deltaTime;
		if (_timer > 1)
		{
			cages = (from o in GameObject.FindGameObjectsWithTag ("Gabbia") orderby Vector3.Distance (Owner.transform.position, o.transform.position) select o).ToArray ();
			_timer = 0;
		}
		//GameObject[] cages = (from o in GameObject.FindGameObjectsWithTag ("Gabbia") orderby Vector3.Distance (Owner.transform.position, o.transform.position) select o).ToArray ();
		if (!cageFound && cages.Length > 0 && Vector3.Distance(cages[0].transform.position, Owner.transform.position) < distance)
		{

			if (cages[0].GetComponent<Gabbia>().FoodInside)
			{
				for(int i = 0; i < Owner.GetComponent<BasicEntity>().Behaviours.Count; i++)
				{
					BaseBehaviour b = Owner.GetComponent<BasicEntity>().Behaviours[i];
					if (b != this)
					{
						//Owner.GetComponent<BasicEntity>().Behaviours.RemoveAt(i);
						b.RemoveAtTheEndOfFrame = true;
					}
				}
				cageFound = true;
				Owner.GetComponent<NavMeshAgent>().SetDestination(cages[0].transform.position);
				if (Owner.GetComponent<BasicEntity>().Animations.WalkAnimation != null)
					Owner.animation.Play(Owner.GetComponent<BasicEntity>().Animations.WalkAnimation.name);
			}
		}
		if (cageFound)
		{
			if (Owner.GetComponent<NavMeshAgent>().remainingDistance == 0)
			{
				cages[0].GetComponent<Gabbia>().capturedWhat = Owner;
				cages[0].GetComponent<Gabbia>().Captured = true;
			}
		}

	}

}
