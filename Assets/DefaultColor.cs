using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DefaultColor : MonoBehaviour, IPointerClickHandler {

	public Image Color;
	public DefaultColorPicker Picker;

	public void OnPointerClick(PointerEventData data)
	{
		if (Picker.OnSelectedColor != null)
		{
			Picker.SelectedColor = Color.color;
			Picker.OnSelectedColor();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
