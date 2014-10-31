using UnityEngine;
using System.Collections;

public class WarpingPoint : MonoBehaviour {
	public string SceneName;
	public Vector3 SpawnPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll)
	{
		GameHelper.LoadNewScene (SceneName, SpawnPosition);
	}	
}
