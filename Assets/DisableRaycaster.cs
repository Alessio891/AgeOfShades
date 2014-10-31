using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisableRaycaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<Canvas>().enabled && GetComponent<GraphicRaycaster>() != null && GetComponent<GraphicRaycaster>().enabled)
			GetComponent<GraphicRaycaster>().enabled = false;

		if (GetComponent<Canvas>().enabled && GetComponent<GraphicRaycaster>() != null && !GetComponent<GraphicRaycaster>().enabled)
			GetComponent<GraphicRaycaster>().enabled = true;

	}
}
