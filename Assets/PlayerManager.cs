using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour, ISerializedObject {

	public GameManager gameManager;
	public ClickToMove ctm;
	GameObject lastHitObject;
	string UID;
	Vector3 p;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		UID = GameHelper.GetUIDForObject (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public object[] Serialize()
	{
		object[] value = new object[1];
		value [0] = new PlayerPosition (transform.position);
		return value;
	}

	public void OnLoadingFinished()
	{
		Debug.Log ("Finish loading");
		transform.position = p;
		GetComponent<NavMeshAgent> ().Warp (transform.position + transform.up);
		ctm.enabled = true;
	}

	public void DeSerialize(object[] data)
	{
		transform.position = (data [0] as PlayerPosition).GetVector ();
		p = (data [0] as PlayerPosition).GetVector ();
		GetComponent<ClickToMove> ().enabled = true;
	}

	public string GetUID()
	{
		return UID;
	}
}
