using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Diagnostics;
public static class ItemDatabase
{

	public static Dictionary<Type, InventoryItem> Items = new Dictionary<Type, InventoryItem>();

	public static System.Type[] GetAllSubTypes(System.Type aBaseClass)
	{
		var result = new System.Collections.Generic.List<System.Type>();
		System.Reflection.Assembly[] AS = System.AppDomain.CurrentDomain.GetAssemblies();
		foreach (var A in AS)
		{
			System.Type[] types = A.GetTypes();
			foreach (var T in types)
			{
				if (T.IsSubclassOf(aBaseClass))
					result.Add(T);
			}
		}
		return result.ToArray();
	}

	public static void Init()
	{
		Items.Clear ();
		Type[] types = GetAllSubTypes (typeof(InventoryItem));
		foreach(Type t in types)
		{
			if (t.IsSubclassOf(typeof(InventoryItem)))
			{
				Items[ t ] = (InventoryItem)Activator.CreateInstance(t);
			}
		}

	}

}

public static class CraftingDatabase
{
	public static Dictionary<Type, BasicCraftable> Recipes = new Dictionary<Type, BasicCraftable>();

	public static void Init()
	{
		Type[] types = Assembly.GetExecutingAssembly ().GetTypes ();
		foreach(Type t in types)
		{
			if (t.IsSubclassOf(typeof(BasicCraftable)))
			{
				Recipes[t] = (BasicCraftable)Activator.CreateInstance(t);
			}
		}
	}
}

