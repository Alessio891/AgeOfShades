using UnityEngine;
using System.Collections;

public class LeatherChest : BaseArmor {

	protected override void Start ()
	{
		ItemName = "Tunica in pelle";
		ArmorSlot = ArmorSlots.Torso;
		Prefab = Resources.Load ("LeatherChest") as GameObject;
	}

}
