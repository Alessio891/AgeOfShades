using UnityEngine;
using System.Collections;

public class BasicDialogueNPC : BasicNPC {

	public TextAsset Dialogo;
	protected bool DialogueOpen { get { return false; } }

	public override void OnSelect ()
	{
		Debug.Log ("Selected");
		DialogueParser.GetDialogue (Dialogo, "Test_Root");
		if (!DialogueOpen)
		{
			if (PlayerIsNearby(2))
			{
				GameHelper.SystemMessage("Ciao stronzo!", Color.white);
				InititateDialogue();
			}
		}
	}

	protected virtual void InititateDialogue()
	{
		//string[] message = Dialogo.text.Split (new string[] { "<br>" }, System.StringSplitOptions.RemoveEmptyEntries);

		DialogueManager.instance.ShowDialogue (Dialogo, "Test_Root");
	}
}
