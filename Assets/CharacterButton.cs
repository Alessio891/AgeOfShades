using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour, IPointerClickHandler {
	MainMenuManager m;
	public int charSlot = 0;
	Character c;

	// Use this for initialization
	void Start () {
		m = GameObject.Find ("Manager").GetComponent<MainMenuManager> ();
	}

	public void OnPointerClick(PointerEventData data)
	{
		switch(charSlot)
		{
		case 0:
			c = m.Char1;
			break;
		case 1:
			c = m.Char2;
			break;
		case 2:
			c = m.Char3;
			break;
		}
		if (!c.Created)
		{
			GameHelper.HideMenu (GameObject.Find ("CharacterList"));
			GameHelper.ShowMenu (GameObject.Find ("CharacterCreation"));
			m.PlayerMesh.SetActive (true);
			(GameObject.FindObjectOfType (typeof(CustomizeButton)) as CustomizeButton).HideAll ();
			m.CurrentSlot = charSlot;
			m.state = 2;
				
		}
		else
		{
			CharManager.manager.character = c;
			Application.LoadLevel("LoadingScreen");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
