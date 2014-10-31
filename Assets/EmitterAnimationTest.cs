using UnityEngine;
using System.Collections;

public class EmitterAnimationTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveAdd(gameObject, iTween.Hash("amount", new Vector3(1, 0, 0), "time", 1, "loopType", "loop"));
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
