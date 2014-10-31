using UnityEngine;
using System.Collections;
using UnityEditor;

public class UIHelper : EditorWindow {

	static GlobalUIManager manager { get { return GameObject.FindObjectOfType (typeof(GlobalUIManager)) as GlobalUIManager; } }

	[MenuItem ("BF Tools/UI Helper")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		UIHelper window = (UIHelper)EditorWindow.GetWindow (typeof (UIHelper));
		
		
	}

	void OnGUI()
	{
		if (GUILayout.Button("Nascondi selezionato"))
		{
			GameHelper.HideMenu(Selection.activeGameObject);
		}
		if (GUILayout.Button("Mostra selezionato"))
		{
			GameHelper.ShowMenu(Selection.activeGameObject);
		}
		if (GUILayout.Button("Nascondi tutti tranne selezionato"))
		{
			manager.HideA();
			GameHelper.ShowMenu(Selection.activeGameObject);

		}
		if (GUILayout.Button("Mostra tutti tranne selezionato"))
		{
			manager.ShowA();
			GameHelper.HideMenu(Selection.activeGameObject);
		}
		if (GUILayout.Button("Nascondi tutti"))
		{
			manager.HideA();
		}
		if (GUILayout.Button("Mostra tutti"))
		{
			manager.ShowA();
		}
	}

}
