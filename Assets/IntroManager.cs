using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {

	public TextAsset IntroText;

	// Use this for initialization
	void Start () {
	//	GameHelper.ShowMenu (DialogueManager.instance.gameObject);		
	//	DialogueManager.instance.ShowDialogue ("DialogoTest.txt", "Root", true);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate()
	{

	}
}
