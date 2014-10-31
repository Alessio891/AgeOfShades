using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
public enum ItemTypes
{
	Debug = 0,
	Weapon,
	Armor,
	Material
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(DoNotRespawnOnLoad))]
[BFCategory("Base/Oggetto Base")]
public class BasicItem : Photon.MonoBehaviour, ISelectable {

	protected GameObject player;
	public GameObject actionTag;
	public GameObject marker;
	public bool hasMarker = false;
	protected bool nearPlayer = false;
	public float ActivationDistance = 3;
	public float NameVisibilityDistance = 10; 
	public string ItemName = "Default";
	public bool Respawn = false;

	public string onUseAnimation;

	protected GUIText ItemNameText;
	protected GUITexture ItemIcon;

	protected GameManager gameManager;
	public string uid;

	public ItemTypes Type = ItemTypes.Debug;

	[HideInInspector]
	public GameObject ActionPanel;
	[HideInInspector]
	public Camera ActionCamera;

	public bool CanPickup = true;
	public InventoryItem PickedUpItem;
	public string PickupScriptName = "WoodLog";

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		gameManager = GameManager.instance;
		PickedUpItem = (InventoryItem)System.Activator.CreateInstance (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, PickupScriptName.Replace(" ", "_")).Unwrap();
		SetupForNetwork ();
		Init ();
		uid = GameHelper.GetUIDForObject (this);
	}


	float waitingToPickup = 0;
	IEnumerator WaitForArrivalAndPickup()
	{
		float dist = Vector3.Distance (GameHelper.GetLocalPlayer ().transform.position, transform.position);
		UnityEngine.Debug.Log ("DISTANCE FROM " + name + " is " + dist + " meters");
		if (!IsNearPlayer())
		{
			waitingToPickup += 0.1f;
			if (waitingToPickup > 5)
			{
				waitingToPickup = 0;
				if (marker != null)
					Destroy(marker);
				return false;
			}
			yield return new WaitForSeconds(0.1f);
			StartCoroutine(WaitForArrivalAndPickup());
		}
		else
		{
			if (marker != null)
				Destroy(marker);
			OnUse ();
			if (string.IsNullOrEmpty(onUseAnimation))
			{
				KneelToPickup();
			}
			else
			{
				GameHelper.GetLocalPlayer().GetComponent<Animator>().Play(Animator.StringToHash("Actions."+onUseAnimation));
			}

		}

	}



	public virtual void OnSelect()
	{
		if (!hasMarker)
		{
			PlayerInventory pi = (PlayerInventory)GameHelper.GetPlayerComponent<PlayerInventory>();
			if (pi.Selected != null)
			{
				BasicItem bi = pi.Selected.GetComponent<BasicItem>();
				bi.hasMarker = false;
				Destroy(bi.marker);
			}
			hasMarker = true;
			marker = Instantiate (Resources.Load ("ObjectMarker")) as GameObject;
			Vector3 scale = marker.transform.localScale;
			marker.transform.position = transform.position;
			float y = Terrain.activeTerrain.SampleHeight(transform.position);
			Vector3 newpos = marker.transform.position;
			newpos.y = y + 0.1f;
			marker.transform.position = newpos;
			pi.Selected = gameObject;
		}
		else
		{
			if (!GameHelper.GetLocalPlayer ().GetComponent<ClickToMove> ().GoTo (transform.position))
			{
				NavMeshAgent agent = (GameHelper.GetPlayerComponent<NavMeshAgent>() as NavMeshAgent);
				ClickToMove cm = GameHelper.GetPlayerComponent<ClickToMove>() as ClickToMove;
				if (Vector3.Distance(transform.position, agent.transform.position) < cm.RunDistance)
				{
					agent.speed = cm.WalkSpeed;
				}
				else
					agent.speed = cm.RunSpeed;
			}
		
			StartCoroutine (WaitForArrivalAndPickup ());
			
		}
	}


	public virtual void OnDeselect()
	{
	}

	protected virtual void Init()
	{
	}
	
	// Update is called once per frame
	void Update () {

		if (GameObject.Find("GameManager").GetComponent<GameManager>().ShowItemNames)
		{
			if (DistanceFromPlayer < NameVisibilityDistance)	
			{
				if (actionTag == null)
				{
					actionTag = Instantiate( Resources.Load("ActionTag") ) as GameObject;
					GameObject bubble = null;
					foreach(Transform t in GetComponentsInChildren<Transform>())
					{
						if (t.name == "BubblePosition")
						{
							bubble = t.gameObject;
							break;
						}
					}
					if (bubble == null)
						actionTag.transform.position = transform.position;
					else
						actionTag.transform.position = bubble.transform.position;
				
					
					actionTag.GetComponentInChildren<UnityEngine.UI.Text>().text = ItemName;
				}

			}
		}
		else
		{
			if (actionTag != null)
				Destroy(actionTag);
		}

		UpdateLogic ();
	}

	public float DistanceFromPlayer { get { return Vector3.Distance (GameHelper.GetLocalPlayer ().transform.position, transform.position); } }

	public bool IsNearPlayer()
	{
		return (Vector3.Distance ( GameObject.FindGameObjectWithTag ("Player").transform.position, transform.position) < ActivationDistance);
	}

	bool canPickUp = true;
	protected virtual void UpdateLogic()
	{


		if (Input.GetKeyDown(KeyCode.F) && nearPlayer && gameManager.CurrentSelectedObject != null && gameManager.CurrentSelectedObject.GetInstanceID() == gameObject.GetInstanceID())
		{
			OnUse();
		}	
	}

	IEnumerator ShowWeapon()
	{
		yield return new WaitForSeconds(2f);
		GameObject.Find("RightWeaponHold").GetComponentInChildren<Renderer>().enabled = true;
		
	}

	protected void PickUP(InventoryItem item, int amount = 1)
	{
		string last = (GameHelper.GetLocalPlayer ().GetComponent<PlayerInventory> ().LastPickedupItem != null) ? GameHelper.GetLocalPlayer ().GetComponent<PlayerInventory> ().LastPickedupItem.ItemName : ""; 
		if (!GameHelper.GetLocalPlayer ().GetComponent<PlayerInventory> ().PickupObject (item, amount))
		{
			GameHelper.ShowNotice("Inventario pieno!");
		}
		else
		{
			GameObject o = Instantiate( Resources.Load("ItemParticleSystem") ) as GameObject;
			o.transform.position = transform.position;
			StartCoroutine(deletePS(o));
	
			Destroy(gameObject);

		}
	}

	void OnDestroy()
	{
		if (!Respawn && !GameHelper.GameIsLoading)
		{
			GetComponent<DoNotRespawnOnLoad>().RememberDestroy();
		}
	}

	IEnumerator deletePS(GameObject ps)
	{
		yield return new WaitForSeconds (0.3f);
		Destroy (ps);
	}

	protected void KneelToPickup()
	{
		Animator a = GameHelper.GetLocalPlayer ().GetComponent<Animator> ();
		a.CrossFade (Animator.StringToHash ("Actions.Grab"), 0.1f);
	}

	protected IEnumerator WaitAndPickUp(InventoryItem item)
	{
		yield return new WaitForSeconds (0.01f);

		GameHelper.GetLocalPlayer().GetComponent<PlayerInventory>().PickupObject(item, 1);
		Destroy (gameObject);
	}

	public virtual void OnUse()
	{
		if (canPickUp)
		{
			canPickUp = false;
		}

		if (CanPickup)
		{
		
			Destroy (gameObject);
			if (PickedUpItem != null)
			{
				photonView.RPC("PickedUp", PhotonTargets.Others, new object[] { PhotonNetwork.player } );
				GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerInventory> ().PickupObject(PickedUpItem);
			}

		}
	}

	// NETWORKING STUFF
	
	protected virtual void SetupForNetwork()
	{
		// Add a PhotonView component
		if (photonView == null)
			gameObject.AddComponent<PhotonView> ();
		// Make it observe this script
		photonView.observed = transform;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

	}

	[RPC]
	void PickedUp(PhotonPlayer byWho)
	{
		Destroy (gameObject);
	}

}
