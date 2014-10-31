using UnityEngine;
using System.Collections;

public class LookCamera : MonoBehaviour {

	public bool Optimize = true;

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		if (Optimize)
		{
			if (renderer != null)
			{
				renderer.enabled = false;
			}
			else if (GetComponent<Canvas>() != null)
			{
				GetComponent<Canvas>().enabled = false;
			}
			transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);	
			if (renderer != null)
			{
				renderer.enabled = true;
			}
			else if (GetComponent<Canvas>() != null)
			{
				GetComponent<Canvas>().enabled = true;
			}
		}
		transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main != null)
			transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
	}
}
