using UnityEngine;
using System.Collections;

public class DoNotRespawnOnLoad : MonoBehaviour, ISerializedObject {

	public string uid;
	
	// Use this for initialization
	void Start () {
		uid = GameHelper.GetUIDForObject (this);
	}
	
	public object[] Serialize()
	{
		object[] data = new object[1];
		data [0] = "RUBBISHRUBBISH_CHTULUISTHEONLYGOD_PRAISEHIM";
		return data;
	}
	
	public void DeSerialize(object[] data) {
		Debug.Log ("Checking if " + name + " isn't supposed to respawn.");
		if (CharManager.manager.character.DoNotRespawnList.Contains(uid))
		{
			Debug.Log("Hey! It isn't!");
			Destroy(gameObject);
		}
		else
		{
			Debug.Log("Ok, it should respawn. Nevemind.");
		}
	}
	
	public void OnLoadingFinished()
	{
	
	}

	public void RememberDestroy()
	{
		Debug.Log ("Destroying " + name);
		if (CharManager.manager.character.DoNotRespawnList == null)
			CharManager.manager.character.DoNotRespawnList = new System.Collections.Generic.List<string>();

		if (!CharManager.manager.character.DoNotRespawnList.Contains(uid))
		{
			CharManager.manager.character.DoNotRespawnList.Add(uid);
		}
	}

	public string GetUID()
	{
		return uid;
	}
}
