using UnityEngine;
using System.Collections;
using System.Linq;

public class ScreamOnHostile : BaseBehaviour {

	public ScreamOnHostile(GameObject owner) : base(owner) {}

	bool screamed = false;
	float _timer = 0;

	GameObject nearestEnemy
	{
		get
		{
			return (from o in GameObject.FindGameObjectsWithTag("SimpleEnemy") orderby Vector3.Distance( Owner.transform.position, o.transform.position ) select o).FirstOrDefault();
		}
	}

	public override void Update ()
	{
		base.Update ();
		if (!screamed)
		{
		 	if (nearestEnemy != null)
			{
				if (Vector3.Distance(Owner.transform.position, nearestEnemy.transform.position) < 10)
				{
					PlayerCombat c = (PlayerCombat)GameHelper.GetPlayerComponent<PlayerCombat>();
					if (c.Target != null && c.Target.GetInstanceID() == nearestEnemy.GetInstanceID())
					{
						GameHelper.ShowNotice("VAI COSI! DAGLIELE DI SANTA RAGIONE FRATELLO!", Owner);
					}
					else
					{
						GameHelper.ShowNotice("UN MOSTRO!!!", Owner);
					}
					screamed = true;
				}
			}
		}
		else
		{
			_timer += Time.deltaTime;
			if (_timer > 5)
			{
				screamed = false;
				_timer = 0;
			}
		}
	}
}
