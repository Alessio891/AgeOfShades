using UnityEngine;
using System.Collections;

public class LeatherPants : BaseArmor {

	protected override void Start ()
	{
		Prefab = Resources.Load ("LeatherPants") as GameObject;
		ItemName = "Pantaloni di pelle";
		ArmorSlot = ArmorSlots.Legs;
		ItemIcon = "Pants";
		ItemIconAtlas = "";
		//Tint = new Color (0.4f, 0.1f, 0.8f);
	}


}
