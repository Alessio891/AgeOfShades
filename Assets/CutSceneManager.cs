using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public class CutSceneManager : MonoBehaviour {
	public GameObject CutsceneCamera;
	public GameObject PlayerCamera;
	public CutScene currentCutscene;
	private System.Action fadeFinish;

	bool inCutsceneMode { get { return (CutsceneCamera.camera.enabled); } }

	public static CutSceneManager instance { get { return _instance; } }
	static CutSceneManager _instance;

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		if (GameObject.FindObjectOfType(typeof(LoadCoroutine)) != null)
		{
			if (GameHelper.GetDataManager().currentGame.firstLoad)
			{
			}
			else
			{
				CutsceneCamera.SetActive(false);
				return;
			}
		}
		iTween.CameraFadeAdd ();

	}

	public void ShowMenus() {

	}

	/// <summary>
	/// Play the specified animation in the Base Layer of the animator
	/// </summary>
	/// <param name="text">Text.</param>
	public void Play(string text)
	{
		CutsceneCamera.GetComponent<Animator> ().Play (Animator.StringToHash ("Base Layer." + text));
	}


	/// <summary>
	/// Instantiate a new Cut scene and call its Start method
	/// </summary>
	/// <param name="scene">Scene.</param>
	public void PlayCutscene(CutScene scene) 
	{
		scene.Manager = this;
		currentCutscene = scene;
		scene.Start ();
	}
	public void PlayCutscene(string scene)
	{
		Debug.Log ("PLAY CUTSCENE!");
		CutScene s = Activator.CreateInstance (Type.GetType (scene)) as CutScene;
		s.Manager = this;
		currentCutscene = s;
		s.Start ();
	}

	// Update is called once per frame
	void Update () {

		if (currentCutscene != null)
		{
			currentCutscene.Update();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (inCutsceneMode)
			{
				if (currentCutscene != null)
				{
				currentCutscene.OnSkip();
				currentCutscene = null;
				}
			}
		}

	}

	/// <summary>
	/// Switchs to cutscene mode
	/// </summary>
	/// <param name="onFadeFinish">On fade finish.</param>
	public void SwitchToCutscene(System.Action onFadeFinish = null)
	{
		fadeFinish = onFadeFinish;
		iTween.CameraFadeTo (iTween.Hash ("amount", 1, "time", 1, "oncomplete", "FadeBackToCutScene", "oncompletetarget", gameObject));
	}

	// Internal, callback from SwitchToCutScene
	void FadeBackToCutScene()
	{
		PlayerCamera.camera.enabled = false;
		CutsceneCamera.camera.enabled = true;
		iTween.CameraFadeTo (iTween.Hash ("amount", 0, "time", 1, "oncomplete", "OnFinish", "oncompletetarget", gameObject));
	}

	// Internal, it gets called after a switch has took place
	void OnFinish()
	{
		if (fadeFinish != null)
			fadeFinish();
	}

	/// <summary>
	/// Switchs to player camera.
	/// </summary>
	/// <param name="onFinishFade">On finish fade.</param>
	public void SwitchToPlayerCamera(System.Action onFinishFade)
	{
		fadeFinish = onFinishFade;
		iTween.CameraFadeTo (iTween.Hash ("amount", 1, "time", 1, "oncomplete", "FadeBackToPlayer", "oncompletetarget", gameObject));
	}
	void FadeBackToPlayer()
	{
		PlayerCamera.camera.enabled = true;
		CutsceneCamera.camera.enabled = false;
		iTween.CameraFadeTo (iTween.Hash ("amount", 0, "time", 1, "oncomplete", "OnFinish", "oncompletetarget", gameObject));
	}

	/// <summary>
	/// Walks to point and then do something.
	/// </summary>
	/// <param name="actor">Actor.</param>
	/// <param name="point">Point.</param>
	/// <param name="doSomething">Do something. Can be null</param>
	public void  WalkAndDoSomething(GameObject actor, Vector3 point, System.Action doSomething)
	{
		actor.GetComponent<NavMeshAgent> ().SetDestination (point);
		StartCoroutine (waitForArrivalAndDo (actor, doSomething));
	}
	bool stopWalkAndDo = false;
	public void StopWalkAndDo()
	{
		stopWalkAndDo = true;
	}

	// Internal, Wait for arrival and call dosomething
	protected IEnumerator waitForArrivalAndDo(GameObject actor, System.Action dosomething)
	{
		yield return new WaitForSeconds (0.2f);	
		NavMeshAgent agent = actor.GetComponent<NavMeshAgent> ();
		while(agent.remainingDistance > 0.1f)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (stopWalkAndDo)
		{
			stopWalkAndDo = false;
			return false;
		}
		else
			dosomething ();
	}

	bool CurrentActionComplete = false;

	[CutsceneAction]
	public void MoveCameraTo(Vector3 target, float time, CutScene scene )
	{
		iTween.MoveTo (CutSceneManager.instance.CutsceneCamera, iTween.Hash ("position", target, "time", time,"easetype", "easeInSine", "oncomplete", "ActionCompleted", "oncompleteparams", scene, "oncompletetarget", gameObject));
	}
	[CutsceneAction]
	public void CameraOnPath(Vector3[] path, float speed, bool orienttopath, CutScene scene)
	{
		iTween.MoveTo (CutSceneManager.instance.CutsceneCamera, iTween.Hash ("path", path, "speed", speed,"easetype", "easeInSine", "orienttopath", orienttopath, "oncomplete", "ActionCompleted", "oncompleteparams", scene, "oncompletetarget", gameObject));
	}
	[CutsceneAction]	
	public void  ActorWalkTo(GameObject actor, Vector3 point, CutScene scene)
	{
		actor.GetComponent<NavMeshAgent> ().SetDestination (point);
		StartCoroutine (waitForArrival (actor, scene));
	}
	[CutsceneAction]
	public void ActorWalkToObject(GameObject actor, GameObject target, CutScene scene)
	{
		actor.GetComponent<NavMeshAgent> ().SetDestination (target.transform.position);
		StartCoroutine (waitForArrival (actor, scene));
	}



	IEnumerator waitForArrival(GameObject actor, CutScene scene)
	{
		while(actor.GetComponent<NavMeshAgent>().remainingDistance > 0.1f)
		{
			yield return new WaitForEndOfFrame();
		}
		scene.currentActionComplete = true;
	}

	void ActionCompleted(CutScene scene)
	{
		Debug.Log ("ActionCompleted");
		scene.currentActionComplete = true;
	}
}

