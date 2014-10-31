using UnityEngine;
using UnityEditor; // Dont forget to add this as we are extending the Editor
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EntitySkills))] //Set tour script to extend the DoCake.cs
[System.Serializable]
public class SkillsComponent : Editor // Our script inherits from Editor
{
	EntitySkills _target;
	
	
	void OnEnable()
	{
		_target = (EntitySkills)target;
		
	}
	[SerializeField]
	int startingCount;
	[SerializeField]
	string[] startingSkills;// = new string[0]; // I DON'T LIKE 
	float[] ssvalues = new float[0]; // ALL OF THESE
	string[] skillTypeName = new string[0]; // SHITS OF ARRAYS. FIX IT YOU LAZY FUCKER.
	public override void OnInspectorGUI()
	{

		_target = (EntitySkills)target;
		startingCount = (_target.startingSkills != null) ? _target.startingSkills.Length : 0;
		if (_target.startingSkills == null)
			_target.startingSkills = new string[0];
		EditorGUILayout.LabelField ("Starting skills");
		startingCount = EditorGUILayout.IntField ("Skills:", startingCount);
		if (startingCount != _target.startingSkills.Length && startingCount != 0)
		{
			string[] it = _target.startingSkills.Clone() as string[];

			_target.startingSkills = new string[startingCount];
			for(int y = 0; y < it.Length; y++)
			{
				_target.startingSkills[y] = it[y];
			}
		}
		for(int i = 0; i < _target.startingSkills.Length; i++)
		{
			_target.startingSkills[i] = EditorGUILayout.TextField("Nome skill:", _target.startingSkills[i]);
		}
		//_target.startingSkills = startingSkills.Clone () as string[];
		//EditorUtility.SetDirty (_target);
		EditorGUILayout.LabelField ("Current skills");
		foreach(KeyValuePair<string, Skill> s in _target.Skills)
		{
			s.Value.Value = EditorGUILayout.Slider( s.Key, s.Value.Value, 0, 100 );
			s.Value.Value = System.Convert.ToSingle(_target.GetSkillValueText( s.Key ));
		}
	}
}