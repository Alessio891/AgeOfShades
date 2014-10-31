using UnityEditor;
using Unity;
using UnityEngine;
using System.Collections.Generic;
using System;

public class DesigningTool : EditorWindow {

	static System.Type[] basicItems;
	

	[MenuItem ("BF Tools/Design Tool")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		DesigningTool window = (DesigningTool)EditorWindow.GetWindow (typeof (DesigningTool));
		basicItems = ItemDatabase.GetAllSubTypes (typeof(BasicItem));
	}

	string state = "Items";

	void OnGUI()
	{
		GUI.BeginGroup (new Rect (5, 5, 600, 100));
		if ( GUI.Button (new Rect (0, 0, 100, 20), "Items") )
			state = "Items";
		if (GUI.Button (new Rect (100, 0, 100, 20), "NPCs"))
			state = "NPCs";
		if (GUI.Button (new Rect (200, 0, 100, 20), "Spawner"))
			state = "Spawner";
		if (GUI.Button (new Rect (300, 0, 100, 20), "Crafting"))
			state = "Crafting";
		if (GUI.Button (new Rect (400, 0, 100, 20), "Misc."))
			state = "Misc.";
		if (GUI.Button (new Rect(500, 0, 100, 20), "LevelBuilding"))
			state = "LevelBuilding";
		GUI.EndGroup ();

		switch(state)
		{
		case "Items":
			Item ();
			break;
		case "NPCs":
			EditorGUILayout.LabelField("NPCs");
			break;
		case "Spawner":
			EditorGUILayout.LabelField("Spawner");
			break;
		case "Crafting":
			EditorGUILayout.LabelField("Crafting");
			break;
		case "Misc.":
			EditorGUILayout.LabelField("Misc.");
			break;
		case "LevelBuilding":
			LevelBuilding();
			break;
		}

	}

	GameObject referenceGameObject;
	AxisPopup axis;
	bool NegativeAxis;
	int repeatFor = 3;
	GameObject[] items = new GameObject[0];
	int itemsCount = 0;
	float spacing = 0;
	bool showAdvanced = false;

	bool orientAsReference = false;
	bool useSelectedAsReference = false;
	bool randomOrientation = false;
	bool selectNewOnCreation = false;
	bool lastGetModified = true;
	bool ignoreBounds = false;
	List<GameObject> LastInserted;
	GameObject lastReference;
	Vector3 lastOrigin;
	void LevelBuilding()
	{
		/* Place X tiles in a row on the specified axis, randomizing between a list of tiles.
		 * Needs a reference GameObject for the axis */
		GUI.BeginGroup (new Rect (0, 30, 600, 500));
		if (!useSelectedAsReference)
			referenceGameObject = EditorGUILayout.ObjectField ( "Riferimento:", referenceGameObject, typeof(GameObject), true) as GameObject;
		else
		{
			referenceGameObject = Selection.activeGameObject;
			if (referenceGameObject != null)
			{
				EditorGUILayout.LabelField("Selezionato: " + referenceGameObject.name);
			}
			else
				EditorGUILayout.LabelField("Nessun oggetto selezionato");
		}

		EditorGUILayout.BeginHorizontal ();
		axis = (AxisPopup)EditorGUILayout.EnumPopup ("Asse:", axis);
		NegativeAxis = EditorGUILayout.Toggle ("Negativo:", NegativeAxis);
		EditorGUILayout.EndHorizontal ();
		repeatFor = EditorGUILayout.IntField ("Ripeti per:", repeatFor);
		float lastSpacing = spacing;
		spacing = EditorGUILayout.FloatField ("Offset Margine:", spacing);
		itemsCount = EditorGUILayout.IntField ("Oggetti:", itemsCount);
		if (lastSpacing != spacing)
		{
			foreach(GameObject o in LastInserted)
				DestroyImmediate(o);
			LastInserted.Clear();
			referenceGameObject = lastReference;
			PlaceObjects();
		}
		if (itemsCount != items.Length)
		{
			if (itemsCount > items.Length)
			{
				GameObject[] it = items.Clone() as GameObject[];
				items = new GameObject[itemsCount];
				for(int y = 0; y < it.Length; y++)
					items[y] = it[y];
			}
			else if (itemsCount < items.Length)
			{
				GameObject[] it = items.Clone() as GameObject[];
				items = new GameObject[itemsCount];
				for(int y = 0; y < items.Length; y++)
					items[y] = it[y];
			}
		}
		for(int i = 0; i < itemsCount; i++)
		{
			items[i] = EditorGUILayout.ObjectField("Oggetto " + i.ToString(), items[i], typeof(GameObject), true) as GameObject;
		}
		if (GUILayout.Button("Piazza"))
		{
			PlaceObjects();
		}

		if (GUILayout.Button("Avanzate"))
		{
			showAdvanced = !showAdvanced;
		}
		if (showAdvanced)
		{

			orientAsReference = GUILayout.Toggle(orientAsReference,"Orienta come l'originale");
			useSelectedAsReference = GUILayout.Toggle(useSelectedAsReference, "Usa l'oggetto selezionato come riferimento");
			randomOrientation = GUILayout.Toggle(randomOrientation, "Orienta in modo casuale");
			selectNewOnCreation = GUILayout.Toggle(selectNewOnCreation, "Seleziona gli oggetti dopo averli creati");
			lastGetModified = GUILayout.Toggle(lastGetModified, "Modifica gli ultimi inseriti in tempo reale");
			ignoreBounds = GUILayout.Toggle(ignoreBounds, "Ignora grandezza oggetto");
		}
		if (GUILayout.Button("Comandi utili"))
		{
			showUtility = !showUtility;
		}
		if (showUtility)
		{
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Elimina ultimi"))
			{
				foreach(GameObject o in LastInserted)
				{
					DestroyImmediate(o);
				}
				LastInserted.Clear();
			}
			if (GUILayout.Button("Applica modifiche agli ultimi") )
			{}
			EditorGUILayout.EndHorizontal();
		}
		GUI.EndGroup ();
	}
	bool showUtility = false;