public class CutScene {
	public List<GameObject> Actors;

	private bool Speaking = false;
	private float _timer = 0;
	private int SpeakingIndex = 0;
	private List<string> Speakinglist = new List<string> ();
	private string speakerName = "";
	private System.Action OnfinishSpeak;

	public Dictionary<string, List<object>> ActionList = new Dictionary<string, List<object>> ();
	int currentAction = 0;
	public bool currentActionComplete = false;

	public virtual void Start() {}
	public virtual void Update() {
		if (currentAction > 0)
		{

			if (currentActionComplete)
			{
				Debug.Log("Completed Action #" + currentAction);
				
				if (currentAction >= ActionList.Count)
				{
					End();
					return;
				}

				currentActionComplete = false;
				string key = ActionList.Keys.ElementAt(currentAction);
				Type manager = Manager.GetType();
				System.Reflection.MethodInfo method = manager.GetMethod(key);
				ActionList[key].Add(this);
				object[] param = ActionList[key].ToArray();
				method.Invoke(Manager, param);
				currentAction++;

			}
		}
		else if (currentAction == 0)
		{

			currentActionComplete = false;
			string key = ActionList.Keys.ElementAt(currentAction);
			Type manager = Manager.GetType();
			System.Reflection.MethodInfo method = manager.GetMethod(key);
			ActionList[key].Add(this);
			method.Invoke(Manager, ActionList[key].ToArray());
			currentAction++;
		}
	}


	public virtual void End() {}

	public virtual void Speak(string name, string text) {
		GameHelper.WarningMessage (name + ": " + text);
	}
	public virtual void Move(GameObject actor, Vector3 xPoint) {}
	public virtual void PerformAction(GameObject actor, string action) {}

	public CutSceneManager Manager;

