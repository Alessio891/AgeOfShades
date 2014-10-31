using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class ColorPicker : MonoBehaviour {

	public Slider Red;
	public Slider Green;
	public Slider Blue;

	public Action OnChange;

	float r_lastValue;
	float g_lastValue;
	float b_lastValue;

	float r_value;
	float g_value;
	float b_value;
	// Use this for initialization
	void Start () {
	
	}

	void GetValues()
	{
		r_value = Red.value;
		g_value = Green.value;
		b_value = Blue.value;
	}

	bool valueChanged { get { return (r_lastValue != r_value) || (g_lastValue != g_value) || (b_lastValue != b_value); } }

	// Update is called once per frame
	void Update () {
		GetValues ();

		if (valueChanged && OnChange != null)
		{
			OnChange();
		}
	}
}
