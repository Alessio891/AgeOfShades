using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SystemMessages : MonoBehaviour {

	public float MaximumY = 278;
	public float MinimumY = 26;
	public float textHeight = 30;

	List<GameObject> textsOnScreen = new List<GameObject>();
	public GameObject TextPrefab;
	public bool Visible { get { return GetComponentInChildren<Graphic> ().enabled; } }

	int spotsCount { get { return Mathf.RoundToInt((MaximumY - MinimumY) / textHeight); } }
	int firstFreeSpot { 
		get {
			for(int i = 0; i < spotsCount; i++)
			{
				if(spots[i])
				{
					return i;
				}
			}
			return -1;
		}
	}
	bool[] spots;
	// Use this for initialization
	void Start () {
		// TRUE - Available
		// False - Not Available
		spots = new bool[spotsCount];
		for(int i = 0; i < spotsCount; i++)
		{
			spots[i] = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// This should be called BEFORE adding to the list
	Vector3 GetValidPosition(int index)
	{
		Vector3 value = Vector3.zero;

		value.x = 161;
		value.y = MinimumY + (index * textHeight);

		return value;
	}

	public void Write(string text, Color color, bool outline = true)
	{
		int index = firstFreeSpot;
		if (firstFreeSpot == -1)
			return;
		GameObject newText = Instantiate (TextPrefab) as GameObject;
		Text t = newText.GetComponent<Text> ();
		t.text = text;

		t.color = color;
		t.GetComponent<RectTransform>().localPosition = GetValidPosition (index);
		
		t.transform.SetParent (gameObject.transform, false);
		t.GetComponent<RectTransform>().localScale = Vector3.one;

		if (!outline)
		{
			Destroy(newText.GetComponent<Outline>());
		}
		spots [index] = false;
		textsOnScreen.Add (newText);
		StartCoroutine (fadeText (newText,index));
		
	}

	IEnumerator fadeText(GameObject text, int spotIndex)
	{
		yield return new WaitForSeconds (1.5f);
		while(text.GetComponent<Text>().color.a > 0)
		{
			Color c = text.GetComponent<Text>().color;
			text.GetComponent<Text>().color = new Color(c.r, c.g, c.b, c.a - 0.05f);
			yield return new WaitForSeconds(0.1f);
		}
		for(int i = 0; i < textsOnScreen.Count; i++)
		{
			if (textsOnScreen[i].GetInstanceID() == text.GetInstanceID())
			{
				textsOnScreen.RemoveAt(i);
				break;
			}
		}
		spots [spotIndex] = true;
		Destroy (text);
	}
}
