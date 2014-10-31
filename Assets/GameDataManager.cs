using UnityEngine;
using System.Collections;

public class GameDataManager : MonoBehaviour {

	public Game currentGame;

	// Use this for initialization
	void Start () {

		DontDestroyOnLoad (gameObject);
	}

	public void LoadDataOnLevelLoad()
	{
		LoadGame = true;
	}

	public bool LoadGame = false;
	void OnLevelWasLoaded()
	{
		if (Application.loadedLevelName == "Prova")
		{

			if (LoadGame)
			{
				Debug.Log("Game Loaded");
				LoadGame = false;
				GameObject.Find ("GameManager").GetComponent<GameManager> ().Load ("Test");
				
			}
		}
	}

	IEnumerator _loadData()
	{
		while( Application.isLoadingLevel )
		{
			Debug.Log("Loading...");
			yield return new WaitForSeconds(0.1f);
		}



	}

	// Update is called once per frame
	void Update () {
	
	}
}
