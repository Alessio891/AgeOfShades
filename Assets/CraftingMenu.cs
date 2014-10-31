using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingMenu : MonoBehaviour {

	public GameObject SlotPrefab;
	public GameObject ResulCraftingIcon;
	public GameObject SelectedTab;
	public BasicCraftable SelectedCraftable;

	public GameObject CraftingGrid;
	public GameObject RightPage;
	public GameObject Info;
	public GameObject NoSelection;

	public List<BasicCraftable> CraftingList = new List<BasicCraftable> ();
	public List<GameObject> SlotObjects = new List<GameObject>();
	public bool UpdateNow = false;
	public int SlotsPerRow = 5;

	CraftingTabs craftingTab = CraftingTabs.Equip;

	int lastSlotCount = 0;
	// Use this for initialization
	void Start () {
		//SelectedTab = EventSystemManager.currentSystem.currentSelectedObject;
		//SelectedTab.GetComponent<Image>().color = Color.green;
		//SelectedTab.GetComponent<RectTransform>().localScale *= 1.1f;
	}

	Vector3 GetAvailablePosition(int count)
	{
		Vector3 value = Vector3.zero;
		Vector2 gridCoords = IndexToCoordinates (count);

		value = new Vector3( -100 + (50 * gridCoords.x), 173 - (50 * gridCoords.y), 0);
		

		return value;
	}

	Vector2 IndexToCoordinates(int index)
	{
		Vector2 coordinates = Vector2.zero;
		int slotY = Mathf.RoundToInt(index / SlotsPerRow);
		// slot - ( (maxSlotsPerRow * slotY) + slotY )
		int slotX = index % SlotsPerRow;
		coordinates.x = (float)slotX;
		coordinates.y = (float)slotY;
		return coordinates;
	}

	
	// Update is called once per frame
	void LateUpdate () {

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Visible)
			{
				GameHelper.HideMenu(this.gameObject);
				return;
			}
		}

		if (lastSlotCount != CraftingList.Count || UpdateNow)
		{
			UpdateNow = false;
			foreach(GameObject o in SlotObjects)
				Destroy(o);
			SlotObjects.Clear();

			int i = 0;
			PlayerInventory pi = (PlayerInventory)GameHelper.GetPlayerComponent<PlayerInventory>();
			bool selectedPresent = false;
			foreach(BasicCraftable c in CraftingList)
			{
				if (c.Tab != craftingTab)
					continue;
				if (!pi.HasMaterials(c))
					continue;
				if (currentSelection != null && c.ItemName == currentSelection.ItemName)
					selectedPresent = true;
				GameObject slot = Instantiate(SlotPrefab) as GameObject;
				slot.transform.SetParent(CraftingGrid.transform, false);
				slot.transform.localPosition = GetAvailablePosition(i);
				
				slot.GetComponent<CraftingListItemButton>().Item = c;
				SlotObjects.Add(slot);
				i++;
			}
			if (!selectedPresent && currentSelection != null)
			{
				currentSelection = null;
			}
		}

		if (currentSelection != null && !Info.GetComponent<Canvas>().enabled)
		{
			Info.GetComponent<Canvas>().enabled = true;
			NoSelection.GetComponent<Canvas>().enabled = false;
		}
		else if (currentSelection == null && !NoSelection.GetComponent<Canvas>().enabled)
		{
			Info.GetComponent<Canvas>().enabled = false;
			NoSelection.GetComponent<Canvas>().enabled = true;
		}

		lastSlotCount = CraftingList.Count;
	}

	void UpdateCraftingList(string Tab)
	{

	}

	public void SelectTab(int tab)
	{
		craftingTab = (CraftingTabs)tab;
		if (SelectedTab != null)
		{
			SelectedTab.GetComponent<RectTransform>().localScale /= 1.1f;
			SelectedTab.GetComponent<Image>().color = Color.white;
		}

		if (EventSystemManager.currentSystem.currentSelectedObject != null)
		{
			SelectedTab = EventSystemManager.currentSystem.currentSelectedObject;
			SelectedTab.GetComponent<Image>().color = Color.green;
			SelectedTab.GetComponent<RectTransform>().localScale *= 1.1f;
		}
		UpdateNow = true;
	}

	public void _UpdateNow()
	{
		UpdateNow = false;
		foreach(GameObject o in SlotObjects)
			Destroy(o);
		SlotObjects.Clear();
		
		int i = 0;
		PlayerInventory pi = (PlayerInventory)GameHelper.GetPlayerComponent<PlayerInventory>();
		foreach(BasicCraftable c in CraftingList)
		{
			if (c.Tab != craftingTab)
				continue;
			if (!pi.HasMaterials(c))
				continue;
			GameObject slot = Instantiate(SlotPrefab) as GameObject;
			slot.transform.SetParent(CraftingGrid.transform, false);
			slot.transform.localPosition = GetAvailablePosition(i);
			
			slot.GetComponent<CraftingListItemButton>().Item = c;
			SlotObjects.Add(slot);
			i++;
		}
		
	}
	
	
	public void AttemptCrafting()
	{
		Debug.Log ("Attemp craft " + currentSelection.ItemName);
		
		EntitySkills skills = GameHelper.GetPlayerComponent<EntitySkills> () as EntitySkills;
		PlayerInventory inventory = GameHelper.GetPlayerComponent<PlayerInventory> () as PlayerInventory;
		bool success = true;
		foreach(KeyValuePair<string, float> pair in currentSelection.RequiredSkills)
		{
			if (!skills.SkillCheckSuccessful(pair.Key, pair.Value, 0.5f))
			{
				success = false;
			}
		}
		for(int i = 0; i < currentSelection.Ingredients.Count; i++)
		{
			if (!inventory.Has(currentSelection.Ingredients[i], currentSelection.IngredientAmounts[i]))
				success = false;
		}

		if (success)
		{

			ResulCraftingIcon.GetComponent<Canvas> ().enabled = true;
			ResulCraftingIcon.GetComponent<Animator> ().Play (Animator.StringToHash ("Base Layer.CraftSuccessAnim"));
			for(int i = 0; i < currentSelection.Ingredients.Count; i++)
			{
				inventory.ConsumeObject(currentSelection.Ingredients[i], currentSelection.IngredientAmounts[i]);
			}
			for(int j = 0; j < currentSelection.CraftingResult.Count; j++)
			{
				inventory.PickupObject(currentSelection.CraftingResult[j], currentSelection.CraftingResultAmount[j]);
			}
			Debug.Log("UPDATE UPDATE UPDATE!");
			UpdateNow = true;

			
		}
		else
		{
			ResulCraftingIcon.GetComponent<Canvas> ().enabled = true;
			ResulCraftingIcon.GetComponent<Animator> ().Play (Animator.StringToHash ("Base Layer.CraftFailAnim"));
		}
		StartCoroutine (waitAndHideResult (1));
		
	}
	IEnumerator waitAndHideResult(float wait)
	{
		yield return new WaitForSeconds (wait);
		ResulCraftingIcon.GetComponent<Canvas> ().enabled = false;
	}

	BasicCraftable currentSelection;
	public void ShowInfo(GameObject slot)
	{

		BasicCraftable item = slot.GetComponent<CraftingListItemButton> ().Item;
		currentSelection = item;
		(GameHelper.GetComponentInChildOf<Text> (Info, "ItemName") as Text).text = item.ItemName;
		(GameHelper.GetComponentInChildOf<Text> (Info, "ItemType") as Text).text = item.CraftingResult [0].Type.ToString ();
		(GameHelper.GetComponentInChildOf<Text> (Info, "ItemDescription") as Text).text = item.CraftingResult [0].ItemDescription;
		if (item.ToolNeeded != "None")
			(GameHelper.GetComponentInChildOf<Text> (Info, "RequiredTool") as Text).text = "Richiede un " + item.ToolNeeded + "!";
		else
			(GameHelper.GetComponentInChildOf<Text> (Info, "RequiredTool") as Text).text = "Questo oggetto non richiede un attrezzo.";
		string path = "Icons/" + item.ItemIcon;
		Sprite s = (Sprite)Resources.Load<Sprite>(path);
		(GameHelper.GetComponentInChildOf<Image> (Info, "ItemIcon") as Image).sprite = s;
	}

	public void RemoveCraftables(List<BasicCraftable> crftbls)
	{
		List<BasicCraftable> craftables = new List<BasicCraftable> (crftbls);
		for(int i = 0; i < CraftingList.Count; i++)
		{
			if (craftables.Count <= 0)
				break;

			for(int j = 0; j < craftables.Count; j++)
			{
				if (craftables[j].ItemName == CraftingList[i].ItemName)
				{
					craftables.RemoveAt(j);
					CraftingList.RemoveAt(i);
					i--;
					break;
				}
			}
		}
	}

	public bool Visible { get { return GetComponentInChildren<Graphic> ().enabled; } }
	
}

public enum CraftingTabs
{
	Equip = 0,
	Spell,
	Potions
}
