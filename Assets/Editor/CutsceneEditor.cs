using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;

public class CutsceneEditor : EditorWindow {

	[MenuItem ("BF Tools/Cutscene Editor")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		CutsceneEditor window = (CutsceneEditor)EditorWindow.GetWindow (typeof (CutsceneEditor));

	}

	int currentMenu = 0;

	void OnGUI()
	{

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button("Crea Nuova Cutscene"))
		{
			currentMenu = 1;
		}
		if (GUILayout.Button("Modifica Cutscene"))
		{
			currentMenu = 2;
		}
		GUILayout.EndHorizontal ();

		if (currentMenu == 1)
		{
			NewCutscene();
		}
		else if (currentMenu == 2)
		{

		}
	}

	int toolBarSelection = 0;
	string CutsceneName = "";
	void NewCutscene()
	{
		CutsceneName = EditorGUILayout.TextField ("Nome Cutscene:", CutsceneName);
		GUILayout.Space (10);
		toolBarSelection = GUILayout.Toolbar (toolBarSelection, new string[] { "Azioni", "Attori", "Avanzate" });		

		switch(toolBarSelection)
		{
		case 0:
			ShowActions();
			break;
		case 1:
			break;
		case 2:
			break;
		}
	}

	List<string> actionList = new List<string>();
	List<string> actionInserted = new List<string>();
	int selectedAction = 0;
	Dictionary<string, object> ActionParametersInfo = new Dictionary<string, object>();
	Dictionary<string, Dictionary<string, object>> parametersInseted = new Dictionary<string, Dictionary<string, object>>();

	void UpdateActionList()
	{
		actionList.Clear();
		MethodInfo[] info = typeof(CutSceneManager).GetMethods ();
		foreach(MethodInfo i in info)
		{
			object[] attributes = i.GetCustomAttributes(true);
			foreach(object o in attributes)
			{
				if (o.ToString() == "CutsceneAction")
				{
					Debug.Log(i.Name);
					actionList.Add(i.Name);
				}
			}
		}
	}

	void ShowActions()
	{
		GUILayout.Space (20);
		if (actionList.Count <= 0)
			UpdateActionList();
		if (GUILayout.Button("Test"))
		{
			UpdateActionList();
		}
		int lastSelected = selectedAction;
		selectedAction = EditorGUILayout.Popup ("Azione:",selectedAction, actionList.ToArray ());
		ParameterInfo[] pInfo = typeof(CutSceneManager).GetMethod (actionList [selectedAction]).GetParameters ();
		if (lastSelected != selectedAction)
		{
			ActionParametersInfo.Clear();
		}
		foreach(ParameterInfo i in pInfo)
		{

			string t = i.ParameterType.ToString();
			if (t == "UnityEngine.Vector3")
			{
				if (!ActionParametersInfo.ContainsKey(i.Name))
				{
					ActionParametersInfo.Add(i.Name, Vector3.zero);
				}
				ActionParametersInfo[i.Name] = EditorGUILayout.Vector3Field(i.Name, (Vector3)ActionParametersInfo[i.Name]);
			}
			else if (t == "System.Single")
			{
				if (!ActionParametersInfo.ContainsKey(i.Name))
				{
					ActionParametersInfo.Add(i.Name, 0.0f);
				}
				ActionParametersInfo[i.Name] = EditorGUILayout.FloatField(i.Name, (float)ActionParametersInfo[i.Name]);
			}
			else if (t == "UnityEngine.Vector3[]")
			{
				if (!ActionParametersInfo.ContainsKey(i.Name))
				{
					ActionParametersInfo.Add(i.Name, new Vector3[2]);
				}
				EditorGUILayout.HelpBox("Raggruppare in un unico parent tutti i transform da registrare, chiamandoli in fila 'Path_xyz' dove xyz indica l'ordine da seguire", MessageType.Info);			
				ActionParametersInfo[i.Name] = EditorGUILayout.ObjectField("Parent di " + i.Name + ":",ActionParametersInfo[i.Name] as GameObject, typeof(GameObject), true);
			}
			else if (t == "System.Boolean")
			{
				if (!ActionParametersInfo.ContainsKey(i.Name))
				{
					ActionParametersInfo.Add(i.Name, false);
				}
				ActionParametersInfo[i.Name] = EditorGUILayout.Toggle(i.Name, (bool)ActionParametersInfo[i.Name]);
			}
			else if (t == "UnityEngine.GameObject")
			{
				if (!ActionParametersInfo.ContainsKey(i.Name))
				{
					ActionParametersInfo.Add(i.Name, null);
				}
				ActionParametersInfo[i.Name] = EditorGUILayout.ObjectField(i.Name,(GameObject)ActionParametersInfo[i.Name], typeof(GameObject), true);
			}
		}

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button("Aggiungi Azione"))
		{
			string dictKey = actionList[selectedAction] + "_0";
			if (parametersInseted.ContainsKey(dictKey))
			{
				int id = 1;
				dictKey = actionList[selectedAction] + "_1";
				while(parametersInseted.ContainsKey(dictKey))
				{
					id++;
					dictKey = actionList[selectedAction] + "_" + id.ToString();
				}
			}
			parametersInseted.Add(dictKey, new Dictionary<string, object>(ActionParametersInfo));
			actionInserted.Add(dictKey);
			
		}
		if (GUILayout.Button("Elimina tutte le azioni"))
		{
			actionInserted.Clear();
			parametersInseted.Clear();
		}
		GUILayout.EndHorizontal ();

		int actionIndex = 0;
		foreach(string action in actionInserted)
		{

			if (!parametersInseted.ContainsKey(action))
				continue;
			string p = "";
			foreach(KeyValuePair<string, object> pair in parametersInseted[action])
			{
				p += pair.Value.ToString() + ", ";
			}
			p += ")";
			p = p.Replace(", )", ")");
			GUILayout.Label("#"+actionIndex.ToString()+": " + action.Remove(action.Length - 2) + "("+p);

			actionIndex++;
		}

		if (GUILayout.Button ("Genera Script e Copia negli Appunti"))
		{
			foreach(string s in actionInserted)
			{
				GenerateScript(s);
			}
		}

	}

	void GenerateScript(string action)
	{
		string strippedAction = action.Remove (action.Length - 2);
		ParameterInfo[] pInfo = typeof(CutSceneManager).GetMethod (strippedAction).GetParameters ();
		string parametersString = "{ ";
		Dictionary<string, object> paramValues = parametersInseted [action];
		foreach(ParameterInfo info in pInfo)
		{
			if (paramValues.ContainsKey(info.Name))
			{
				string t = info.ParameterType.ToString();
				if (t == "UnityEngine.Vector3")
				{
					parametersString += "new Vector3" + paramValues[info.Name].ToString() + ", ";
				}
				else if (t == "System.Single")
				{
					parametersString += paramValues[info.Name].ToString() + "f, ";
				}
				else if (t == "System.Boolean")
				{
					parametersString += paramValues[info.Name].ToString().ToLower() + ", ";
				}
				else if (t == "UnityEngine.GameObject")
				{
					parametersString += "GameObject.Find(\"" + (paramValues[info.Name] as GameObject).name + "\"), ";
				}
				else
				{
					parametersString += paramValues[info.Name].ToString() + ", ";
				}
			}
		}
		parametersString += "}";
		parametersString = parametersString.Replace(", }", "}");
		Debug.Log("ActionList.Add(\"" + strippedAction + "\", new List<object> () " + parametersString + ";");
	}
}

public struct ActionParameterInfo
{
	object[] Value;
	Type ParameterType;
}
