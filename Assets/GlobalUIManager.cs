using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GlobalUIManager : MonoBehaviour {
	public GameObject Inventory;
	public GameObject Skills;
	public GameObject Crafting;
	public GameObject QuickSlots;
	public GameObject WarningMessages;
	public GameObject _Paperdoll;
	public GameObject Spellbook;
	public GameObject SystemMessage;

	public bool HideAll;
	public bool ShowAll;

	public static GlobalUIManager instance;

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}

	public void HideA()
	{
		HideAll = false;
		GameHelper.HideMenu(Inventory);
		GameHelper.HideMenu(Skills);
		GameHelper.HideMenu (Crafting);
		GameHelper.HideMenu(QuickSlots);
		GameHelper.HideMenu(WarningMessages);
		GameHelper.HideMenu(Spellbook);
		GameHelper.HideMenu(SystemMessage);
		_Paperdoll.GetComponent<Paperdoll>().Hide();	
	}

	public void ShowA()
	{
		ShowAll = false;
		GameHelper.ShowMenu(Inventory);
		GameHelper.ShowMenu(Skills);
		GameHelper.ShowMenu (Crafting);
		GameHelper.ShowMenu(QuickSlots);
		GameHelper.ShowMenu(WarningMessages);
		GameHelper.ShowMenu(Spellbook);
		GameHelper.ShowMenu(SystemMessage);
		_Paperdoll.GetComponent<Paperdoll>().Show();

	}

	void Update () {
		if (HideAll)
		{
			HideA();
		}
		else if (ShowAll)
		{
			ShowA();
		}
	}
}
