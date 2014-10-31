using UnityEngine;
using System.Collections;

public class SpeechTrigger : MonoBehaviour, ISerializedObject {

	public string Text;
	public GameObject Target;
	public bool DestroyOnCollision = true;
	public string uid;

	// Use this for initialization
	void Start () {
		uid = GameHelper.GetUIDForObject (this);
	}

	public object[] Serialize()
	{
		return null;
	}

	public void DeSerialize(object[] data) {}

	public void OnLoadingFinished()
	{
		if (CharManager.manager.character.DoNotRespawnList.Contains(uid))
			Destroy(gameObject);
	}

	public string GetUID()
	{
		return uid;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		GameHelper.WarningMessage (Text);
		if (DestroyOnCollision)
		{
			if (CharManager.manager.character.DoNotRespawnList == null)
				CharManager.manager.character.DoNotRespawnList = new System.Collections.Generic.List<string>();
			CharManager.manager.character.DoNotRespawnList.Add(uid);
			Destroy(gameObject);
		}
	}

}

public interface ISerializedTrigger
{

}