using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMeshManager : MonoBehaviour, ILoadingFinishedHandler {

	public BodySet BodyRef;
	public ActualBodyParts Equip = new ActualBodyParts();
	public ActualBodyParts Skins = new ActualBodyParts();

	public bool AttachedToPlayer { get { return GetComponent<PlayerEquip> () != null; } }
	public bool CreationMenu { get { return GameObject.FindObjectOfType (typeof(MainMenuManager)) != null; } }
	public Color SkinColor = new Color(0.937f, 0.816f, 0.812f, 1);

	// Use this for initialization
	void Start () {
		if (!CreationMenu)
		{
			if (CharManager.manager.character == null)
				SkinColor = new Color(0.937f, 0.816f, 0.812f, 1);

			//			SkinColor = CharManager.manager.character.SkinColor.Color;
			Skins.Arms = InstantiateAndAdd (BodyRef.Arms, SkinColor);
			Skins.Head = InstantiateAndAdd (BodyRef.Head, SkinColor);
			Skins.Feet = InstantiateAndAdd (BodyRef.Feet, SkinColor);
			Skins.Hands = InstantiateAndAdd (BodyRef.Hands, SkinColor);
			Skins.Legs = InstantiateAndAdd (BodyRef.Legs, SkinColor);
			Skins.Neck = InstantiateAndAdd (BodyRef.Neck, SkinColor);
			Skins.Torso = InstantiateAndAdd (BodyRef.Torso, SkinColor);
			if (CharManager.manager.character.HairPrefab != null)
			{ 
				Skins.Hair = InstantiateAndAdd( Resources.Load(CharManager.manager.character.HairPrefab) as GameObject, CharManager.manager.character.HairColor.Color );
			}
			//Skins.Hair = InstantiateAndAdd (BodyRef.Hair, Color.blue);
			// Generate Basic Mesh
		}
	}

	void OnDeserialized()
	{
		RemoveAll ();
		AssembleBodyRefModel ();
	}

	public void OnLoadingFinished()
	{
		//Debug.Log ("Changed color to " + CharManager.manager.character.SkinColor.Color);
		//ChangeSkinColor (CharManager.manager.character.SkinColor.Color);
	
	}

	public void AssembleBodyRefModel()
	{
		Skins.Arms = InstantiateAndAdd (BodyRef.Arms, SkinColor);
		Skins.Head = InstantiateAndAdd (BodyRef.Head, SkinColor);
		Skins.Feet = InstantiateAndAdd (BodyRef.Feet, SkinColor);
		Skins.Hands = InstantiateAndAdd (BodyRef.Hands, SkinColor);
		Skins.Legs = InstantiateAndAdd (BodyRef.Legs, SkinColor);
		Skins.Neck = InstantiateAndAdd (BodyRef.Neck, SkinColor);
		Skins.Torso = InstantiateAndAdd (BodyRef.Torso, SkinColor);

	}

	public void RemoveAll()
	{
		Remove (ref Skins.Arms);
		Remove (ref Skins.Head);
		Remove (ref Skins.Feet);
		Remove (ref Skins.Hands);
		Remove (ref Skins.Legs);
		Remove (ref Skins.Neck);
		Remove (ref Skins.Torso);
	}

	public void ChangeColor(ref List<GameObject> part, Color c)
	{
		if (part == null)
			return;
		if (part.Count <= 0)
			return;	
		foreach(GameObject o in part)
		{
			SkinnedMeshRenderer meshRenderer = o.GetComponent<SkinnedMeshRenderer>();
			if (meshRenderer != null)
			{
				if (meshRenderer.materials.Length > 1)
				{
					foreach(Material m in meshRenderer.materials)
					{
						m.color = c;
					}
					meshRenderer.materials[0].color = SkinColor;
				}
				else
				{
					meshRenderer.material.color = c;
				}
			}
		}
	}

	public Color GetPieceColor(ref List<GameObject> piece)
	{
		if (piece == null || piece.Count <= 0)
			return Color.white;
		return (piece [0].GetComponent<SkinnedMeshRenderer> () != null) ? piece [0].GetComponent<SkinnedMeshRenderer> ().material.color : Color.white;
	}
	List<Color> oldColors = new List<Color>();
	public void ChangeAllColors(Color c, bool rememberOldColor = true)
	{
		oldColors.Clear ();
		oldColors.Add (GetPieceColor (ref Skins.Head));
		oldColors.Add (GetPieceColor (ref Skins.Arms));
		oldColors.Add (GetPieceColor (ref Skins.Neck));
		oldColors.Add (GetPieceColor (ref Skins.Torso));
		oldColors.Add (GetPieceColor (ref Skins.Legs));
		oldColors.Add (GetPieceColor (ref Skins.Feet));
		oldColors.Add (GetPieceColor (ref Skins.Hands));

		ChangeColor (ref Skins.Head, c);
		ChangeColor (ref Skins.Arms, c);
		ChangeColor (ref Skins.Neck, c);
		ChangeColor(ref Skins.Torso, c);
		ChangeColor(ref Skins.Legs, c);
		ChangeColor (ref Skins.Feet, c);
		ChangeColor (ref Skins.Hands, c);
	}

	public void RestoreOldColors()
	{
		ChangeColor (ref Skins.Head, oldColors[0]);
		ChangeColor (ref Skins.Arms, oldColors[1]);
		ChangeColor (ref Skins.Neck, oldColors[2]);
		ChangeColor(ref Skins.Torso, oldColors[3]);
		ChangeColor(ref Skins.Legs, oldColors[4]);
		ChangeColor (ref Skins.Feet, oldColors[5]);
		ChangeColor (ref Skins.Hands, oldColors[6]);
	}

	public void ChangeSkinColor(Color c)
	{
		PlayerEquip e = GameHelper.GetPlayerComponent<PlayerEquip> () as PlayerEquip;
		SkinColor = c;
		ChangeColor (ref Skins.Head, c);
		ChangeColor (ref Skins.Arms, c);
		ChangeColor (ref Skins.Neck, c);
		if (e != null)
		{
			if (e.Armor.Shirt == null)
			{
				ChangeColor(ref Skins.Torso, c);
			}
			if (e.Armor.Pants == null)
			{
				ChangeColor(ref Skins.Legs, c);
			}
			if (e.Armor.Feet == null)
				ChangeColor (ref Skins.Feet, c);
			
		}
		ChangeColor (ref Skins.Hands, c);
	}

	public void Remove(ref List<GameObject> part)
	{
		if (part == null)
		{
			Debug.LogError("La parte che si vuole rimuovere non esiste.");
			return;
		}
		foreach(GameObject o in part)
		{
			Destroy(o);
		}
		part = null;
	}

	void PutOn(GameObject obj, ref List<GameObject> part, Color c)
	{
		part = InstantiateAndAdd (obj, c);
	}

	void Shirt()
	{
//		Destroy (ActualBody.Head);
//		ActualBody.Head = InstantiateAndAdd (Set1.Head);
	}

	List<GameObject> InstantiateAndAdd(GameObject prefab, Color c)
	{
		GameObject o = Instantiate (prefab) as GameObject;
		List<GameObject> i = SkinnedMeshTools.AddSkinnedMeshTo (o, transform, true);
		foreach(GameObject ob in i)
		{
			if (!AttachedToPlayer)
			{
				ob.layer = LayerMask.NameToLayer("UI");
			}

			ob.GetComponent<SkinnedMeshRenderer>().material.color = c;
		}
		Destroy (o);
		return i;
	}

	public void Swap(GameObject obj, ref List<GameObject> part, Color c)
	{
		Remove (ref part);
		PutOn (obj, ref part, c);
	}

	// Update is called once per frame
	bool shirt = false;
	void Update () {

	}
}


[System.Serializable]
public class BodySet
{
	public GameObject Head;
	public GameObject Neck;
	public GameObject Torso;
	public GameObject Arms;
	public GameObject Legs;
	public GameObject Hands;
	public GameObject Feet;
	public GameObject Hair;
	public GameObject Eyes;
	public GameObject Eyebrows;
}

[System.Serializable]
public class ActualBodyParts
{
	public List<GameObject> Head;
	public List<GameObject> Neck;
	public List<GameObject> Torso;
	public List<GameObject> Arms;
	public List<GameObject> Legs;
	public List<GameObject> Hands;
	public List<GameObject> Feet;
	public List<GameObject> Hair;
	public List<GameObject> Eyes;
	public List<GameObject> Eyebrows;

	public void FromClone(ActualBodyParts cloned, PlayerMeshManager manager)
	{

	}
}