using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingListItemButton : MonoBehaviour {

	GameObject Player;
	public BasicCraftable Item;
	bool tooltip = false;

	public UnityEngine.UI.Image Icon;
	public GameObject grid;

	// Use this for initialization
	void Start () {

		Player = GameObject.FindGameObjectWithTag ("Player");
		grid = transform.parent.gameObject;
		Icon = GameHelper.GetComponentInChildOf<UnityEngine.UI.Image> (gameObject, "Icon") as UnityEngine.UI.Image;
	}

	// Update is called once per frame
	void Update () {
		if (Item != null)
		{
			if (Icon.sprite == null || (Icon.sprite != null && Icon.sprite.name != Item.ItemIcon))
			{
				Icon.color = new Color(1,1,1,1);
				string path = "Icons/" + Item.ItemIcon;
				Sprite s = (Sprite)Resources.Load<Sprite>(path);
	
				Icon.sprite =  s;
			}
		}
	}


	public void MouseClick()
	{
		GameObject.Find("CraftingMenu").GetComponent<CraftingMenu> ().ShowInfo (gameObject);
	}
}
