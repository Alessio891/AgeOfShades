using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {
	public Character character;
	static CharacterManager _instance;

	public static bool isActive {
		get {
			return _instance != null;
		}
	}

	public static void Init()
	{
		_instance = Object.FindObjectOfType(typeof(CharacterManager)) as CharacterManager;
		if (_instance == null)
		{
			GameObject go = new GameObject("CharacterManager");
			DontDestroyOnLoad(go);
			_instance = go.AddComponent<CharacterManager>();
		}
	}

	static public CharacterManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType(typeof(CharacterManager)) as CharacterManager;
				if (_instance == null)
				{
					GameObject go = new GameObject("CharacterManager");
					DontDestroyOnLoad(go);
					_instance = go.AddComponent<CharacterManager>();
				}
			}
			return _instance;

		}
	}

	public void StartRoutine( IEnumerator routine )
	{
		StartCoroutine (routine);
	}
	
}

public static class CharManager
{
	static CharacterManager instance;

	public static CharacterManager manager {
		get {
			return CharacterManager.instance;
		}
	}
}