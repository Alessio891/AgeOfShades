using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {

	public Character Char1;
	public Character Char2;
	public Character Char3;

	public Text Char1Slot;
	public Text Char2Slot;
	public Text Char3Slot;

	public Animator CanvasAnimator;

	public int CurrentSlot = 0;

	public int state = 0; // 0 = start 1 = char list 2 = char creation

	public GameObject PlayerMesh;
	public PlayerMeshManager MeshManager;
	// Use this for initialization
	void Start () {
		Debug.Log (Application.persistentDataPath);
		DontDestroyOnLoad (CharManager.manager.gameObject);
		MeshManager = GameObject.FindObjectOfType (typeof(PlayerMeshManager)) as PlayerMeshManager;
		//MeshManager.RemoveAll ();
		MeshManager.Swap (Resources.Load ("SimpleShirt") as GameObject, ref MeshManager.Skins.Torso, Color.white);
		MeshManager.Swap (Resources.Load ("SimplePants") as GameObject, ref MeshManager.Skins.Legs, Color.black);
		MeshManager.Swap (Resources.Load ("Boots") as GameObject, ref MeshManager.Skins.Feet, Color.blue);
		MeshManager.Swap (MeshManager.BodyRef.Head, ref MeshManager.Skins.Head, MeshManager.SkinColor);
		MeshManager.Swap (MeshManager.BodyRef.Arms, ref MeshManager.Skins.Arms, MeshManager.SkinColor);
		MeshManager.Swap (MeshManager.BodyRef.Hands, ref MeshManager.Skins.Hands, MeshManager.SkinColor);
		MeshManager.Swap (MeshManager.BodyRef.Neck, ref MeshManager.Skins.Neck, MeshManager.SkinColor);
		MeshManager.Swap (MeshManager.BodyRef.Eyes, ref MeshManager.Skins.Eyes, Color.white);
		MeshManager.Swap (MeshManager.BodyRef.Eyebrows, ref MeshManager.Skins.Eyebrows, Color.white);
		PlayerMesh.SetActive (false);
		GameHelper.HideMenu (GameObject.Find ("CharacterList"));
		GameHelper.HideMenu (GameObject.Find ("CharacterCreation"));
		Character loadChar1 = SaveLoad.LoadCharacter ("Char0");
		Character loadChar2 = SaveLoad.LoadCharacter ("Char1");
		Character loadChar3 = SaveLoad.LoadCharacter ("Char2");
		if (loadChar1 != null)
		{
			Char1 = loadChar1;
			Char1.Created= true;
		}
		if (loadChar2 != null)
		{
			Char2 = loadChar2;
			Char2.Created = true;
		}
		if (loadChar3 != null)
		{
			Char3 = loadChar3;
			Char3.Created = true;
		}
	}

	public void Enter()
	{
		GameHelper.HideMenu (GameObject.Find ("MainMenu"));
		GameHelper.ShowMenu (GameObject.Find ("CharacterList"));
		state = 1;
	}
	public void Settings()
	{}
	public void Exit()
	{}


	// Update is called once per frame
	void Update () {
		if (Char1.Created)
		{
			Char1Slot.text = Char1.Name;
		}
		else
		{
			Char1Slot.text = "Vuoto";
		}

		if (Char2.Created)
		{
			Char2Slot.text = Char2.Name;
		}
		else
		{
			Char2Slot.text = "Vuoto";
		}

		if (Char3.Created)
		{
			Char3Slot.text = Char3.Name;
		}
		else
		{
			Char3Slot.text = "Vuoto";
		}
	}
	
}

[System.Serializable]
public class Character
{
	public bool Created = false;
	public bool LevelTransition = false;
	public string Name;

	public string SceneName;
	public bool JustCreated;

	public SerializedColor SkinColor;
	public SerializedColor ShirtColor;
	public SerializedColor HairColor;
	public SerializedColor PantsColor;
	public SerializedColor BootsColor;

	public string HairPrefab;

	public List<string> DoNotRespawnList = new List<string>();

	public int Slot;

	public string[] startingSkills;
	
}

[System.Serializable]
public class SerializedColor
{
	public float r;
	public float g;
	public float b;
	public float a;

	public SerializedColor(Color c)
	{
		FromColor(c);
	}

	public Color Color { get { return new Color (r, g, b, a); } }

	public void FromColor(Color c)
	{
		r = c.r;
		g = c.g;
		b = c.b;
		a = c.a;
	}
	
}