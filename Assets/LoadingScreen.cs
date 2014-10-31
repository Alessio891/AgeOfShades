using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public bool LevelTransition = false;
	public static LoadingScreen instance;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		if (CharManager.manager.character == null)
		{
			CharManager.manager.character = new Character();
			CharManager.manager.character.LevelTransition = true;
		}
		if (!CharManager.manager.character.LevelTransition)
		{
			if (SaveLoad.SaveExists(CharManager.manager.character.Name))
			{
				CharManager.manager.character = SaveLoad.LoadCharacter("Char" + CharManager.manager.character.Slot.ToString());
				SaveLoad.Load (CharManager.manager.character.Name);
			}
			else
			{

				SaveLoad.Save("Cave_terrain", CharManager.manager.character.Name);

				SaveLoad.Load(CharManager.manager.character.Name, true);
			}
		}
		else
		{
			Application.LoadLevel(CharManager.manager.character.SceneName);
			CharManager.manager.character.LevelTransition = false;
			Destroy(gameObject);
		}
	}


}
