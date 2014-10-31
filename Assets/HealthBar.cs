using UnityEngine;
using System.Collections;

public enum ProgressBars
{
	Health = 0,
	Stamina,
	Hunger,
	Mana
}

public class HealthBar : MonoBehaviour {

	public ProgressBars Type = ProgressBars.Health;
	float lastLifeRatio;
	float lastHungreRatio;
	float lastStaminaRatio; 
	bool KeepVisible = false;
	bool showing = false;
	// Use this for initialization
	void Start () {
	}
	public IEnumerator HideStatus()
	{
		yield return new WaitForSeconds (1.5f);
		if (KeepVisible)
		{
			StartCoroutine(HideStatus());
			KeepVisible = false;
			return false;
		}
		showing = false;
	}
	
	// Update is called once per frame
	void Update () {
		EntityStatus status = (EntityStatus)GameHelper.GetPlayerComponent<EntityStatus> ();
		switch (Type)
		{
		case ProgressBars.Health:
			float lifeRatio = status.Life / status.MaxLife;
			UnityEngine.UI.Image bar = GameHelper.GetComponentInChildOf<UnityEngine.UI.Image>(gameObject, "BarForeground") as UnityEngine.UI.Image;
			bar.fillAmount =lifeRatio;// Mathf.Lerp( bar.fillAmount, lifeRatio, 3f * Time.deltaTime);
			bar.Rebuild(UnityEngine.UI.CanvasUpdate.PreRender);
			break;
		case ProgressBars.Hunger:
			float hungerRatio = 1 - GameHelper.GetLocalPlayer ().GetComponent<EntityStatus> ().Hunger;// / GameHelper.GetLocalPlayer ().GetComponent<PlayerStatus> ().MaxLife;
			UnityEngine.UI.Image hungerbar = GameHelper.GetComponentInChildOf<UnityEngine.UI.Image>(gameObject, "BarForeground") as UnityEngine.UI.Image;
			hungerbar.fillAmount = hungerRatio;// Mathf.Lerp( bar.fillAmount, lifeRatio, 3f * Time.deltaTime);

			break;
		case ProgressBars.Stamina:

			float stamRatio = status.Stamina / status.Dexterity;
			UnityEngine.UI.Image stambar = GameHelper.GetComponentInChildOf<UnityEngine.UI.Image>(gameObject, "BarForeground") as UnityEngine.UI.Image;
			stambar.fillAmount = stamRatio;// Mathf.Lerp( bar.fillAmount, lifeRatio, 3f * Time.deltaTime);
			break;
		case ProgressBars.Mana:
			float manaRatio = status.Mana / status.Intellect;
			UnityEngine.UI.Image manabar = GameHelper.GetComponentInChildOf<UnityEngine.UI.Image>(gameObject, "BarForeground") as UnityEngine.UI.Image;
			manabar.fillAmount = manaRatio;// Mathf.Lerp( bar.fillAmount, lifeRatio, 3f * Time.deltaTime);
			manabar.Rebuild(UnityEngine.UI.CanvasUpdate.PreRender);
			break;
		}
	}
}
