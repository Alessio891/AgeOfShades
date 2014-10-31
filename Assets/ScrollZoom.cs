using UnityEngine;
using System.Collections;

public class ScrollZoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.CameraFadeAdd ();

	}
	
	// Update is called once per frame
	float value = 60;
	void Update () {
		value += Input.GetAxis ("Mouse ScrollWheel") * -4;
		value = Mathf.Clamp (value, 40, 60);
		camera.fieldOfView = value;

	}
}
