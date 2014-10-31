using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityStatus : MonoBehaviour, ISerializedObject {

	public float Strength = 10;
	public float Dexterity = 10;
	public float Intellect = 10;

	public float MaxWeight = 40;
	public float Weight {
		get {
			PlayerInventory pi = GameHelper.GetPlayerComponent<PlayerInventory>();
			float w = 0;
			foreach(InventoryEntry i in pi.Inventory)
			{
				w += i.Item.Weight;
			}
			return w;
		}
	}
	public bool AttachedToPlayer { get { return (GetComponent<PlayerManager> () != null); } }
	public List<IBuff> Buffs = new List<IBuff> ();

	public float Hunger = 0;
	public float Thirst = 0;

	public float Stamina = 10;

	public float Mana = 10;

	public bool Hidden = false;
		
	public float CheckEvery = 60;
	float _timer = 0;
	public bool killOnHungry = false;

	public float Life = 10;
	public float MaxLife = 10;

	[HideInInspector]
	public bool Exhausted = false;

	float _stamTimer = 0;
	float stamRegen = /*Tick ogni*/ 0.3f /*secondi */;
	float stamRegenAmount = 0.5f /*stamina ogni stamRegen secondi*/;
	float stamDrain = 0.2f;
	float stamDrainAmount = 0.4f;

	float _manaTimer = 0;
	float manaRegen = 0.3f;
	float manRegenAmount = 0.2f;

	public GameObject DamageCanvas;
	public string uid;

	// Use this for initialization
	void Start () {
		uid = GameHelper.GetUIDForObject (this);
		foreach(Transform t in GetComponentsInChildren<Transform>())
		{
			if (t.name == "DamageCanvas")
			{
				DamageCanvas = t.gameObject;
			}
		}

		Strength = 40;
		Life = 40;
		MaxLife = 40;
	}

	public object[] Serialize() {
		object[] data = new object[6];
		data [0] = Strength;
		data [1] = Dexterity;
		data [2] = Intellect;
		data [3] = Hunger;
		data [4] = Life;
		data [5] = Stamina;

		return data;
	}
	public void DeSerialize(object[] data) {
		Strength = (float)data [0];
		Dexterity = (float)data [1];
		Intellect = (float)data [2];
		Hunger = (float)data [3];
		Life = (float)data [4];
		Stamina = (float)data [5];
		MaxLife = Strength;
	}
	public string GetUID() { return uid; }
	public void OnLoadingFinished() {}

	IEnumerator BuffTick(float tickEvery, IBuff buff)
	{
		float _elapsed = 0;
		while(_elapsed < buff.Duration)
		{
			if (buff.ToBeRemoved)
			{
				break;
			}
			buff.OnTick ();
			_elapsed += tickEvery;
			yield return new WaitForSeconds(tickEvery);
		}
		RemoveBuff (buff);
	}

	public void ApplyBuff(IBuff buff)
	{
		buff.Status = this;
		Buffs.Add (buff);
		buff.OnApply ();
		StartCoroutine (BuffTick (buff.TickEvery, buff));
	}

	public void RemoveBuff(IBuff buff)
	{
		int i = 0;
		foreach(IBuff b in Buffs)
		{
			if (b.Name == buff.Name)
			{
				b.OnDecay();
				break;
			}
			i++;
		}
		if (i < Buffs.Count)
			Buffs.RemoveAt(i);
	}

	public bool HasBuff(System.Type t)
	{
		foreach(IBuff buff in Buffs)
		{
			if (buff.GetType() == t)
				return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.F3) && AttachedToPlayer)
		{
			ApplyBuff( new HiddenBuff() );
		}

		// Hunger //
		_timer += Time.deltaTime;
		MaxLife = Strength;
		if (Life > MaxLife)
			Life = MaxLife;
		if (_timer > CheckEvery)
		{
			_timer = 0;
			Hunger += 0.1f;
			Debug.Log(Hunger);
			Hunger = (float)System.Math.Round(Hunger, 1);
			string mess = "Inizio ad avere fame...";
			if (Hunger == 0.5f)
				mess = "Devo assolutamente mangiare qualcosa.";
			else if (Hunger == 0.6f)
				mess = "La fame inizia ad indebolirmi...";
			else if (Hunger == 0.7f)
				mess = "Devo procurarmi del cibo in fretta.";
			else if ( Hunger == 0.8f)
				mess = "Non posso resistere a lungo, devo mangiare qualcosa o svengo.";
			else if (Hunger == 0.9f)
				mess = "Non riesco a resistere, la fame è troppa.";
			else if (Hunger > 1)
				mess = "Sverrò da un momento all'altro...";
			//if (Hunger > 0.4f)
			//	GameObject.Find("NoticePanel").GetComponent<NoticePanel>().Play(mess);
			// Do something here
		
			if (Hunger > 1f && killOnHungry)
			{
				Hunger = -100;
				Die ();
				//Camera.main.gameObject.GetComponent<Animator>().Play("KnockedOut");
			}
		}
		// /Hunger //

		// Stamina // Disabled for enemies

		ClickToMove ctm = GetComponent<ClickToMove> ();
		PlayerInventory pi = GetComponent<PlayerInventory> ();
		if (ctm != null)
		{
			if (ctm.IsMoving)
			{
				float ratio = Weight / MaxWeight;
				if (ratio > 0.3f)
				{ 
					if (ctm.IsWalking)
					{
						Stamina -= ratio / 10;
					}
					else
					{
						Stamina -= ratio / 5;
					}

					if (Stamina <= 0)
					{
						Exhausted = true;
					}
				}
			}
			if (Stamina < 0)
				Stamina = 0;
			if (Stamina < Dexterity)
			{
				_stamTimer += Time.deltaTime;
				float timeToWait = stamRegen; //(GetComponent<ThirdPersonController>()._characterState != CharacterState.Down) ? stamRegen : 0.01f;
				if (_stamTimer >= timeToWait)
				{
					Stamina += stamRegenAmount;//(GetComponent<ThirdPersonController>()._characterState != CharacterState.Down) ? stamRegenAmount : stamRegenAmount * 10;
					Stamina = Mathf.Clamp(Stamina, 0, Dexterity);
					_stamTimer = 0;
					if ( (Stamina/Dexterity) > 0.2f && Exhausted)
					{
						Exhausted = false;
					}
				}
			}
		}
 
		// /Stamina // */

		// MANA //

		if (Mana < 0)
			Mana = 0;
		if (Mana > Intellect)
			Mana = Intellect;
		if (Mana < Intellect)
		{
			_manaTimer += Time.deltaTime;
			float timeToWait = manaRegen; //(GetComponent<ThirdPersonController>()._characterState != CharacterState.Down) ? stamRegen : 0.01f;
			if (_manaTimer >= timeToWait)
			{
				Mana += manRegenAmount;//(GetComponent<ThirdPersonController>()._characterState != CharacterState.Down) ? stamRegenAmount : stamRegenAmount * 10;
				Mana = Mathf.Clamp(Mana, 0, Intellect);
				_manaTimer = 0;
		
			}
		}

		// /MANA //
	}

	public void Damage(float damage)
	{
		Life -= damage;

//		GameObject.Find("HP").GetComponent<UIPanel>().SetAlphaRecursive(1, true);
		if (Life <= 0)
		{
			Die();
		}
		else
		{
			GameObject t = Instantiate( Resources.Load("DamageText") ) as GameObject;
			t.transform.position = DamageCanvas.transform.position;
			t.GetComponent<UnityEngine.UI.Text>().text = damage.ToString();
			GameObject o = new GameObject();
			o.transform.position = DamageCanvas.transform.position;
			o.transform.SetParent(DamageCanvas.transform);
			o.transform.localScale = Vector3.one;
			
			t.transform.SetParent(o.transform, true);
			
			Destroy(t, 3);
			Destroy(o, 3);
		}
	}

	public void Die()
	{
		//Camera.main.gameObject.GetComponent<Animator> ().Play ("KnockedOut");
//		GameObject.Find ("GameOverPanel").GetComponent<UIPanel> ().SetAlphaRecursive (1, true);
		StartCoroutine (DisablePlayer ());
	}

	IEnumerator DisablePlayer()
	{
		yield return new WaitForSeconds(GameHelper.GetLocalPlayer ().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
		
	}
}
