using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HairPickerEntry : MonoBehaviour, IPointerClickHandler {

	public GameObject MeshObject;
	public PlayerMeshManager Manager;

	// Use this for initialization
	void Start () {
	
	}

	public void OnPointerClick(PointerEventData data)
	{
		if (MeshObject != null)
			Manager.Swap (MeshObject, ref Manager.Skins.Hair, Color.black);
		else
		{
			Manager.Remove( ref Manager.Skins.Hair );
		}

		CharManager.manager.character.HairPrefab = MeshObject.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