	void PlaceObjects()
	{
		if (referenceGameObject == null)
		{
			if (lastReference == null)
				Debug.LogError("LEVEL BUILDER: Oggetto riferimento non impostato.");
			else
				referenceGameObject = lastReference;
		}
		else if (itemsCount <= 0)
		{
			Debug.LogError("LEVEL BUILDER: Nessun oggetto da piazzare.");
		}
		else
		{
			Vector3 ax = Vector3.zero;
			lastReference = referenceGameObject;	
			switch(axis)
			{
			case AxisPopup.X:
				ax = referenceGameObject.transform.right;
				break;
			case AxisPopup.Y:
				ax = referenceGameObject.transform.up;
				break;
			case AxisPopup.Z:
				ax = referenceGameObject.transform.forward;
				break;
			}
			if (NegativeAxis)
				ax *= -1;

			List<GameObject> created = new List<GameObject>();
			for(int i = 0; i < repeatFor; i++)
			{
				
				GameObject newobj = GameObject.Instantiate( items[ UnityEngine.Random.Range(0, items.Length) ] ) as GameObject;
				Vector3 size = newobj.renderer.bounds.size;
				float dist = 0;
				switch(axis)
				{
				case AxisPopup.X:
					dist = size.x;
					break;
				case AxisPopup.Y:
					dist = size.y;
					break;
				case AxisPopup.Z:
					dist = size.z;
					break;
				}
				
				if (orientAsReference)
				{
					newobj.transform.rotation = referenceGameObject.transform.rotation;
				}
				else if (randomOrientation)
				{
					newobj.transform.rotation = new Quaternion( UnityEngine.Random.Range(-120, 120), UnityEngine.Random.Range(-120, 120), UnityEngine.Random.Range(-120, 120), UnityEngine.Random.Range(-120, 120));
				}

				if (!ignoreBounds)
					newobj.transform.position = referenceGameObject.transform.position + (ax * (i+1) * (dist + spacing));
				else
					newobj.transform.position = referenceGameObject.transform.position + (ax * (i+1) *  spacing);
				
				created.Add(newobj);
			}
			if (selectNewOnCreation)
			{
				Selection.objects = created.ToArray();
			}
			LastInserted = new List<GameObject>(created);
		}
	}

	string itemState = "null";
	void Item()
	{
		GUI.BeginGroup (new Rect (10, 30, 200, 200));
		foreach(System.Type t in basicItems)
		{
			string path = t.GetAttributeValue( (BFCategory cat) => cat.path );

			EditorGUILayout.LabelField(path);
		}
		GUI.EndGroup ();
		ShowCreateItem ();
	}

	string itemName = "";
	string itemDescription = "";
	string itemIcon = "";
	bool canStack = true;
	int maxStack = 0;
	GameObject prefab;
	void ShowCreateItem()
	{

	}
	void CreateItem()
	{
	}
}

enum AxisPopup
{
	X = 0,
	Y,
	Z
}