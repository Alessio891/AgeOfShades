using UnityEngine;
using System.Collections;
[System.Serializable]
public class SimpleShirt : BaseArmor {

	protected override void Start ()
	{
		Prefab = Resources.Load ("SimpleShirt") as GameObject;
		ItemName = "Maglietta";
		ArmorSlot = ArmorSlots.Shirt;
		ItemIcon = "Shirt";
		ItemIconAtlas = "";
		Tint = new SerializedColor (new Color (0.4f, 0.1f, 0.8f));
	}

	public override void ApplyStats ()
	{
		base.ApplyStats ();
		Light l = GameHelper.GetLocalPlayer ().AddComponent<Light> ();
		l.range = 3;
		l.color = Color.green;
		l.type = LightType.Point;
		l.intensity = 2;
		PlayerMeshManager m = GameHelper.GetPlayerComponent<PlayerMeshManager> ();
		m.Remove (ref m.Skins.Arms);
	}

	public override void RemoveStats ()
	{
		base.RemoveStats ();
		GameObject.Destroy (GameHelper.GetLocalPlayer ().GetComponent<Light> ());
	}
}
