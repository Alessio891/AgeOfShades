using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReagentListItem : MonoBehaviour {

	public bool Needed = false;

	// Use this for initialization
	void Start () {
	
	}
		
	public void NotNeeded()
	{
		GetComponent<Text>().color = new Color(0.45f, 0.45f, 0.45f, 1);		
	}
	public void Need()
	{
		GetComponent<Text> ().color = Color.white;
	}

	// Update is called once per frame
	void Update () {

	}
}
