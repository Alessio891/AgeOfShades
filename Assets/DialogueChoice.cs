using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueChoice : MonoBehaviour, IPointerClickHandler {

	public string choiceId;
	public string[] data;

	// Use this for initialization
	void Start () {
	
	}

	public void OnPointerClick(PointerEventData data)
	{
		DialogueManager.instance.OnChoice (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
