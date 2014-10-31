using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizeButton : MonoBehaviour, IPointerClickHandler {

	// Use this for initialization
	void Start () {
		HideAll ();
	}
	bool visible = false;

	public void HideAll()
	{
	
		GameHelper.HideMenu (GameObject.Find ("HairPicker"));
		GameHelper.HideMenu (GameObject.Find ("ComingSoon"));
		GameHelper.HideMenu (GameObject.Find ("SkillPicker"));
		GameHelper.HideMenu (GameObject.Find ("ShirtPicker"));
		GameHelper.HideMenu (GameObject.Find ("PantsPicker"));
		GameHelper.HideMenu (GameObject.Find ("BootsPicker"));
		
		
	}
	public void DisactivateAllButtons()
	{
		foreach(Object o in GameObject.FindObjectsOfType(typeof(CustomizeButton)))
		{
			(o as CustomizeButton).GetComponentInChildren<Graphic>().color = Color.white;
		}

	}

	public void OnPointerClick(PointerEventData data)
	{
		HideAll ();
		DisactivateAllButtons ();
		GetComponentInChildren<Graphic> ().color = Color.blue;
		/*if (name == "Capelli")
		{
			if (!visible)
			{
				HideAll();

				GameHelper.ShowMenu (GameObject.Find ("ColorPicker"));
				GameHelper.ShowMenu (GameObject.Find ("HairPicker"));
				ColorPicker c = GameObject.Find ("ColorPicker").GetComponent<ColorPicker> ();
				c.OnChange = () => {
					Color col = new Color(c.Red.value, c.Green.value, c.Blue.value);
					PlayerMeshManager m = GameObject.Find("PaperdollMesh").GetComponent<PlayerMeshManager>();
					if (m.Skins.Hair != null)
						m.ChangeColor(ref m.Skins.Hair, col);
				};
				visible = true;
			}
			else
			{
				GameHelper.HideMenu (GameObject.Find ("ColorPicker"));
				GameHelper.HideMenu (GameObject.Find ("HairPicker"));
				ColorPicker c = GameObject.Find ("ColorPicker").GetComponent<ColorPicker> ();
				c.OnChange = null;
				visible = false;
			}
		}*/
		if (name == "Razza")
		{
		}
		else if (name == "Abilita")
		{
			GameHelper.ShowMenu(GameObject.Find("SkillPicker"));
		}
		else if (name == "Aspetto")
		{

			GameHelper.ShowMenu (GameObject.Find ("LookEditor"));
		}
		else if (name == "Maglia")
		{

			GameHelper.ShowMenu (GameObject.Find ("ColorPicker"));
			GameHelper.ShowMenu (GameObject.Find ("HairPicker"));
			ColorPicker c = GameObject.Find ("ColorPicker").GetComponent<ColorPicker> ();
			c.OnChange = () => {
				Color col = new Color(c.Red.value, c.Green.value, c.Blue.value);
				PlayerMeshManager m = GameObject.Find("PaperdollMesh").GetComponent<PlayerMeshManager>();
				if (m.Skins.Torso != null)
				m.ChangeColor(ref m.Skins.Torso, col);
			};
		}
		else if (name == "Pantaloni")
		{
			HideAll();
			
			GameHelper.ShowMenu (GameObject.Find ("ColorPicker"));
			GameHelper.ShowMenu (GameObject.Find ("HairPicker"));
			ColorPicker c = GameObject.Find ("ColorPicker").GetComponent<ColorPicker> ();
			c.OnChange = () => {
				Color col = new Color(c.Red.value, c.Green.value, c.Blue.value);
				PlayerMeshManager m = GameObject.Find("PaperdollMesh").GetComponent<PlayerMeshManager>();
				if (m.Skins.Legs != null)
					m.ChangeColor(ref m.Skins.Legs, col);
			};
		}
		else if (name == "Stivali")
		{
			HideAll();
			
			GameHelper.ShowMenu (GameObject.Find ("ColorPicker"));
			GameHelper.ShowMenu (GameObject.Find ("HairPicker"));
			ColorPicker c = GameObject.Find ("ColorPicker").GetComponent<ColorPicker> ();
			c.OnChange = () => {
				Color col = new Color(c.Red.value, c.Green.value, c.Blue.value);
				PlayerMeshManager m = GameObject.Find("PaperdollMesh").GetComponent<PlayerMeshManager>();
				if (m.Skins.Feet != null)
				m.ChangeColor(ref m.Skins.Feet, col);
			};
		}
		else
		{
			GameHelper.ShowMenu (GameObject.Find("ComingSoon"));
		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
