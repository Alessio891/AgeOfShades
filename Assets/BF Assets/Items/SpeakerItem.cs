using UnityEngine;
using System.Collections;

public class SpeakerItem : BasicItem {
	public string Text;

	public override void OnUse ()
	{
		GameHelper.ShowNotice (Text, gameObject);
	}

	public override void OnSelect ()
	{
		OnUse ();
	}
}
