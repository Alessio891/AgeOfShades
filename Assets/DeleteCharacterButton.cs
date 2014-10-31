using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class DeleteCharacterButton : MonoBehaviour, IPointerClickHandler {
	MainMenuManager Manager;
	Character c;
	public int charSlot = 0;
	// Use this for initialization
	void Start () {
		Manager = GameObject.FindObjectOfType (typeof(MainMenuManager)) as MainMenuManager;

	}
	
	// Update is called once per frame
	void Update () {
		switch(charSlot)
		{
		case 0:
			c = Manager.Char1;
			break;
		case 1:
			c = Manager.Char2;
			break;
		case 2:
			c = Manager.Char3;
			break;
		}
		if (c.Created && Manager.state == 1)
		{
			GameHelper.ShowMenu(GameObject.Find("deletebutton" + charSlot.ToString()));
		}
		else
			GameHelper.HideMenu(GameObject.Find("deletebutton" + charSlot.ToString()));
	}

	public void OnPointerClick(PointerEventData data)
	{
		File.Delete(Application.persistentDataPath + "/Char" + charSlot.ToString() + ".char");
		File.Delete (Application.persistentDataPath + "/" + c.Name + ".gd");
		c.Created = false;
		switch(charSlot)
		{
		case 0:
			Manager.Char1 = c;
			break;
		case 1:
			Manager.Char2 = c;
			break;
		case 2:
			Manager.Char3 = c;
			break;
		}
	}
}
