using UnityEngine;
using System.Collections;
using System.Linq;

public class NetworkManager : MonoBehaviour {
	public GameObject playerPrefab;
	int ownerID = 0;
	string RoomName;
	public static NetworkManager instance;
	// Use this for initialization
	void Awake () {
		instance = this;
		//PhotonNetwork.ConnectUsingSettings ("v1.0");
#if UNITY_EDITOR
		if (PhotonNetwork.connected)
			PhotonNetwork.Disconnect ();
#endif
		PhotonNetwork.offlineMode = true;
		CreateNewRoom ("Offline");
	} 
	
	// Update is called once per frame
	void Update () {
	}
	

	public void CreateNewRoom (string roomName = "NewGame")
	{
		RoomName = roomName;
		PhotonNetwork.CreateRoom (roomName);
	}

	public void JoinRoom ( string roomName = "NewGame")
	{
		RoomName = roomName;
		PhotonNetwork.JoinRoom (roomName);
	}

	public void InstantiatePlayer(Vector3 pos)
	{
		GameObject p = PhotonNetwork.Instantiate (playerPrefab.name, pos, Quaternion.identity, 0) as GameObject;
	}

	void OnPhotonJoinRoomFailed()
	{
		GameHelper.SystemMessage("Multiplayer: Impossibile unirsi alla partita.", Color.red);
	}
	void OnPhotonCreateRoomFailed()
	{
		GameHelper.SystemMessage("Multiplayer: Impossibile aprire la partita.", Color.red);
	}


	void OnJoinedRoom()
	{


		if (PhotonNetwork.playerList.Length <= 1)
			return;
		GameHelper.SystemMessage("Multiplayer: Ti sei unito alla partita " + RoomName, Color.green);
		Destroy (Camera.main.gameObject);
		GameObject p = PhotonNetwork.Instantiate (playerPrefab.name, GameObject.Find("spawn").transform.position, GameObject.Find("spawn").transform.rotation, 0) as GameObject;
	}


	void OnCreatedRoom()
	{
		if (!PhotonNetwork.offlineMode)
		{
			GameHelper.SystemMessage("Multiplayer: La partita " + RoomName + " è ora aperta.", Color.green);
		}
		if (PhotonNetwork.offlineMode || !PhotonNetwork.connected)
		{
			GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, GameObject.Find ("Spawn").transform.position, GameObject.Find ("Spawn").transform.rotation, 0) as GameObject;
		}
		//PhotonNetwork.AllocateViewID ();
		//GameObject.FindGameObjectithTag ("Player").GetComponent<PhotonView> ().viewID = PhotonNetwork.player.ID;
		ownerID = PhotonNetwork.player.ID;
		
		Debug.Log ("Partita avviata. Owner id: " + ownerID + " CurrentID: " + PhotonNetwork.player.ID);
	}

	void OnGUI()
	{
		GUI.Label (new Rect (10, 90, 100, 20), PhotonNetwork.connectionState.ToString ());
		if (PhotonNetwork.room != null)
			GUI.Label (new Rect (10, 140, 100, 20), PhotonNetwork.room.name);
	}

	void OnConnectionFail()
	{
	}
	void OnFailedToConnectToPhoton()
	{
	}
	void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
	}
	[RPC]
	void SetOwner(int id)
	{
		ownerID = id;
	}
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

	}

}
