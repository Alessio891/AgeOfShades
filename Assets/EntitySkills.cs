using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntitySkills : MonoBehaviour, ISerializedObject {

	public Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();
	public string uid;
	public float SkillCap;
	public float SkillsTotal {
		get {
			float v = 0;
			foreach(KeyValuePair<string, Skill> p in Skills)
			{
				v += p.Value.Value;
			}
			return v;
		}
	}
	bool AttachedToPlayer { get { return (GetComponent<PlayerManager> () != null); } }
	public string[] startingSkills;
	// Use this for initialization
	void Awake () {
			uid = GameHelper.GetUIDForObject (this);
		
		if (AttachedToPlayer)
		{
			SkillCap = 600.0f;
			Skills = new Dictionary<string, Skill> ();
			Skills ["Corpo A Corpo"] = new CorpoACorpo ();
			Skills ["Cultura"] = new Cultura ();
			Dictionary<Skill, float> sn = new Dictionary<Skill, float> ();
			sn.Add (Skills ["Cultura"], 25);
			Skills ["Cultura: Anatomia"] = new Skill ("Cultura: Anatomia", 0, 1, sn);
			Skills ["Magia"] = new Magia ();
			Skills ["Nascondersi"] = new Nascondersi ();
			if (CharManager.manager.character.JustCreated)
			{
				startingSkills = CharManager.manager.character.startingSkills;
				Debug.Log("Skill iniziali: " + startingSkills.Length);
				
				for(int i = 0; i < 3; i++)
				{
					Skills[ startingSkills[i] ].Value += 10.0f;
					foreach(System.Type item in Skills [startingSkills[i]].BonusItems)
					{
						(GameHelper.GetPlayerComponent<PlayerInventory>() as PlayerInventory).PickupObject( (InventoryItem)System.Activator.CreateInstance(item), 1, false);
					}
				}
			}
		}
		else
		{
			foreach(string s in startingSkills)
			{
				Debug.Log("Hey! Im not the player and I have a new skill " + s.Replace(" ", ""));
				Skills[s] = (Skill)System.Activator.CreateInstance(System.Type.GetType(s.Replace(" ", "")));
				Skills[s].Value = 15;
				Debug.Log(Skills[s].Value);
			}
		}

	}

	public object[] Serialize() {
		object[] data = new object[1];
		data [0] = Skills;
		return data;
	}
	public void DeSerialize(object[] data) {
		Debug.Log (data [0].GetType ());
		if (data[0] != null)
			Skills = (Dictionary<string, Skill>)data [0];
	}
	public string GetUID() { return uid; }
	public void OnLoadingFinished() {

	}

	public bool SkillCheckSuccessful( string skill, float check, float chance ) 
	{ 
		// Più è alta dS piu probabilità di successo
		// p = 1 - 1/dS
		float p = (Skills [skill].Value / check) * 0.5f;
		if (p > 1)
		{
			p = 1;
			p -= Random.Range(0.0f, 0.1f); // Almost never 100% chance
		}
		float random = (float) (Random.Range (0, 1000) / 1000.0f);
		bool result = random < p;
		Debug.Log ("Checking " + check + " against " + Skills [skill].Value + ". RNG is " + random + ", chance are " + p +". The check is " + ((result) ? "passed." : "failed."));
		return result;
	}
	public float SkillIncreaseSuccessful (string skill, float difficulty, float minSkill) { return Skills[skill].Increase(difficulty, minSkill); }

	public string GetSkillValueText(string skill)
	{
		if (Skills[skill].Value % 1 == 0)
		{
			return Skills[skill].Value.ToString() + ".0";
		}
		else
		{
			return Skills[skill].Value.ToString();
		}
	}

	// Update is called once per frame
	void Update () {

	}
}


[System.Serializable]
public class Skill
{
	public string Name;
	public string Description;
	public float Value;
	public float AdvancementSpeed;

	public bool MustBeUnlocked = false;
	public Dictionary<Skill, float> SkillsNeeded = new Dictionary<Skill, float>();

	public List<System.Type> BonusItems = new List<System.Type> ();

	public bool CanBeUsed = false;


	public Skill() {}

	public Skill(string name, float initialValue, float advancementSpeed, bool canbeUsed = false)
	{

		Name = name;
		Value = initialValue;
		AdvancementSpeed = advancementSpeed;
		MustBeUnlocked = false;
		CanBeUsed = canbeUsed;

	}
	public Skill(string name, float initialValue, float advancementSpeed, Dictionary<Skill, float> skillNeeded, bool canBeUsed = false)
	{
		
		Name = name;
		Value = initialValue;
		AdvancementSpeed = advancementSpeed;
		MustBeUnlocked = true;
		SkillsNeeded = skillNeeded;
		CanBeUsed = canBeUsed;
	}
	public override string ToString ()
	{
		return Name;
	}

	public void ChangeValueTo(float value)
	{
		Value = value;
		GameHelper.SystemMessage ("L'abilita " + Name + " e' ora a " + value.ToString (), Color.blue);
	}

	public float Increase(float difficulty, float minSkill, float maxSkill = 100.0f)
	{
		/*float P = (1 / (Value - minSkill));
		float random = Random.Range (0.0f, 1.0f);
		bool result = random < (P * AdvancementSpeed /* * GlobalScale *///);

		float random = (float) (Random.Range (0, 1000) / 1000.0f);

		float chance = (Value - minSkill) / (maxSkill - minSkill);
		bool result = chance >= random;
		EntitySkills sk = GameHelper.GetPlayerComponent<EntitySkills> ();
		float gc = (sk.SkillCap - sk.SkillsTotal) / sk.SkillCap;
		gc += (100.0f - Value) / 100.0f;
		gc /= 0.2f;
		gc *= AdvancementSpeed;
		if (gc < 0.01f)
			gc = 0.01f;
		//Debug.Log ("P is " + p + ", rand is " + random + ", P * Speed is " + p * AdvancementSpeed + ", result is " + result + ". (Speed was " + AdvancementSpeed + ")"); 
		float inc = 0;
		if (result)
		{
			inc = 0.1f;

		}
		if (Value > maxSkill)
			inc = 0;
		if (inc != 0)
		{
			GameHelper.SystemMessage ("La tua abilità in " + Name + " è aumentata di " + System.Math.Round (inc, 1).ToString () + "!", Color.blue);
			Value += inc;
		}
		return (float)System.Math.Round( inc, 1 );
	}

	public virtual void OnUse()
	{
		Debug.Log (Name);
	}
}

[System.Serializable]
public class Nascondersi : Skill
{
	public Nascondersi()
	{
		Name = "Nascondersi";
		Value = 10;
		AdvancementSpeed = 1;
		CanBeUsed = true;
	}

	public override void OnUse ()
	{
		base.OnUse ();
		(GameHelper.GetPlayerComponent<PlayerMeshManager> () as PlayerMeshManager).ChangeAllColors (Color.gray);
		(GameHelper.GetPlayerComponent<EntityStatus> () as EntityStatus).Hidden = true;
		//foreach(SkinnedMeshRenderer r in GameHelper.GetLocalPlayer().GetComponentsInChildren<SkinnedMeshRenderer>())
		//	r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, 0.4f);
	}
}



public interface ITargetReceiver
{
	void OnTargetReceived(RaycastHit target);
}

public enum CraftingCategory
{
	Equip = 0,
	Spells,
	Potions,
	Misc
}