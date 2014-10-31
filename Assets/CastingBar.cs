using UnityEngine;
using System.Collections;

public class CastingBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 p = transform.position;
		p.y = 2.23f;
		transform.position = p;
		GameHelper.HideMenu (transform.parent.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
