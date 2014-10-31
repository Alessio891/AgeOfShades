using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum NPCStates
{
	None = 0,
	Idle,
	Wandering,
	Fleeing,
	Following,
	Thinking,
	Attacking
}

[System.Serializable]
public class EntitySounds
{
	public AudioClip AttackClip;
	public AudioClip GetHitClip;
	public AudioClip IdleClip;
	public AudioClip StartledClip;
}

[System.Serializable]
public class EntityLegacyAnimations
{
	public AnimationClip IdleAnimation;
	public AnimationClip StartledAnimation;
	public AnimationClip WalkAnimation;
	public AnimationClip RunAnimation;
	public AnimationClip GetHitAnimation;
	public AnimationClip AttackAnimation;
	public AnimationClip AttackAnimation2;
	public AnimationClip DieAnimation;
}


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PhotonView))]
public class BasicEntity : Photon.MonoBehaviour, ISelectable, ISerializedObject {


	public string uid;
	public EntitySounds Sounds;
	public EntityLegacyAnimations Animations;
	public bool UseMecanim = false;
	public bool OverrideMecanim = false;
	public bool Respawn = false;
	public float WalkSpeed = 2;
	public float RunSpeed = 4;

	protected Vector3 lastPosition;
	protected Vector3 randomDestination;
	protected Vector3 lastPositionCheck;
	protected float _attackTimer = 0;
	public Animator animator;
	public EntityStatus Status;
	
	public WeaponDamageData DamageData;
	public GameObject AttackTarget;
	public NPCStates State = NPCStates.Idle;
	public NPCStates lastState = NPCStates.Idle;
	public GameObject DamageCanvas;
	public bool Dead = false;

	public BaseWeapon RightHand;
	public BaseWeapon LeftHand;

	bool isMoving { get { return lastPositionCheck != transform.position; } }

	public Transform TargetMarkPosition;


	public string EntityName = "Default";

	public LootPack Loot = new LootPack();

	bool fleeing = false;
	bool findNext = true;
	bool idling = false;
	protected float _timer = 0;
	protected float waitFor = 2;

	bool markerActivated = false;
	bool markerDeactivated = false;

	public List<BaseBehaviour> Behaviours = new List<BaseBehaviour>();
	void Awake()
	{
		Status = GetComponent<EntityStatus> ();
	}

	void Start () {
		Status = GetComponent<EntityStatus> ();
		uid = GameHelper.GetUIDForObject (this);
		animator = GetComponent<Animator> ();
		GetComponent<NavMeshAgent> ().enabled = true;
		foreach(Transform t in GetComponentsInChildren<Transform>())
		{
			if (t.name == "DamageCanvas")
			{
				DamageCanvas = t.gameObject;
			}
		}
		Init ();
		

	}
	public BaseBehaviour hasBehaviour(System.Type b)
	{
		foreach(BaseBehaviour be in Behaviours)
		{
			
			if (be.GetType() == b)
			{
				return be;
			}
		}
		return null;
	}
	protected List<object> SerializeBasicAttributes()
	{
		List<object> data = new List<object> ();
		data.Add (DamageData);
		data.Add (UseMecanim);
		data.Add (OverrideMecanim);
		data.Add (new PlayerPosition (transform.position));
		data.Add (Behaviours.Count);
		foreach(BaseBehaviour b in Behaviours)
		{
			data.Add ( b.GetType() );
		}
		return data;
	}

	public virtual object[] Serialize() {
		object[] data;
		data = SerializeBasicAttributes ().ToArray ();
		return data;
	}
	public virtual void DeSerialize(object[] data) {
		DamageData = (WeaponDamageData)data [0];
		UseMecanim = (bool)data [1];
		OverrideMecanim = (bool)data [2];
		transform.position = (data [3] as PlayerPosition).GetVector ();
		int bCount = (int)data [4];
		Behaviours.Clear ();
		for(int i = 5; i < bCount+5; i++)
		{
		//	Behaviours.Add( System.Activator.CreateInstance( data[i] as System.Type ) as BaseBehaviour );
		}
	}
	public virtual string GetUID() { return uid; }
	public virtual void OnLoadingFinished() {

	}

	public bool PlayerIsNearby(float range = 1)
	{
		return (Vector3.Distance (transform.position, GameHelper.GetLocalPlayer ().transform.position) <= range);
	}

	protected List<T> GetNearby<T>(float range)
	{
		List<T> returnValue;
		returnValue = (from o in GameObject.FindObjectsOfType (typeof(T)) select (T)((object) o)).ToList();
	
		//returnValue = (from o in l select (T)o).ToList ();
		return returnValue;
	}

