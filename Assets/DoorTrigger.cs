using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {

	public Door Door;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider c)
	{
		Debug.Log ("Trigger");
		if (c.GetComponent<NavMeshAgent>() != null)
		{
			if (!Door.Opened)
			{
				c.GetComponent<NavMeshAgent>().Stop();
				c.GetComponent<NavMeshAgent>().path.ClearCorners();
			}
		}
	}
}
