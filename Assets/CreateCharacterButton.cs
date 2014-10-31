using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateCharacterButton : MonoBehaviour, IPointerClickHandler {

	public MainMenuManager Manager;
	public InputField Name;
	public int charSlot = 0;
	Character c;
	// Use this for initialization
	void Start () {

	}

	public void OnPointerClick(PointerEventData data)
	{
		Debug.Log ("Here the color is: " + Manager.MeshManager.SkinColor);
		if (string.IsNullOrEmpty(Name.value ))
		{
			Name.GetComponent<Image>().CrossFadeColor(Color.red, 0.2f, false, false);
			return;
		}

		charSlot = Manager.CurrentSlot;
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
		c = new Character ();
		c.Name = Name.value;
		c.startingSkills = new string[3];
		c.startingSkills [0] = GameObject.Find ("SkillSlot1").GetComponent<CreateChar_SkillSlot> ().skill;
		c.startingSkills [1] = GameObject.Find ("SkillSlot2").GetComponent<CreateChar_SkillSlot> ().skill;
		c.startingSkills [2] = GameObject.Find ("SkillSlot3").GetComponent<CreateChar_SkillSlot> ().skill;
		for(int i = 0; i < 3; i++)
		{
			if (string.IsNullOrEmpty(c.startingSkills[i]))
			{
				GameObject.Find("SkillSlot"+ (i+1).ToString()).GetComponent<Graphic>().CrossFadeColor(Color.red, 0.2f, false, false);
				return;
			}
		}
		c.SkinColor = new SerializedColor (Manager.MeshManager.SkinColor);
		if (Manager.MeshManager.Skins.Hair != null)
			c.HairColor = new SerializedColor(Manager.MeshManager.GetPieceColor (ref Manager.MeshManager.Skins.Hair));
		else
			c.HairColor = new SerializedColor(Color.white);
		c.BootsColor = new SerializedColor (Manager.MeshManager.GetPieceColor (ref Manager.MeshManager.Skins.Feet));
		c.PantsColor = new SerializedColor (Manager.MeshManager.GetPieceColor (ref Manager.MeshManager.Skins.Legs));
		c.ShirtColor = new SerializedColor (Manager.MeshManager.GetPieceColor (ref Manager.MeshManager.Skins.Torso));
		c.Created = true;
		c.Slot = charSlot;
		c.HairPrefab = CharManager.manager.character.HairPrefab;

//		if (Manager.MeshManager.Skins.Hair.Count > 0)
//			c.HairPrefab = Manager.MeshManager.Skins.Hair [0].name;
//		else
//			c.HairPrefab = null;
		c.JustCreated = true;

	

		CharManager.manager.character = c;
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

		SaveLoad.SaveCharacter ("Char"+charSlot.ToString());
		Manager.PlayerMesh.SetActive (false);
		GameHelper.HideMenu (GameObject.Find ("CharacterCreation"));
		GameHelper.ShowMenu (GameObject.Find ("CharacterList"));

	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
