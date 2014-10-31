using UnityEngine;
using System.Collections;
[System.Serializable]
public class Boots : BaseArmor {

	protected override void Start ()
	{
		ArmorSlot = ArmorSlots.Feet;
		ItemName = "Stivali";
		Prefab = Resources.Load ("Boots") as GameObject;
	}

}
