using UnityEngine;
using System.Collections;

public class BasicTunic : BaseArmor {

	protected override void Start ()
	{
		
		Prefab = Resources.Load ("Cloth1") as GameObject;
		
		ArmorSlot = ArmorSlots.Torso;

		ItemName = "Tunica";
	}

}