	public List<BasicEntity> GetNearbyLikeMe(float range)
	{
		List<BasicEntity> returnValue = new List<BasicEntity> ();
		returnValue = GetNearby<BasicEntity> (range);
		returnValue.RemoveAll (t => t.GetType () != this.GetType ());
		return returnValue;
	}

	public void AlertOthers(GameObject target, float range = 5)
	{
		foreach(BasicEntity e in GetNearbyLikeMe(range))
		{
			e.Attack(target);
		}
	}

	protected virtual void CheckForQuestRequirement()
	{
		PlayerQuestLog q = GameHelper.GetLocalPlayer ().GetComponent<PlayerQuestLog> ();
		foreach(BaseQuest quest in q.Quests)
		{
			foreach( KeyValuePair<System.Type, int> pair in quest.GetCurrentPhase().Requested )
			{
				if (this.GetType() == pair.Key)
				{
					if (quest.GetCurrentPhase().Killed.ContainsKey(this.GetType()))
					{
						quest.GetCurrentPhase().Killed[this.GetType()] += 1;
					}
					else
					{
						quest.GetCurrentPhase().Killed.Add(this.GetType(), 1);
					}
				}
			}
		}
	}

	protected virtual void Init()
	{
		//FindDestination ();
		gameObject.tag = "Animale";
	}

	public void ActivateMarker()
	{
		foreach(Transform t in GetComponentsInChildren<Transform>())
		{
			if (t.name == "Marker")
			{
				t.renderer.enabled = true;
			}
		}
		markerActivated = true;
		markerDeactivated = false;
	}
	
	public void DeactivateMarker()
	{
		foreach(Transform t in GetComponentsInChildren<Transform>())
		{
			if (t.name == "Marker")
			{
				t.renderer.enabled = false;
			}
		}
		markerActivated = false;
		markerDeactivated = true;
	}
	protected Vector3 lastpos;
	protected virtual void UpdateLogic()
	{

	}

	void Update () {

		/* MECANIM */
		if (UseMecanim && !OverrideMecanim)
		{
			if (isMoving)
			{
				animator.Play( Animator.StringToHash("Base Layer.Walking"));
			}
			else
			{
				animator.Play( Animator.StringToHash("Base Layer.Idle"));
			}
		}

		Vector3 pos = transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight (pos);
		transform.position = pos;

		for (int x = 0; x < Behaviours.Count; x++)
		{
			Behaviours[x].Update();
			if (Behaviours[x].RemoveAtTheEndOfFrame)
			{
				Behaviours.RemoveAt(x);
				x--;
			}
		}

		if (!canBeDamaged)
		{
			_damageDelay += Time.deltaTime;
			if (_damageDelay > 0.6f)
			{
				canBeDamaged = true;
				_damageDelay = 0;
			}
		}

		UpdateLogic ();
		lastPositionCheck = transform.position;
		
	}
	float _damageDelay = 0;
	
	protected virtual void FindDestination()
	{
		Vector3 dest = new Vector3 (transform.position.x + Random.Range (-10, 10),
		                            transform.position.y,
		                            transform.position.z + Random.Range (-10, 10));
		NavMeshPath path = new NavMeshPath ();
		if (!GetComponent<NavMeshAgent>().CalculatePath( dest, path ))
		{
			FindDestination();
			return;
		}
		GetComponent<NavMeshAgent> ().SetDestination (dest);
		randomDestination = dest;
	}
	
	// NETWORK STUFF
	Vector3 lastKnownPosition;
	Quaternion lastKnownRotation;
	
	protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (PhotonNetwork.isMasterClient)
			{
				stream.SendNext( transform.position );
				stream.SendNext( 	(int)State );
				stream.SendNext( transform.rotation );
			}
		}
		else
		{
			if (!PhotonNetwork.isMasterClient)
			{
				lastKnownPosition = (Vector3) stream.ReceiveNext();
				State = (NPCStates)stream.ReceiveNext();
				lastKnownRotation = (Quaternion)stream.ReceiveNext();
			}
		}
	}
	protected bool AlreadySync = false;
	protected virtual void OnPhotonPlayerConnected()
	{
		if (!PhotonNetwork.offlineMode && PhotonNetwork.isMasterClient)
		{
			GetComponent<PhotonView>().RPC("SyncPos", PhotonTargets.Others, new object[] { transform.position });
		}
	}
	
	[RPC]
	protected void SyncPos(Vector3 pos)
	{
		if (!AlreadySync)
		{
			transform.position = pos;
			AlreadySync = true;
		}
	}
	
	[RPC]
	protected void HostSaidGoTo(Vector3 dest)
	{
		if (GetComponent<Animator>().GetBool("Idling"))
			GetComponent<Animator>().SetBool("Idling", false);
		GetComponent<NavMeshAgent> ().SetDestination (dest);
	}

	[RPC]
	protected void HostSaidStopAndIdle(float time)
	{
		if (!GetComponent<Animator>().GetBool("Idling"))
			GetComponent<Animator>().SetBool("Idling", true);
	}
	
	[RPC]
	protected void HostSaidToChangeSpeed(float speed)
	{
		GetComponent<NavMeshAgent> ().speed = speed;
	}
	
	
	[RPC]
	void RemotePlayerIsNear(Vector3 pos)
	{
		
	}

	public void DoDamage(float damage)
	{
		photonView.RPC ("Damage", PhotonTargets.All, new object[] { damage });
	}

	protected bool canBeDamaged = true;

	[RPC]
	public virtual void Damage(float damage)
	{
		if (canBeDamaged)
		{
			Status.Life -= damage;

			if (Status.Life <= 0)
			{
				if (!Dead)
				{
					Die();
					Dead = true;
				}
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
				
				Destroy(t, 1);
				Destroy(o, 1);
			}
			canBeDamaged = false;
		}
	}

	public bool IsAlive {
		get { return Status.Life > 0; }
	}

	public virtual void Attack(GameObject target)
	{
		ChangeState (NPCStates.Attacking);
		AttackTarget = target;
	}
	
	[RPC]
	protected virtual void Die()
	{
		if (!Respawn)
		{
			//CharManager.manager.character.DoNotRespawnList.Add(GetComponent<DoNotRespawnOnLoad>().uid);
			GetComponent<DoNotRespawnOnLoad>().RememberDestroy();	
		}
		GameObject t = GameHelper.GetLocalPlayer ().GetComponent<PlayerCombat> ().Target;
		if (t.GetInstanceID() == gameObject.GetInstanceID())
		{
			Destroy(GameHelper.GetLocalPlayer().GetComponent<PlayerCombat>().markerParent);
			Destroy(GameHelper.GetLocalPlayer().GetComponent<PlayerCombat>().TargetMarker);	
		}

		PlayerCombat combat = (PlayerCombat)GameHelper.GetPlayerComponent<PlayerCombat> ();
		if (!combat.FreeSpot(gameObject))
		{
			Debug.LogError("Il nemico non era presente negli slot del giocatore. Controllare che venga correttamente eliminato dalla lista.");
		}
	}

	[RPC]
	protected virtual void AddCorpse()
	{

		GameObject o = Instantiate(Resources.Load ("CorpseItem")) as GameObject;
		o.transform.position = transform.position;
		Corpse i = o.GetComponent<Corpse>();
		i.loot = Loot;
		Destroy (gameObject, 1.5f);
	}

	protected virtual IEnumerator WaitAndDie()
	{
		yield return new WaitForSeconds(1.5f);
		if (!PhotonNetwork.offlineMode && PhotonNetwork.inRoom)
		{
			photonView.RPC("AddCorpse", PhotonTargets.All, null);
		}
		else
		{
			AddCorpse();
		}
	}

	protected virtual void ChangeState(NPCStates newState)
	{
		lastState = State;
		State = newState;
	}
	bool hasMarker = false;
	GameObject marker;

	public virtual void OnSelect()
	{
		if (!GameHelper.GetLocalPlayer().GetComponent<PlayerCombat>().WarState)
		{

		}
		else
		{
			GameHelper.GetLocalPlayer().GetComponent<PlayerCombat>().StartAttackOn(gameObject);
			foreach(Canvas c in GetComponentsInChildren<Canvas>())
			{
				if (c.name == "HealthBar")
				{
					c.enabled = true;

				}
			}
		}
	}

	public virtual void OnDeselect() {}

}


public class LootPack
{
	Dictionary<System.Type, float> Loot = new Dictionary<System.Type, float> ();
	List<int> amounts = new List<int>();
	public LootPack( Dictionary<System.Type, float> loot)
	{
		Loot = loot;
	}

	public LootPack()
	{
	}

	public int ItemsInLoot { get { return Loot.Count; } }

	public void AddLootItem(System.Type itemType, int amount, float chance)
	{
		if (amount <= 0)
			return; 
		amounts.Add (amount);
		Loot.Add (itemType, chance);
	}

	public Dictionary<System.Type, int> GetRandomizedLoot()
	{
		Dictionary<System.Type, int> _loot = new Dictionary<System.Type, int> ();
		int i = 0;
		foreach( KeyValuePair<System.Type, float> pair in Loot)
		{
			Debug.Log( pair.Key + " chance: " + pair.Value );
			if (Random.Range(0.0f, 1.0f) <= pair.Value)
			{
				_loot.Add( pair.Key, amounts[i]);
			}
			i++;
		}

		return _loot;
	}

	public LootPack Copy()
	{
		LootPack p = new LootPack (Loot);
		return p;
	}

}

public interface IFriendly
{
}

public interface IHostile
{
}

public interface INeutral
{
}