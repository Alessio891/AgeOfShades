using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public enum CharacterState
{
	Idle = 0,
	Walking,
	Running,
	Dead
}

public class ClickToMove : MonoBehaviour {


	public bool Enabled = true;
	NavMeshAgent agent;
	Animator animator;
	public Animator animatorPrefab;
	NavMeshPath currentPath;
	public float WalkSpeed = 3;
	public float RunSpeed = 8;
	public float RunDistance = 3;
	public float SpeedModifier = 0;
	public bool IsMoving { get { return (animator != null && animator.GetInteger("CharacterState") != (int)CharacterState.Idle); } }
	public bool IsWalking { get { return (animator != null && animator.GetInteger ("CharacterState") == (int)CharacterState.Walking); } }
	public bool IsRunning { get { return (animator != null && animator.GetInteger ("CharacterState") == (int)CharacterState.Running); } }
	public bool IsCasting { get { return ((GameHelper.GetPlayerComponent<EntitySpells> () as EntitySpells).currentSpell != null); } }
	public bool Targeting = false;

	public bool waitForTarget = false;
	public ITargetReceiver TargetReceiver;
	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent> ();
		
		animator = GetComponent<Animator> ();
		lastPos = transform.position;
		currentPath  = new NavMeshPath();
	}

	void OnDeserialized()
	{
		Debug.Log ("Deserializing ClickToMove Component...");
		agent = GetComponent<NavMeshAgent> ();
		GameObject mec = Resources.Load ("Mecanim") as GameObject;
		animator.avatar = mec.GetComponent<Animator> ().avatar;

		lastPos = transform.position;
		currentPath  = new NavMeshPath();
		agent.path.ClearCorners ();
	}

	Vector3 lastPos;
	bool holdingToMove = false;
	bool pressedOnce = false;
	bool meetWithEntity = false; 
	// Update is called once per frame

	public void WaitForTarget(ITargetReceiver receiver)
	{
		TargetReceiver = receiver;
		Targeting = true;
		waitForTarget = true;
	}

	void LateUpdate()
	{

	}

	void Update () {
		if (agent == null)
			agent = GetComponent<NavMeshAgent> ();
		if (animator.avatar == null)
			animator.avatar = Resources.Load("Human_Rig_StaccatoAvatar") as Avatar;

		if (Input.GetMouseButton(1))
		{
			
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit))
			{
				if (hit.collider.name == "Terrain")
				{
					Vector3 groundPos = hit.point;
					//groundPos.y = Terrain.activeTerrain.SampleHeight(groundPos);
					if (CanGoThere(groundPos))
					{
						holdingToMove = true;
						float distance = Vector3.Distance(transform.position, groundPos);
//						Debug.Log("HIT DISTANCE: " + distance);
						if (distance < RunDistance)
						{
							agent.speed = 3 + SpeedModifier;
						}
						else
						{
							agent.speed = 8 + SpeedModifier;
						}
						agent.SetPath(currentPath);
						
					}
					else
					{
//						GameHelper.SpeechBubble("Non posso raggiungerlo.", GameHelper.GetLocalPlayer());
					}
				}
			}
		}
		else
		{
			if (holdingToMove)
			{
				agent.Stop ();
				pressedOnce = false;
			}
				holdingToMove = false;
			
		}
		if (agent.path.status == NavMeshPathStatus.PathPartial)
		{
			agent.SetDestination(transform.position);
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystemManager.currentSystem.IsPointerOverEventSystemObject())
			{
				return;
			}
			if (Targeting)
			{
				if (!waitForTarget)
					return;
				else
				{
					Ray ra = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hitt;
					if (Physics.Raycast(ra, out hitt))
					{
						if (TargetReceiver != null)
						{
							TargetReceiver.OnTargetReceived(hitt);
						}

					}
					waitForTarget = false;
					TargetReceiver = null;
					Targeting = false;
				}
			}
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit))
			{
				if (hit.collider.name == "Terrain")
				{
					Vector3 groundPos = hit.point;
					if (CanGoThere(groundPos))
					{
						bool targetInTheWay = false;

						// Check if our target is in the way, if it is, stop at the attack radius distance
						if (GetComponent<PlayerCombat>().Target != null)
						{
							Ray targetRay = new Ray(transform.position + new Vector3(0, 2, 0), groundPos - transform.position);
							RaycastHit targetHit;
							if (Physics.SphereCast(targetRay, 2, out targetHit, hit.distance))
							{
								Debug.Log(targetHit.collider.name);
								if (targetHit.collider.gameObject.GetInstanceID() == GetComponent<PlayerCombat>().Target.GetInstanceID())
								{
									targetInTheWay = true;
									meetWithEntity = true;
								}
							}
						}
						if (!targetInTheWay)
						{
							float distance = Vector3.Distance(transform.position, groundPos);
							if (distance < RunDistance)
							{
								agent.speed = 3 + SpeedModifier;
							}
							else
							{
								agent.speed = 8 + SpeedModifier;
							}
							agent.SetPath(currentPath);
							GameHelper.SystemMessage("La distanza da percorrere è " + agent.remainingDistance.ToString() + " metri", Color.red);
						}
						else
						{
							GameObject targ = GetComponent<PlayerCombat>().Target;
							if (CanGoThere(targ.transform.position + targ.transform.forward.normalized * (GetComponent<PlayerCombat>().AttackRadius)))
							{
								agent.SetPath(currentPath);
							}
						}
					}
				}
				else if (hit.collider.GetComponent(typeof(ISelectable)) != null)
				{
					ISelectable s = hit.collider.GetComponent(typeof(ISelectable)) as ISelectable;
					s.OnSelect();
				}

			}
		}

		if (IsMoving && (GameHelper.GetPlayerComponent<EntityStatus> () as EntityStatus).Hidden)
		{
			(GameHelper.GetPlayerComponent<EntityStatus> () as EntityStatus).Hidden = false;
			(GameHelper.GetPlayerComponent<PlayerMeshManager> () as PlayerMeshManager).RestoreOldColors();
		}

		EntityStatus ps = GameHelper.GetPlayerComponent<EntityStatus> ();
		if (ps.Exhausted)
		{
			agent.speed = 2 + SpeedModifier;
		}


		UpdatePath ();
		UpdateAnimation ();
		if (agent.isPathStale)
						agent.Stop ();
		lastPos = transform.position;

	}

	void UpdatePath()
	{
		if (meetWithEntity)
		{
			GameObject t = GetComponent<PlayerCombat>().Target;
			CanGoThere(t.transform.position + (t.transform.forward * GetComponent<PlayerCombat>().AttackRadius));
		}
	
		if (agent.remainingDistance <= 0.1f)
		{
			agent.Stop();
			meetWithEntity = false;
		}


	}

	void UpdateAnimation()
	{
		if (agent.velocity != Vector3.zero)
		{
			if (agent.speed <= WalkSpeed)
			{
				animator.SetInteger("CharacterState", (int)CharacterState.Walking);
				//GameHelper.SetPlayerState(CharacterState.Walking);

			}
			else
			{
				animator.SetInteger("CharacterState", (int)CharacterState.Running);
				//GameHelper.SetPlayerState(CharacterState.Running);
			}
		}
		else
		{
			animator.SetInteger("CharacterState", (int)CharacterState.Idle);
			//GameHelper.SetPlayerState(CharacterState.Idle);
		}
	}

	public bool CanGoThere(Vector3 point)
	{
		bool value = false;

		agent.CalculatePath (point, currentPath);

		return (agent.path.status != NavMeshPathStatus.PathPartial);

	}

	public bool GoTo(Vector3 position)
	{
		if (CanGoThere(position))
		{
			agent.SetPath (currentPath);
			animator.SetInteger("CharacterState", (int)CharacterState.Walking);
			return true;
		}
		else
			return false;
	}

}


public interface ISelectable
{
	void OnSelect();
	void OnDeselect();
}