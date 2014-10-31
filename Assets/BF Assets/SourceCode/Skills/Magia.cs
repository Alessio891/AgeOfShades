using UnityEngine;
using System.Collections;

[System.Serializable]
public class Magia : Skill {

	public Magia()
	{
		Name = "Magia";
		Value = 20;
		AdvancementSpeed = 1;

		BonusItems.Add (typeof(SpellBookItem));
	}
}

