using UnityEngine;
using System.Collections;

public class GenericTargetSpell : CastTimeSpell, ITargetSpell {

	protected bool _waitForTarget = false;
	public bool WaitingForTarget { get { return _waitForTarget; } }


	// Use this for initialization
	void Start () {

	}

	public virtual void OnTargetSelect(object target)
	{

	}

	public virtual void WaitForTarget()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit))
			{
				if (hit.collider.name == "Terrain")
				{
					OnTargetSelect(hit.point);
					_waitForTarget = false;
				}
			}
		}
	}

	public override void Cast (ICaster caster)
	{
		base.Cast (caster);
		if (!weCanCast)
			return;
		_waitForTarget = true;
		GameHelper.SystemMessage ("Seleziona un punto:", Color.yellow);
		(GameHelper.GetPlayerComponent<ClickToMove> () as ClickToMove).Targeting = true;
	}

	public override void FinalizeCasting ()
	{
		base.FinalizeCasting ();
	}


	

	// Update is called once per frame
	void Update () {
		if (WaitingForTarget)
		{
			WaitForTarget();
		}
	}
}
