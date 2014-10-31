using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WarningMessages : MonoBehaviour {

	public GameObject messagePrefab;
	List<string> messageQueue = new List<string>();
	string text;
	float _timer = 0;
	bool shown = false;
	bool fading = false;
	public GameObject message;
	// Use this for initialization
	void Start () {
	
	}
	bool renew = false;
	public void Show(string text)
	{
		this.text = text;
		message.GetComponent<Text> ().CrossFadeAlpha (1, 1, false);
		shown = false;
		renew = true;
		_timer = 0;	
	}
	
	// Update is called once per frame
	void Update () {

		if (!shown && !string.IsNullOrEmpty(text))
		{
			message.GetComponent<Text>().text = text;
			shown = true;
			renew = false;
		}

		if (shown)
		{
			_timer += Time.deltaTime;

			if (_timer >= 4)
			{
				if (renew)
				{
					_timer = 0;
					renew = false;
					return;
				}
				//iTween.ValueTo(message, iTween.Hash("from", 1, "to", 0, "time", 2, "onupdate", "FadeText", "onupdatetarget", gameObject, "oncomplete", "DestroyText", "oncompletetarget", gameObject));
				message.GetComponent<Text>().CrossFadeAlpha(0, 1, false);
				_timer = 0;
				fading = true;
			}
		}


	}

	public void FadeText(float alpha)
	{
		Color c = message.GetComponent<Text> ().color;
		c.a = alpha;
		message.GetComponent<Text> ().color = c;
	}
	public void DestroyText()
	{
		fading = false;
		shown = false;
		renew = false;
		text = "";
	}

}
