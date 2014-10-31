using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BasicCraftable {

	public List<InventoryItem> Ingredients = new List<InventoryItem>();
	public List<int> IngredientAmounts = new List<int> ();
	public BasicWorktable RequiredWorktable;

	public List<InventoryItem> CraftingResult = new List<InventoryItem> ();
	public List<int> CraftingResultAmount = new List<int>();

	public Dictionary<string, float> RequiredSkills = new Dictionary<string, float> ();

	public string ItemIcon;
	public string ItemName = "Default";
	public string ItemIconAtlas = "ItemIcons";

	public string CraftedNear = "Worktable";
	public string ToolNeeded = "None";

	public CraftingTabs Tab = CraftingTabs.Equip;

	public BasicCraftable()
	{
		CraftedNear = "Worktable";
		Init ();
	}

	protected void AddIngredient( InventoryItem ingredient, int amount = 1)
	{
		Ingredients.Add (ingredient);
		IngredientAmounts.Add (amount);
	}

	protected void AddResult( InventoryItem result, int amount = 1)
	{
		CraftingResult.Add (result);
		CraftingResultAmount.Add (amount);
	}

	protected virtual void Init()
	{
	}

	protected void AddRequirement(string skill, float amount)
	{
		RequiredSkills [skill] = amount;
	}
}

public struct Ingredient
{
	public InventoryItem Item;
	public int Amount;
}

public static class CraftingHelper {

	public static Dictionary<string, BasicCraftable> CraftingItems;

	public static void Initialize()
	{
	}

	public static Ingredient[] GetIngredientsFor(BasicCraftable item)
	{
		List<Ingredient> result = new List<Ingredient> ();
		int y = 0;
		foreach(InventoryItem i in item.Ingredients)
		{
			Ingredient ing = new Ingredient();
			ing.Item = i;
			ing.Amount = item.IngredientAmounts[y];
			result.Add(ing);
			y++;
		}
		return result.ToArray ();
	}

}
