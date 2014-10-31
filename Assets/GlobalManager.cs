using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalManager : MonoBehaviour {

	public static GlobalManager instance;

	void Awake()
	{ 
		instance = this; 
		DontDestroyOnLoad (gameObject);

	}

	public List<GameObject> ManagerPrefabs;
	[HideInInspector]
	public bool spawnedPrefab;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SpawnPrefabs()
	{
		if (spawnedPrefab)
			return;
		foreach(GameObject o in ManagerPrefabs)
		{
			GameObject ob = Instantiate(o) as GameObject;
			ob.name = ob.name.Replace("(Clone)", "");
		}
		spawnedPrefab = true;
	}
}
