using UnityEngine;
using UnityEditor; // Dont forget to add this as we are extending the Editor
using System.Collections;

//[CustomEditor(typeof(PlayerEquip))] //Set tour script to extend the DoCake.cs
public class EquipComponent : Editor // Our script inherits from Editor
{
	PlayerEquip _target;
	GUISkin s;
	GUIStyle normalLabel;
	GUIStyle greenLabel;
	GUIStyle redLabel;
	GUIStyle blueLabel;
	void OnEnable()
	{
		_target = (PlayerEquip)target;
		s = GUISkin.Instantiate (Resources.Load ("ComponentSkin")) as GUISkin;
		normalLabel = new GUIStyle (s.label);
		normalLabel.normal.textColor = new Color (0, 0, 0);
		normalLabel.fontStyle = FontStyle.Normal;
		normalLabel.fontSize = 10;
		s.label.normal.textColor = new Color (0.5f, 0.2f, 0.1f);
		s.label.fontStyle = FontStyle.BoldAndItalic;
		s.label.fontSize = 14;

		greenLabel = new GUIStyle (normalLabel);
		greenLabel.normal.textColor = new Color (0.1f, 0.7f, 0.2f);
		greenLabel.alignment = TextAnchor.MiddleLeft;

		redLabel = new GUIStyle (normalLabel);
		redLabel.normal.textColor = new Color (0.8f, 0.1f, 0.2f);
		redLabel.alignment = TextAnchor.MiddleLeft;

		blueLabel = new GUIStyle (normalLabel);
		blueLabel.normal.textColor = new Color (0.2f, 0.1f, 0.7f);
		blueLabel.alignment = TextAnchor.MiddleLeft;
	}

	public override void OnInspectorGUI()
	{
		_target = (PlayerEquip)target;
		EditorGUILayout.LabelField ("Mano destra", s.label);
		ShowHand (_target.RightHand);

		EditorGUILayout.LabelField ("Mano sinistra", s.label);
		ShowHand (_target.LeftHand);
	}

	void ShowHand(InventoryItem hand)
	{
		EditorGUILayout.BeginVertical ();
		

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Oggetto: ");
		EditorGUILayout.LabelField( ( (hand != null) ? hand.ItemName : "Vuota" ), (hand != null) ? greenLabel : redLabel);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.LabelField (GetToolString(hand), (IsTool(hand)) ? blueLabel : redLabel);
		if (hand != null && !IsTool(hand))
		{
			EditorGUILayout.HelpBox ( "Un oggetto viene considerato un attrezzo se ha un tag differente da quello di default.", MessageType.Info );
		}
		
		if (hand != null)
		{
			if (GUILayout.Button( "Rimuovi" ) )
			{
				_target.UnEquip(0);
			}
		}
		if (hand != null)
		{
			hand.AttackDamage = EditorGUILayout.FloatField ("Danno:", hand.AttackDamage);
			hand.AttackDelay = EditorGUILayout.FloatField ("Velocità:", hand.AttackDelay);
			hand.AttackRange = EditorGUILayout.FloatField ("Raggio d'azione:", hand.AttackRange);

			EditorGUILayout.LabelField ("Il nome dello script è " + hand.GetType ().ToString ());
		}
		EditorGUILayout.EndVertical ();
	}

	bool IsTool(InventoryItem item)
	{
		return (item != null && item.Prefab.tag != "Untagged");
	}

	string GetToolString(InventoryItem item)
	{
		string isTool = "Questo oggetto ";
		if (item != null)
		{
			isTool += ( (item.Prefab.tag != "Untagged") ? "è un attrezzo" : "non è un attrezzo");
		}
		else
			isTool = "- Nessun Oggetto in questa mano -";
		
		return isTool;
	}
}