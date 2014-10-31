using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcWaypoint : MonoBehaviour {

	public List<GameObject> ConnectsTo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#if UNITY_EDITOR_WIN
	void OnDrawGizmos()
	{
		if (UnityEditor.Selection.activeGameObject == gameObject)
		{
			Gizmos.color = Color.gray;				
			Gizmos.DrawWireCube(transform.position, Vector3.one * 2);
			foreach(GameObject o in ConnectsTo)
			{
				Gizmos.color = Color.green;				
				Gizmos.DrawWireCube(o.transform.position, Vector3.one);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(transform.position, o.transform.position);
			}
		}
	}
#endif
}
