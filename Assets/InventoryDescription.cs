using UnityEngine;
using System.Collections;

public class InventoryDescription : MonoBehaviour {
	bool Visible = false;
	public GameObject Selected;
	public GameObject EquipButton;
	public GameObject StatsButton;
	public GameObject DropButton;
	// Use this for initialization
	void Start () {
		Hide ();
	}
	
	// Update is called once per fram

	bool hide = true;
	public bool IsOveringMenu = false;
	public void MouseOver()
	{
		IsOveringMenu = true;
	}
	public void MouseExit()
	{
		IsOveringMenu = false;
	}

	public void Show(string text = "")
	{
/*		if (StatsButton.GetComponentInChildren<UnityEngine.UI.Text>().text == "Indietro")
		{
			GetComponent<Animator>().Play(Animator.StringToHash("Base Layer.StatsHideAnimation"));
			StatsButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Stats";
		}*/
		GetComponent<Canvas> ().enabled = true;
		GetComponentInChildren<UnityEngine.UI.Text> ().text = text;

		Visible = true;
		//transform.position = Input.mousePosition + new Vector3 (16, -16, 0);
	//	GetComponent<UIFollowMouse> ().Enabled = true;
	}

	IEnumerator _hide()
	{
		yield return new WaitForSeconds (1.2f);
		if (!hide)
		{
			hide = true;
			return false;
		}
	}
	public void Use()
	{
		if (Selected != null)
		{
			Selected.GetComponent<InventorySlot>().Item.OnUse ();
		}
	}

	public void ShowStats()
	{
		if (Selected == null)
			return;
		if (StatsButton.GetComponentInChildren<UnityEngine.UI.Text>().text == "Stats")
		{
			GetComponent<Animator> ().Play (Animator.StringToHash ("Base Layer.StatsMenuAnimation"));
			foreach(CanvasRenderer canvas in GetComponentsInChildren<CanvasRenderer>())
			{
				if (canvas.name == "EquipButton" || canvas.name == "DropButton" || canvas.transform.parent.name == "EquipButton" || canvas.transform.parent.name == "DropButton")
					canvas.SetAlpha(0);
			}
			foreach(UnityEngine.UI.Button button in GetComponentsInChildren<UnityEngine.UI.Button>())
			{
				if (button.name == "EquipButton" || button.name == "DropButton")
				{
					button.interactable = false;
				}
			}
			GameObject.Find ("StatText").GetComponent<UnityEngine.UI.Text> ().enabled = true;
			GameObject.Find ("StatText").GetComponent<UnityEngine.UI.Text> ().text = "Danno: " + Selected.GetComponent<InventorySlot> ().Item.AttackDamage.ToString () + "\n" +
							"Raggio: " + Selected.GetComponent<InventorySlot> ().Item.AttackRange.ToString () + "\n" +
							"Delay: " + Selected.GetComponent<InventorySlot> ().Item.AttackDelay.ToString () + "\n" +
							"Durata: 15/20 \n" +
					"Mano preferita: Destra";
			StatsButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Indietro";
		}
		else
		{
			GetComponent<Animator> ().Play (Animator.StringToHash ("Base Layer.StatsHideAnimation"));
			foreach(CanvasRenderer canvas in GetComponentsInChildren<CanvasRenderer>())
			{
				//if (canvas.name == "EquipButton" || canvas.name == "DropButton" || canvas.transform.parent.name == "EquipButton" || canvas.transform.parent.name == "DropButton")
					canvas.SetAlpha(1);
			}
			foreach(UnityEngine.UI.Button button in GetComponentsInChildren<UnityEngine.UI.Button>())
			{
				if (button.name == "EquipButton" || button.name == "DropButton")
				{
					button.interactable = true;
				}
			}
			GameObject.Find ("StatText").GetComponent<UnityEngine.UI.Text> ().enabled = false;

			StatsButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Stats";

		}
																			   
	}

	public void HideStats()
	{

	}

	public void Hide()
	{



		Visible = false;
		GetComponent<Canvas> ().enabled = false;
	}
}
