using UnityEngine;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour {
	BasicEntity owner;
	// Use this for initialization
	void Start () {
		owner = GetComponentInParent<BasicEntity> ();
		Vector3 lp = transform.localPosition;
		lp.y = 1.05f;
		transform.localPosition = lp;
	}
	
	// Update is called once per frame
	void Update () {
		if (owner.Status != null)
		{
			float ratio = owner.Status.Life / owner.Status.MaxLife;
			foreach(UnityEngine.UI.Image image in GetComponentsInChildren<UnityEngine.UI.Image>())
			{
				if (image.name == "Health")
				{
					image.fillAmount = ratio;
				}
			}
		}
	}
}