	protected void WalkdAndDoSomething(GameObject actor, Vector3 point, System.Action doSomething)
	{
		Manager.WalkAndDoSomething (actor, point, doSomething);
	}

	protected void SpeakList(List<string> texts, string name, System.Action OnSpeakFinish)
	{
		Speak (name, texts [0]);
		Speaking = true;
		Speakinglist = texts;
		speakerName = name;
		OnfinishSpeak = OnSpeakFinish;
	}

	protected void StopSpeakList()
	{
		Speaking = false;
		Speakinglist.Clear ();
		OnfinishSpeak = null;
	}

	public virtual void OnSkip()
	{
	}

	public CutScene() {}


	protected void DisablePlayer()
	{
		(GameHelper.GetPlayerComponent<ClickToMove> () as ClickToMove).enabled = false;
	}

	protected void EnablePlayer()
	{
		(GameHelper.GetPlayerComponent<ClickToMove> () as ClickToMove).enabled = true;
	}
}


public class TestCutscene : CutScene
{
	GameObject Player;
	GameObject PlayerFriend;

	public override void Start ()
	{
		base.Start ();
		DisablePlayer ();
		GameHelper.HideMenu (GameObject.Find ("QuickSlots"));
		//GameHelper.HideMenu (GameObject.Find ("SystemMessages"));
		GameHelper.HideMenu (GameObject.Find ("Inventory"));

		List<string> t = new List<string> () { "Hey! Svegliati! Svegliati!!!", "Sei vivo!? Oh grazie al cielo. Vieni alzati, dobbiamo scappare di qui!" };

		//GameHelper.ShowNotice ("Hey! Svegliati! Svegliati!!!", PlayerFriend);
		Player = GameHelper.GetLocalPlayer ();
		PlayerFriend = GameObject.Find ("PlayerFriend");
		PlayerFriend.GetComponent<Animator> ().Play (Animator.StringToHash ("Base Layer.Running"));
		WalkdAndDoSomething (PlayerFriend, Player.transform.position + Player.transform.forward, () => { 
						PlayerFriend.transform.LookAt (Player.transform);
			PlayerFriend.GetComponent<Animator> ().Play (Animator.StringToHash ("Base Layer.Idle"));
					//	Speak ("Lars", "Hey! Svegliati! Svegliati!!!");
			SpeakList(t, "Lars", () => { Manager.SwitchToPlayerCamera(() => {
					EnablePlayer(); 

					PlayerFriend.GetComponent<BasicNPC>().enabled = true; 
					//GameHelper.ShowMenu (GameObject.Find ("Inventory"));
					GameHelper.ShowMenu (GameObject.Find ("QuickSlots"));
					GameHelper.ShowNotice("Procurati un'arma... Ho visto degli scheletri qui intorno.", PlayerFriend);
					GameHelper.WarningMessage("Clicca con il tasto sinistro per muovere il personaggio. Tieni premuto il tasto destro per un movimento continuo. Il personaggio aggiusterà la velocità automaticamente in base alla distanza.");
					SaveLoad.Save(Application.loadedLevelName, CharManager.manager.character.Name);
				}); });
				});
	}

	public override void OnSkip ()
	{
		StopSpeakList ();
		Manager.SwitchToPlayerCamera (() => {
						EnablePlayer (); 
						StopSpeakList();
						Manager.StopWalkAndDo();
						PlayerFriend.GetComponent<BasicNPC> ().enabled = true; 
					//	GameHelper.ShowMenu (GameObject.Find ("Inventory")); 
						GameHelper.ShowMenu (GameObject.Find ("QuickSlots"));
					GameHelper.WarningMessage("Clicca con il tasto sinistro per muovere il personaggio. Tieni premuto il tasto destro per un movimento continuo. Il personaggio aggiusterà la velocità automaticamente in base alla distanza.");
					GameHelper.ShowNotice("Procurati un'arma... Ho visto degli scheletri qui intorno.", PlayerFriend);
					SaveLoad.Save("Caverna_2", CharManager.manager.character.Name);
				
				});
	}
}

[System.AttributeUsage(System.AttributeTargets.Method)]
public class CutsceneAction : System.Attribute
{
	public CutsceneAction()
	{
	}
}