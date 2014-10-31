using UnityEngine;
using UnityEditor; // Dont forget to add this as we are extending the Editor
using System.Collections;

[CustomEditor(typeof(EntityStatus))] //Set tour script to extend the DoCake.cs
public class StatusComponent : Editor // Our script inherits from Editor
{
	EntityStatus _target;
	GUISkin s;
	
	void OnEnable()
	{
		_target = (EntityStatus)target;
		s = GUISkin.Instantiate (Resources.Load ("ComponentSkin")) as GUISkin;
		
	}
	
	public override void OnInspectorGUI()
	{
		_target = (EntityStatus)target;


		s.label.normal.textColor = new Color (0.5f, 0.2f, 0.1f);
		s.label.fontStyle = FontStyle.BoldAndItalic;
		s.label.fontSize = 14;
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Status", s.label);
		_target.Life = EditorGUILayout.Slider ("Vita:", _target.Life, 0, _target.MaxLife);
		_target.Hunger = EditorGUILayout.Slider ("Fame:", _target.Hunger, 0, 1);
		_target.Stamina = EditorGUILayout.Slider ("Stamina:", _target.Stamina, 0, _target.Dexterity);
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Statistiche", s.label);
		_target.Strength = EditorGUILayout.FloatField ("Forza:", _target.Strength);
		_target.Dexterity = EditorGUILayout.FloatField ("Destrezza:", _target.Dexterity);
		_target.Intellect = EditorGUILayout.FloatField ("Intelletto:", _target.Intellect);
		EditorGUILayout.EndVertical ();




		
	}
}