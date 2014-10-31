using UnityEngine;
using System.Collections;

public class SimpleTrigger : MonoBehaviour {
	public GameObject Target;
	public string Function;
	public bool Once = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			if (Target != null && !string.IsNullOrEmpty(Function))
			{
				Target.SendMessage(Function);
			}
		}
	}
}
