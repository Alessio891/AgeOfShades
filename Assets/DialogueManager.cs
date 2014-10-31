using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;

public class DialogueManager : MonoBehaviour {

	public Text AreaTesto;
	public GameObject AreaScelte;
	public GameObject ChoicePrefab;
	public List<GameObject> currentChoices = new List<GameObject> ();
	static DialogueManager _instance;
	public static DialogueManager instance { get { return _instance; } }
	public bool SomeonesTalking { get { return WhosTalking != null; } }
	public GameObject WhosTalking;
	string currentDialogueScript;
	void Awake()
	{
		_instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}

	public void ShowDialogue(string filename, string dialogueName, bool nopath)
	{

		string path = Application.dataPath;
		if (Application.platform == RuntimePlatform.OSXPlayer) {
			path += "/../../";
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			path += "/../";
		}
		try
		{
			StreamReader sr = new StreamReader (path + "/" + filename);
			string lines = sr.ReadToEnd ();
			sr.Close ();
			ShowDialogue (lines, dialogueName);
		}
		catch (Exception e)
		{
			GameHelper.SystemMessage("Errore: Il file " + filename + " non è stato trovato. (Forse manca l'estensione?)", Color.red);
		}

	}
	public void ShowDialogue(TextAsset dialogue, string dialogueName)
	{
		ShowDialogue (dialogue.text, dialogueName);
	}
	public void ShowDialogue(string dialogue, string dialogueName)
	{
		currentDialogueScript = dialogue;
		foreach (GameObject o in currentChoices)
			DestroyImmediate (o);
		currentChoices.Clear ();
		AreaTesto.text = string.Empty;
		foreach(string s in DialogueParser.GetDialogue(dialogue, dialogueName))
		{
			AreaTesto.text += s + "\n";
		}

		foreach(string[] ch in DialogueParser.GetChoices(dialogue, dialogueName))
		{
			GameObject choice = Instantiate(ChoicePrefab) as GameObject;
			choice.transform.parent = AreaScelte.transform;
			choice.transform.localPosition = Vector3.zero;
			choice.transform.localScale = Vector3.one;
			choice.GetComponent<Text>().text = ch[1];
			choice.GetComponent<DialogueChoice>().choiceId = ch[0];
			choice.GetComponent<DialogueChoice>().data = ch;
			currentChoices.Add(choice);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnChoice(DialogueChoice c)
	{
		string choiceId = c.choiceId;
		Debug.Log ("Scelta la numero " + c.choiceId);
		if (choiceId.Contains("_End_"))
		{
			foreach(GameObject o in currentChoices)
				Destroy(o);
			currentChoices.Clear();
			AreaTesto.text = string.Empty;
			GameHelper.HideMenu(gameObject);
			return;
		}
		else if (choiceId == "_Cutscene_")
		{
			foreach(GameObject o in currentChoices)
				Destroy(o);
			currentChoices.Clear();
			AreaTesto.text = string.Empty;
			Debug.Log(c.data[2]);
			GameHelper.HideMenu(gameObject);
			CutSceneManager.instance.SwitchToCutscene( () => { CutSceneManager.instance.PlayCutscene(c.data[2]); } );
			return;
		}
		ShowDialogue (currentDialogueScript, choiceId);
	}
}


public class DialogueData
{
	public string Text;
	public string[] choices;
}

public static class DialogueParser
{
	public static string[] GetDialogue(TextAsset dialogueScript, string dialogueName)
	{
		return GetDialogue (dialogueScript.text, dialogueName);
	}
	public static string[] GetDialogue(string dialogueScript, string dialogueName)
	{
		List<string> returnValue = new List<string> ();
		string[] lines = dialogueScript.Split (new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
		Debug.Log (dialogueName);
		int lineCount = 0;
		while (!lines[lineCount].ToLower().StartsWith("start dialogo " + dialogueName.ToLower()))
		{
			Debug.Log("Parsing: " + lines[lineCount]);
			
			lineCount++;
			if (lineCount >= lines.Length)
			{
				Debug.LogError("Dialogue Parser: E' stata raggiunta la fine del file senza trovare il dialogo.");
				return null;
			}
		}
		lineCount++;
		while(!lines[lineCount].ToLower().StartsWith(":end dialogo"))
		{
			Debug.Log("Parsing: " + lines[lineCount]);
			
			returnValue.Add(lines[lineCount]);
			lineCount++;
			if (lineCount >= lines.Length)
			{
				Debug.LogError("Dialogue Parser: Impossibile trovare la fine del dialogo " + dialogueName);
				return null;
			}
		}

		return returnValue.ToArray ();
	}

	public static List<string[]> GetChoices(TextAsset dialogueScript, string dialogueName)
	{
		return GetChoices (dialogueScript.text, dialogueName);
	}
	public static List<string[]> GetChoices(string dialogueScript, string dialogueName)
	{
		List<string> returnValue = new List<string> ();

		string[] lines = dialogueScript.Split (new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
		
		int lineCount = 0;
		while (!lines[lineCount].ToLower().StartsWith("start scelte " + dialogueName.ToLower()))
		{
			Debug.Log("Parsing: " + lines[lineCount]);
			
			lineCount++;
			if (lineCount >= lines.Length)
			{
				Debug.LogError("Dialogue Parser: E' stata raggiunta la fine del file senza trovare le scelte del dialogo.");
				return null;
			}
		}
		lineCount++;
		while(!lines[lineCount].ToLower().StartsWith(":end scelte"))
		{
			Debug.Log("Parsing: " + lines[lineCount]);
			
			returnValue.Add(lines[lineCount]);
			lineCount++;
			if (lineCount >= lines.Length)
			{
				Debug.LogError("Dialogue Parser: Impossibile trovare la fine delle scelte del dialogo " + dialogueName);
				return null;
			}
		}

		List<string[]> retList = new List<string[]> ();
		for(int i = 0; i < returnValue.Count; i++)
		{
			string[] splitted = returnValue[i].Split(new char[] { ':' }, System.StringSplitOptions.RemoveEmptyEntries);
			string k = splitted[0].Replace("[", "").Replace("]", "");
			splitted[0] = k;
			retList.Add(splitted);
		}

		return retList;
	}
}