using UnityEngine;
using System.Collections;

public class HideOnPlayerPassage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (Vector3.Distance (transform.position, GameHelper.GetLocalPlayer ().transform.position));
		if (PlayerNearby())
		{
			if (renderer.enabled)
			{
				renderer.enabled = false;
			}
		}
		else
		{
			if (!renderer.enabled)
			{
				renderer.enabled = true;
			}
		}
	}

	public float Distance = 2;

	bool PlayerNearby()
	{
		return (Vector3.Distance (transform.position, GameHelper.GetLocalPlayer ().transform.position) < Distance);
	}
}
