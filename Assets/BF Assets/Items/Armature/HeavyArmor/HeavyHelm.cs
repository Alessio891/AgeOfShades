using UnityEngine;
using System.Collections;

public class HeavyHelm : BaseArmor {

	protected override void Start ()
	{
		ArmorSlot = ArmorSlots.Head;
		Prefab = Resources.Load ("HeavyHelm") as GameObject;

		ItemName = "Elmo Pesante";
		ItemIcon = "Ascia";
	}

}
