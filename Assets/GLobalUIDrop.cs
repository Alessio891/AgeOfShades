using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GLobalUIDrop : MonoBehaviour, IDropHandler {

	// Use this for initialization
	void Start () {
	
	}

	public void OnDrop(PointerEventData data)
	{
		if (data.pointerDrag != null)
			Destroy (data.pointerDrag);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
