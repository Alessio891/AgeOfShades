using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text> ().CrossFadeAlpha (0, 8.5f, false);
		StartCoroutine (startGame ());	
	}

	IEnumerator startGame()
	{
		yield return new WaitForSeconds(8.5f);
		Application.LoadLevel ("Cave_terrain");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
		{
			StopCoroutine("startGame");
			Application.LoadLevel("Cave_terrain");
		}
	}
}
