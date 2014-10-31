using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {

	public static void SaveCharacter(string fileName = "Character1")
	{
		Character character = CharManager.manager.character;
		character.SceneName = GameHelper.GetDataManager ().currentGame.SceneName;
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/" + fileName + ".char");
		bf.Serialize (file, character);
		file.Close ();
	}

	public static Character LoadCharacter(string fileName = "Character1")
	{
		if (File.Exists(Application.persistentDataPath + "/" + fileName + ".char"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + fileName + ".char", FileMode.Open);
			Character c = (Character)bf.Deserialize(file);
			file.Close();
			Debug.Log("Char loaded, skin color is " + c.SkinColor.Color);
			return c;
		}
		return null;
	}

	public static void Save(string Scene = "Caverna_2", string fileName = "savegame1")
	{

		Game g = GameHelper.GetDataManager ().currentGame;
		g.Save (Scene);
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/" + fileName + ".gd");
		bf.Serialize (file, g);
		file.Close ();
	//	CharManager.manager.character.SceneName = Application.loadedLevelName;
	//	SaveCharacter ("Char" + CharManager.manager.character.Slot.ToString ());
	}

	public static void Load(string fileName = "savegame1", bool firstLoad = false)
	{
		if (File.Exists(Application.persistentDataPath + "/" +fileName + ".gd"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + fileName + ".gd", FileMode.Open);
			Game g = (Game)bf.Deserialize(file);
			file.Close();
			g.firstLoad = firstLoad;
			g.Load();
			GameHelper.GetDataManager().currentGame = g;

		 
		}
	}

	public static bool SaveExists(string filename)
	{
		return (File.Exists (Application.persistentDataPath + "/" + filename + ".gd"));
	}

}

public interface ILoadingFinishedHandler
{
	void OnLoadingFinished();
}