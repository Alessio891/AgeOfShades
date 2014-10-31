using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
[System.Serializable]
public enum Elements
{
	Neutral = 0,
	Fire,
	Water,
	Lightning
}

[System.Serializable]
public class WeaponDamageData
{
	public float minDamage;
	public float maxDamage;
	public string skillUsed;
	public float skillNeeded;
	public Elements element;
	[System.NonSerialized]
	public GameObject owner;
	public WeaponDamageData() 
	{

	}

	public WeaponDamageData (float min, float max, string skill, float skillneeded, Elements element = Elements.Neutral)
	{
		minDamage = min;
		maxDamage = max;
		skillUsed = skill;
		this.skillNeeded = skillneeded;
		this.element = element;
	}

	public float GetBaseDamage()
	{
		return UnityEngine.Random.Range (minDamage, maxDamage);
	}
	public float Tronca(float value, int digits)
	{
		double mult = System.Math.Pow(10.0, digits);
		double result = System.Math.Truncate( mult * value ) / mult;
		return (float) result;
	}
	
	public float GetFinalDamage()
	{
		float baseD = GetBaseDamage();
		baseD *= ApplySkillModifier (baseD);

		return Tronca (baseD, 1);

	}

	float ApplySkillModifier(float baseDamage)
	{
		if (string.IsNullOrEmpty(skillUsed))
			return 1;
		float skillValue = 0;
		if (owner == null)
			skillValue = GameHelper.GetLocalPlayer ().GetComponent<EntitySkills> ().Skills [skillUsed].Value;
		else
			skillValue = owner.GetComponent<EntitySkills>().Skills[skillUsed].Value;
		float ratio = (skillValue / skillNeeded);
		if (ratio < 0)
			return 0;

		if (ratio < 0.5f)
			return 0.5f;
		if (ratio >= 0.5f && ratio < 0.8f)
			return 0.7f;
		if (ratio >= 0.8f && ratio <= 1)
			return 1;
		else
			return 1.8f;

	}


}

public class PlayerCombat : MonoBehaviour, ISerializedObject {
	public float AttackRadius = 1;
	public float AttackDelay = 0.5f;
	public float AttackDamage = 1;
	public WeaponDamageData Damage;
	public WeaponDamageData Fists;
	bool canAttack = true;
	float _attackDelay = 0;
	public string SkillUsed = "Null";
	public bool Attacking = false;
	bool scaledTime = false;
	public GameObject markerParent;
	string uid;
	
	//NEWVARS
	public bool WarState = false;
	public GameObject Target;
	public GameObject TargetMarker;

	public EnemySpot[] Spots = new EnemySpot[8];
	//public List<EnemySpot> spots = new List<EnemySpot>(8);
	// Use this for initialization
	void Start () {
		AttackRadius = 1.5f;
		WarState = false;
		Fists = new WeaponDamageData (0.5f, 1.2f, "Corpo A Corpo", 5, Elements.Neutral);
		GetComponent<Animator>().SetBool("Attacking", false);

		for(int i = 0; i < Spots.Length; i++)
		Spots[i] = new EnemySpot() { Free = true };
		uid = GameHelper.GetUIDForObject (this);
	}

	public int FreeSpotAvailable()
	{
		for(int i = 0; i < Spots.Length; i++)
		{
			if (Spots[i].Free)
			{
				Ray r = new Ray(transform.position + transform.up, GetSpotPosition(i) - transform.position);
				RaycastHit hit;
				if (Physics.Raycast(r, out hit, 2))
				{
					if (hit.collider.tag == "Friendly" || hit.collider.gameObject.GetComponent<BasicEntity>() != null)
					{
						continue;
					}
				}
				return i;
			}
		}
		return -1;
	}

	public bool FreeSpot(GameObject occupant)
	{
		for(int i = 0; i < Spots.Length; i++)
		{
			if (Spots[i].Occupant.GetInstanceID() == occupant.GetInstanceID())
			{
				Spots[i].Free = true;
				Spots[i].Occupant = null;
				return true;
			}
		}
		return false;
	}

	public Vector3 GetSpotPosition(int index)
	{
		Vector3 r_value = Vector3.zero;
		switch( index )
		{ // FUNZIONE CHE RITORNA POSTO IN BASE A INDEX!!!
		case 0:
			r_value = Vector3.forward.normalized;
			break;
		case 1:
			r_value = -Vector3.forward.normalized;
			break;
		case 2:
			r_value = Vector3.right.normalized;
			break;
		case 3:
			r_value = -Vector3.right.normalized;
			break;
		case 4:
			r_value = (Vector3.forward + Vector3.right).normalized;
			break;
		case 5:
			r_value = (Vector3.forward - Vector3.right).normalized;
			break;
		case 6:
			r_value = -(Vector3.forward + Vector3.right).normalized;
			break;
		case 7:
			r_value = -(Vector3.forward - Vector3.right).normalized;
			break;
		}
		return r_value;
	}

