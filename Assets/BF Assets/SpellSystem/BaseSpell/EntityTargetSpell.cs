using UnityEngine;
using System.Collections;

public class EntityTargetSpell : GenericTargetSpell {

	public override void WaitForTarget ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit))
			{
				if (hit.collider.GetComponent<BasicEntity>() != null || hit.collider.GetComponent<EntityStatus>() != null)
				{
					OnTargetSelect(hit.collider.gameObject);
					_waitForTarget = false;
				}
			}
		}
	}	

	public override void OnTargetSelect (object target)
	{
		ClickToMove cm = Caster.Transform.GetComponent<ClickToMove> ();
		if (cm != null)
			cm.Targeting = false;
	}
}
