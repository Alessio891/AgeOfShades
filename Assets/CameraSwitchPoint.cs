using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class CameraSwitchPoint : MonoBehaviour {

	public Vector3 Direction;
	public float speed = 0.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerManager>() != null)
		{
			Camera.main.GetComponent<SmotthFollow>().SetFacingDirection(Direction, speed);
		}

	}

	void OnDrawGizmosSelected()
	{
		Vector3 targetPos = transform.position + Direction * 3;
		Gizmos.color = Color.red;
		Gizmos.DrawLine (transform.position, targetPos);
	}
}