	public int GetOccupantIndex(GameObject occupant)
	{
		for(int i = 0; i < Spots.Length; i++)
		{
			if (Spots[i].Occupant.GetInstanceID() == occupant.GetInstanceID())
				return i;
		}
		return -1;
	}
	bool chasingTarget = false;
	public void StartAttackOn (GameObject target)
	{
		if (!(target.GetComponent<BasicEntity>() is IHostile))
			return;	
		if (target != Target)
		{
			Destroy (TargetMarker);
			Destroy (markerParent);
			if (Target != null)
			{
				foreach(Canvas c in Target.GetComponentsInChildren<Canvas>())
				{
					if (c.name == "HealthBar")
					{
						c.enabled = false;
					}
				}
			}
			Target = target;
			TargetMarker = Instantiate (Resources.Load ("ObjectMarker")) as GameObject;
			Vector3 scale = TargetMarker.transform.localScale;
			TargetMarker.transform.position = Target.transform.position + new Vector3(0,0.1f,0);
			TargetMarker.GetComponent<Animator>().Play(Animator.StringToHash("Base Layer.EnemyMarkerAnimation"));

			markerParent = new GameObject ();
			markerParent.transform.position = Target.transform.position;
			float y = Terrain.activeTerrain.SampleHeight(transform.position);
			Vector3 newpos = markerParent.transform.position;
			newpos.y = y + 0.1f;
			markerParent.transform.position = newpos;
			markerParent.transform.SetParent (Target.transform, true);
			markerParent.transform.localScale = Vector3.one;

			TargetMarker.transform.SetParent (markerParent.transform, true);
		}
		else
		{
			NavMeshAgent agent = (GameHelper.GetPlayerComponent<NavMeshAgent>() as NavMeshAgent);
			ClickToMove cm = GameHelper.GetPlayerComponent<ClickToMove>() as ClickToMove;
			if (Vector3.Distance(Target.transform.position, agent.transform.position) < cm.RunDistance)
			{
				agent.speed = cm.WalkSpeed;
			}
			else
				agent.speed = cm.RunSpeed;
			chasingTarget = true;
		}
	}	
	public bool IsWalkable(int i)
	{
		bool value = true;

		return value;
	}
	public bool IsSpotOccupiedBy(int index, GameObject occupant)
	{
		return (!Spots [index].Free && Spots [index].Occupant.GetInstanceID () == occupant.GetInstanceID ());
	}
	public bool EnemyHasOccupiedSpot(GameObject occupant)
	{
		foreach(EnemySpot spot in Spots)
		{
			if (spot.Free)
				continue;
			if (spot.Occupant.GetInstanceID() == occupant.GetInstanceID())
				return true;
		}
		return false;
	}
		// Update is called once per frame
	void LateUpdate () {

		//spots = Spots.ToList ();
		if (chasingTarget)
		{
			if (Vector3.Distance(transform.position, Target.transform.position) > 1.5f)
				GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);
			else
				chasingTarget = false;
			
		}
		if (GetComponent<ClickToMove>().IsMoving)
		{
			for(int i = 0; i < Spots.Length; i++)
			{
				Spots[i].Free = true;
				Spots[i].Occupant = null;
			}
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			WarState = !WarState;
			if (!WarState)
			{
				if (TargetMarker != null)
					Destroy(TargetMarker);
				if (Target != null)
					Target = null;
			}
		}
		GetComponent<Animator> ().SetBool ("WarState", WarState);

		/* Debug 
		if (Input.GetKey(KeyCode.LeftControl))
		{
			if (!scaledTime)
			{
				Time.timeScale = 0.1f;
				scaledTime = true;
			}
			else
			{
				Time.timeScale = 1;				
				scaledTime = false;
			}
		}
		/* Debug End */

		PlayerEquip equip = (PlayerEquip)GameHelper.GetPlayerComponent<PlayerEquip> ();

		if (equip.RightHand != null)
		{
			AttackRadius = equip.RightHand.AttackRange;
			AttackDelay = equip.RightHand.AttackDelay;
			AttackDamage = equip.RightHand.AttackDamage;
		}
		else
		{
			AttackRadius = 2.5f;
			AttackDelay = 1;
			AttackDamage = 1;
		}
		GetComponent<Animator>().SetBool("Attacking", false);
		if (Target != null && Vector3.Distance(transform.position, Target.transform.position) <= AttackRadius)
		{
			Attack();
		}

		if (!canAttack)
		{
			_attackDelay += Time.deltaTime;
			if (_attackDelay >= AttackDelay)
			{
				_attackDelay = 0;
				canAttack = true;
			}
		}
	}

	void Attack()
	{
		if (!Target.GetComponent<BasicEntity>().IsAlive)
			return;
		if (Target.GetComponent<BasicEntity>() == null)
		{
			foreach(Canvas c in Target.GetComponentsInChildren<Canvas>())
			{
				if (c.name == "HealthBar")
				{
					c.enabled = false;
				}
			}
			Target = null;
			return;
		}
		if (!GetComponent<ClickToMove>().IsMoving)
		{
			transform.rotation = Quaternion.LookRotation (Target.transform.position - transform.position);
			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
		}
		if (canAttack)
		{
			GetComponent<Animator>().SetBool("Attacking", true);
			canAttack = false;
			BaseWeapon rHand = (GameHelper.GetPlayerComponent<PlayerEquip>() as PlayerEquip).RightHand;
			if (rHand != null)
			{
				rHand.OnAttack(Target);
			}
			else
			{
				EntitySkills skills = (EntitySkills)GameHelper.GetPlayerComponent<EntitySkills>();
				if (skills.SkillCheckSuccessful("Corpo A Corpo", 10, 1))
				{
					Target.GetComponent<BasicEntity>().Damage(Fists.GetFinalDamage());
					skills.SkillIncreaseSuccessful("Corpo A Corpo", 1, 10);
				}
			}
		}
	}

	public object[] Serialize()
	{
		object[] data = new object[1];
		data[0] = WarState;
		return data;
	}

	public void DeSerialize(object[] data)
	{
		WarState = (bool)data [0];
	}

	public void OnLoadingFinished() {}

	public string GetUID() { return uid; }

}

[System.Serializable]
public class EnemySpot
{
	public GameObject Occupant;
	public bool Free = true;
}