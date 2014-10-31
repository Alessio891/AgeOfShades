using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect( Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 20), "Nuova partita"))
		{
			Game newGame = new Game("Test");
			GameHelper.GetDataManager().currentGame = newGame;
			newGame.StartNewGame();
			Application.LoadLevel("Prova");
		}
		if (GUI.Button(new Rect( Screen.width / 2 - 50, Screen.height / 2 - 20, 100, 20), "Carica partita"))
		{
			Application.LoadLevel("Prova");
			GameHelper.GetDataManager().LoadDataOnLevelLoad();
		}
		if (GUI.Button(new Rect( Screen.width / 2 - 50, Screen.height / 2 + 10, 100, 20), "Esci"))
		{
			Application.Quit();
		}



	}
}
