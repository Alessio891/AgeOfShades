using UnityEngine;
using System.Collections;

[System.Serializable]
public class Cultura : Skill, ITargetReceiver
{
	public Cultura()
	{
		Name = "Cultura";
		Value = 30;
		AdvancementSpeed = 1;
		CanBeUsed = true;
		BonusItems.Add (typeof(Fungo));
	}
	
	public void OnTargetReceived(RaycastHit hit)
	{
		if (hit.collider.gameObject.GetComponent<BasicEntity>() != null)
		{
			GameHelper.SystemMessage("Inizi a studiare " + hit.collider.name + "...", Color.blue);
		}
		else if (hit.collider.gameObject.GetComponent<BasicItem>() != null)
		{
			GameHelper.SystemMessage(hit.collider.name + " sembra sia un " + hit.collider.gameObject.GetComponent<BasicItem>().Type.ToString() + ".", Color.white);
		}
	}
	
	public override void OnUse ()
	{
		base.OnUse ();
		(GameHelper.GetPlayerComponent<ClickToMove> () as ClickToMove).WaitForTarget (this);
	}
	
}