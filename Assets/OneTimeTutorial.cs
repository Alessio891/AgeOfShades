using UnityEngine;
using System.Collections;

public class OneTimeTutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string Text = "";

	void OnTriggerEnter(Collider coll)
	{
		Destroy (gameObject);
	}
}
