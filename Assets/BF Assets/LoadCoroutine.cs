using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadCoroutine : MonoBehaviour {
	public string SceneName;
	public bool DoneLoading = false;
	public bool IsLoading = false;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Load Scene
	// Load objects state
	// Start Scene

	public void StartLoading()
	{

		StartCoroutine (_load());
	}

	IEnumerator _load()
	{
		DoneLoading = false;
		//SceneName = CharManager.manager.character.SceneName;
		if (CharManager.manager.character.JustCreated)
		{
			Application.LoadLevel ("Cave_terrain");
		}
		else
			Application.LoadLevel (SceneName);
		Debug.Log ("Scene loaded!");

		Debug.Log (SceneName + " loaded. Deserializing objects.");
		IsLoading = true;

		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame ();

		float t = Time.time;
		foreach(MonoBehaviour obj in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
		{
			if (obj is ISerializedObject)
			{
			Debug.Log("Deserializing object: UID = " + (obj as ISerializedObject).GetUID());
				
				// You are so fucking stupid sometimes. DO NOT RETURN NULL IN SERIALIZING CALLS.
				if (GameHelper.GetDataManager().currentGame.Data.ContainsKey( (obj as ISerializedObject).GetUID()))
				{
					(obj as ISerializedObject).DeSerialize( GameHelper.GetDataManager().currentGame.Data[ (obj as ISerializedObject).GetUID() ] );
				}
			}
		}
		foreach(MonoBehaviour obj in GameObject.FindObjectsOfType(typeof(MonoBehaviour)))
		{
			if (obj is ISerializedObject)
			{
				if (GameHelper.GetDataManager().currentGame.Data.ContainsKey( (obj as ISerializedObject).GetUID()))
				{
					(obj as ISerializedObject).OnLoadingFinished();
				}
			}
			if (obj is ILoadingFinishedHandler)
			{
				(obj as ILoadingFinishedHandler).OnLoadingFinished();
			}
		}
		DoneLoading = true;
		IsLoading = false;
		yield return new WaitForSeconds (0.2f);
		CharManager.manager.character.JustCreated = false;
		Destroy (gameObject);
		yield return true;
	}
}
