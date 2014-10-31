using UnityEngine;
using UnityEditor; // Dont forget to add this as we are extending the Editor
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PlayerInventory))] //Set tour script to extend the DoCake.cs
public class InventoryComponent : Editor // Our script inherits from Editor
{
	PlayerInventory _target;

	string[] startItems = new string[0];
	int size = 0;

	void OnEnable()
	{
		_target = (PlayerInventory)target;

	}

	public override void OnInspectorGUI()
	{
		_target = (PlayerInventory)target;

		_target.MaxItems = EditorGUILayout.IntSlider ( "Oggetti massimi:", _target.MaxItems, 1, 100);

		_target._size = EditorGUILayout.IntField ("Numero:", _target._size);

		if (_target._size != _target.startItems.Length)
		{
			_target.startItems = new string[_target._size];
		}

		for(int i = 0; i < _target._size; i++)
		{
			_target.startItems[i] = EditorGUILayout.TextField(i.ToString(), _target.startItems[i]);
		}

	}
}