using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseSpell : MonoBehaviour, ISpell {

	// Variables
	public string _name;
	public string description;
	public int circle;
	public ParticleSystem effect;	protected Elements element;
	public Sprite icon;
	public List<Reagent> reagents = new List<Reagent>();
	public float requiredMagery = 0;
	public float requiredMP = 1;
	protected bool fizzled = false;
	public ICaster Caster;
	public bool IsProjectile = false;
	public string powerWords;
	public bool _castWalking;

	// Properties
	public int Circle { get { return circle; } }
	public string Description { get { return description; } set { description = value; } }
	public List<Reagent> ReagentsNeeded { get { return reagents; } }
	public Sprite Icon { get { return icon; } set { icon = value; } }
	public string Name { get { return _name; } set { _name = value; } }
	public GameObject Effect { get { return effect.gameObject; } set { effect = value.GetComponent<ParticleSystem>(); } }
	public GameObject GO { get { return gameObject; } }
	public Elements Element { get { return element; } set { element = value; } }
	public float RequiredMagery { get { return requiredMagery; } set { requiredMagery = value; } }
	public string PowerWords { get { return powerWords; } set { powerWords = value; } }
	public bool CanCastWhileWalking { get { return _castWalking; } set { _castWalking = value; } }
	// Methods
	protected IEnumerator disable(float time)
	{
		yield return new WaitForSeconds (time);
		FinalizeCasting ();
		effect.Stop ();
		effect.time = 0;
		effect.enableEmission = false;
		Destroy (gameObject);
	}

	public virtual void Cast(ICaster caster)
	{
		Caster = caster;	

	}

	public virtual void FinalizeCasting()
	{
		EntitySpells s = (EntitySpells)GameHelper.GetPlayerComponent<EntitySpells> ();
		s.currentSpell = null;	
	}

	public virtual void Fizzle()
	{
		fizzled = true;
	}

	public void ConsumeMP(float mp)
	{
		Caster.ConsumeMP (mp);
	}

	public void PronunceWords()
	{
		GameHelper.ShowNotice (PowerWords, Caster.Transform.gameObject);
	}

	public virtual bool CanCast()
	{

		EntitySkills s = (EntitySkills)GameHelper.GetPlayerComponent<EntitySkills> ();
		if (!s.SkillCheckSuccessful("Magia", requiredMagery, 1))
		{
			float inc = s.SkillIncreaseSuccessful ("Magia", 1, requiredMagery);
			GameHelper.SystemMessage("Non sei abbastanza abile nella magia...", Color.red);
			return false;
		}
		foreach(Reagent r in reagents)
		{
			if (!GameHelper.GetPlayerComponent<PlayerInventory>().Has(r.name, 1))
			{
				GameHelper.SystemMessage("Sei a corto di " + r.name + "!", Color.red);
				return false;
			}
		}
		if (Caster.isPlayer)
		{
			
			Debug.Log("Player is casting");
			if (!Caster.HasEnoughMP(requiredMP))
			{
				GameHelper.SystemMessage("Non hai abbastanza mana!", Color.red);
				return false;
			}

		}

		return true;
	}

	public virtual void InterruptCast()
	{
	}

	// Use this for initialization
	void Start () {
		_name = "TestSpell";
	}
	
	// Update is called once per frame
	void Update () {

	}
}
