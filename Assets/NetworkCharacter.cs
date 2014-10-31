using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkCharacter : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	GameObject[] FindGameObjectsWithLayer (int layer) {
		GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		List<GameObject> goList = new List<GameObject>();
		for (int i = 0; i < goArray.Length; i++) {
			if (goArray[i].layer == layer) {
				goList.Add(goArray[i]);
			}
		}
		if (goList.Count == 0) {
			return null;
		}
		return goList.ToArray();
	}

	bool disabledArms = false;
	// Update is called once per frame
	void Update () {
		if (!PhotonNetwork.inRoom)
			return;

		if (!GetComponent<PhotonView>().isMine || PhotonNetwork.playerList[0].ID != PhotonNetwork.player.ID)
		{
			CharacterController c = GetComponent(typeof(CharacterController)) as CharacterController;
			c.enabled = false;
			if (GetComponentInChildren<Camera>() != null)
				Destroy(GetComponentInChildren<Camera>().gameObject);

			if (!disabledArms)
			{
				foreach(GameObject o in FindGameObjectsWithLayer( LayerMask.NameToLayer("Braccia")))
				{
					o.layer = LayerMask.NameToLayer("Default");
					o.tag = "Untagged";
				}
				gameObject.tag = "OnlinePlayer";
				disabledArms = true;
			}
		}

		if (!GetComponent<PhotonView>().isMine)
		{
			transform.position = Vector3.Lerp(transform.position, lastKnownPosition, Time.deltaTime * 5);
			transform.rotation = Quaternion.Slerp( transform.rotation, lastKnownRotation, Time.deltaTime * 5);
		}
		else
		{

		}
	
	}
	Vector3 lastKnownPosition = Vector3.zero;
	Quaternion lastKnownRotation = Quaternion.identity;

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext( (Vector3) transform.position );
			stream.SendNext( (Quaternion) transform.rotation );
		}
		else
		{
			lastKnownPosition = (Vector3)stream.ReceiveNext();
			lastKnownRotation = (Quaternion)stream.ReceiveNext();
		}
	}
}
