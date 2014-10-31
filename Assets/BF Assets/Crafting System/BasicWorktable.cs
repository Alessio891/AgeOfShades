using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class BasicWorktable : MonoBehaviour, ISelectable {

	public List<BasicCraftable> CraftableHere = new List<BasicCraftable>();
	public GameObject CraftingMenu;
	public GameObject Player;

	public GameObject Marker;

	bool nearPlayer = false;

	// Use this for initialization
	void Start () {
		CraftingMenu = GameObject.Find ("CraftingMenu");
		Player = GameObject.FindGameObjectWithTag ("Player");
		Init ();
	}

	/// <summary>
	/// Check if this item is already selected. If it is, go to its position otherwise enable the marker.
	/// </summary>
	public virtual void OnSelect()
	{
		if (Marker == null)
		{
			Marker = Instantiate( Resources.Load("ObjectMarker") ) as GameObject;
			Marker.transform.position = transform.position + new Vector3(0, 0.2f, 0);
			Marker.transform.localScale *= 1.3f;
		}
		else
		{
			NavMeshAgent agent = (GameHelper.GetPlayerComponent<NavMeshAgent>() as NavMeshAgent);
			ClickToMove cm = GameHelper.GetPlayerComponent<ClickToMove>() as ClickToMove;
			if (Vector3.Distance(transform.position, agent.transform.position) < cm.RunDistance)
			{
				agent.speed = cm.WalkSpeed;
			}
			else
				agent.speed = cm.RunSpeed;
			(GameHelper.GetPlayerComponent<NavMeshAgent>() as NavMeshAgent).SetDestination(transform.position + transform.forward);
			StartCoroutine(waitForArrivalAndOpenMenu());
		}
	}

	IEnumerator waitForArrivalAndOpenMenu()
	{
		NavMeshAgent agent = (NavMeshAgent)GameHelper.GetPlayerComponent<NavMeshAgent> ();
		while(Vector3.Distance(agent.transform.position, transform.position) > 1)
		{
			yield return new WaitForSeconds(0.1f);
		}
		GameHelper.ShowMenu (CraftingMenu);
	}

	public virtual void OnDeselect()
	{
	}

	protected virtual void Init()
	{
	}
	
	protected bool CanCraftHere(string name)
	{
		foreach(BasicCraftable c in CraftableHere)
		{
			if (c.ItemName == name)
				return true;
		}
		return false;
	}

	public void AddToCraftingList()
	{
		EntitySkills skills = (EntitySkills)GameHelper.GetPlayerComponent<EntitySkills>();
		PlayerInventory inventory = (PlayerInventory)GameHelper.GetPlayerComponent<PlayerInventory> ();
		foreach(BasicCraftable c in CraftableHere)
		{
			bool skip = false;
			foreach(KeyValuePair<string, float> req in c.RequiredSkills)
			{
				// If we have less thatn the required skill value
				if ( skills.Skills[req.Key].Value < req.Value )
				{
					// Let the code know we have to skip this.
					skip = true;
					break;
				}
			}
			// But wait! If the item doesn't require a skill to be crafted, then don't skip it
			if (c.RequiredSkills.Count <= 0)
				skip = false;
			// If it turns out we have to skip it... well skip it.
			if (skip)
				break;

			// Do we have the requried materials for this item?
			if ( inventory.HasMaterials(c))
			{
				// If this item isn't already in the list somehow
				if ( (from i in CraftingMenu.GetComponent<CraftingMenu>().CraftingList where i.ItemName == c.ItemName select i).Count() <= 0)
				{
					// Add it to the crafting list
					CraftingMenu.GetComponent<CraftingMenu>().CraftingList.Add(c);
				}
			}
		}
	}
	float _timer = 0;
	protected virtual void UpdateLogic()
	{
		if (IsNearPlayer())
		{
			if (!nearPlayer)
			{
				nearPlayer = true;
				AddToCraftingList();
				//CraftingGrid.GetComponent<CraftingGrid>().CraftingList.AddRange( CraftableHere );
				CraftingMenu.GetComponent<CraftingMenu>().UpdateNow = true;
			}
	
		}
		else
		{
			
			if (nearPlayer)
			{
				nearPlayer = false;
				CraftingMenu.GetComponent<CraftingMenu>().RemoveCraftables(CraftableHere);
				CraftingMenu.GetComponent<CraftingMenu>().UpdateNow = true;
				
			}
		}
	}

	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Marker != null)
			{
				Destroy(Marker);
			}
		}

		UpdateLogic ();


	}

	protected bool IsNearPlayer()
	{
		
		return (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 5);
	}
}
