using UnityEngine;
using System.Collections;
[System.Serializable]
public class SimplePants : BaseArmor {
	protected override void Start ()
	{
		ItemName = "Pantaloni";
		Prefab = Resources.Load ("SimplePants") as GameObject;
		ItemIcon = "Pants";
		ArmorSlot = ArmorSlots.Pants;
	}
}
