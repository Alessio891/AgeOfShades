using UnityEngine;
using System.Collections;
[System.Serializable]
public class CorpoACorpo : Skill {

	public CorpoACorpo()
	{
		Value = 20;
		Name = "Corpo A Corpo";
		AdvancementSpeed = 5;

		BonusItems.Add( typeof(Spada_lunga));
	}
}
