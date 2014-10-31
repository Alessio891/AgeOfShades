using UnityEngine;
using System.Collections;
using System;

public class CutSceneTrigger : MonoBehaviour {
	public string cutSceneName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			CutSceneManager Manager = GameObject.Find("CutSceneManager").GetComponent<CutSceneManager>();
			Manager.CutsceneCamera.transform.position = Camera.main.transform.position;
			Manager.CutsceneCamera.transform.rotation = Camera.main.transform.rotation;
			CutScene scene;
			try
			{
				scene = (CutScene)Activator.CreateInstance(Type.GetType(cutSceneName));
			} catch (InvalidCastException e)
			{
				Debug.LogError("CutScene Collider " + name + " non è riuscito a trovare la scena " + cutSceneName);
				return;
			}
			GameObject.Find("CutSceneManager").GetComponent<CutSceneManager>().SwitchToCutscene( () => {
				GameObject.Find("CutSceneManager").GetComponent<CutSceneManager>().PlayCutscene(scene); 
			});
			Destroy(gameObject);
		}
	}
}


public class BossCutscene : CutScene
{
	public BossCutscene() : base(){}

	public override void Start ()
	{
		DisablePlayer ();

		iTween.MoveTo (Manager.CutsceneCamera, iTween.Hash ("position", GameObject.Find("CameraFlypos2").transform.position, "time", 7, "orienttopath", false));
	}

	public override void Update ()
	{
		if (Vector3.Distance(Manager.CutsceneCamera.transform.position, GameObject.Find("CameraFlypos2").transform.position) < 0.1f)
		{
			Manager.SwitchToPlayerCamera( () => { EnablePlayer(); } );
			Manager.currentCutscene = null;
			GameObject.Find ("ZombieBoss").GetComponent<BasicEntity> ().enabled = true;
			GameHelper.ShowNotice("Cer...ve...lli...", GameObject.Find("ZombieBoss"));
		}
	}

	public override void OnSkip ()
	{
		Manager.SwitchToPlayerCamera( () => { EnablePlayer(); } );
		Manager.currentCutscene = null;
		GameObject.Find ("ZombieBoss").GetComponent<BasicEntity> ().enabled = true;
		GameHelper.ShowNotice("Cer...ve...lli...", GameObject.Find("ZombieBoss"));
	}
}