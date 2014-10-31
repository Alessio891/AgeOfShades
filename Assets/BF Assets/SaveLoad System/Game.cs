using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interface for any object that will be serialized. 
/// </summary>
public interface ISerializedObject
{
	object[] Serialize();
	void DeSerialize(object[] data);
	string GetUID();
	void OnLoadingFinished();
}

/// <summary>
/// Store a Vector3 to be serialized
/// </summary>
[System.Serializable]
public class PlayerPosition {
	public float x;
	public float y;
	public float z;
		
	public PlayerPosition(Vector3 pos)
	{
		x = pos.x;
		y = pos.y;
		z = pos.z;
	}

	public Vector3 GetVector()
	{
		return new Vector3(x, y, z);
	}
}

/// <summary>
/// Main Data Structure for the saved game.
/// </summary>
[System.Serializable]
public class Game
{
	public string SceneName;
	public string GameName;
	public Dictionary<string, object[]> Data;
	public bool firstLoad;

	public Game(string name)
	{
		GameName = name;
	}

	public void StartNewGame()
	{
		Data = new Dictionary<string, object[]> ();
		SceneName = Application.loadedLevelName;
	}

	public void Save(string Scene)
	{
		SceneName = Scene;
		//CharManager.manager.character.SceneName = SceneName;
		foreach(MonoBehaviour obj in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
		{
			//Debug.Log("Checking... " + obj.GetType());
		   if (obj is ISerializedObject)
			{
				//Debug.Log("Serializing: " + obj.GetType().ToString());
		   		Data[ (obj as ISerializedObject).GetUID() ] = (obj as ISerializedObject).Serialize();
				/*int i = 0;
				foreach(object o in Data[ (obj as ISerializedObject).GetUID() ])
				{
					if (o == null)
					{
						Debug.LogError("ALERT!ALERT! NULL SERIALIZATION! Serializer: " + obj.GetType() + " : :  Serialized index: " + i);
					}
					Debug.Log("-- " + o.GetType() + " serialized.");
					i++;
				}
				Debug.Log("End Serializing: " + obj.GetType().ToString());*/
			}
		}
	}

	public void Load()
	{
		GameObject o = new GameObject ();
		LoadCoroutine r = o.AddComponent<LoadCoroutine> ();
		r.SceneName = SceneName;
		r.StartLoading();
	}
}
