using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;

public class GameManager : MonoBehaviour {
	
	public Texture Cursor;
	public Texture WarCursor;
	public Texture TargetCursor;
	public bool ShowCursor = false;
	public GameObject CurrentSelectedObject;
	public GameObject MeshToFile;
	GameObject ActionPanel;
	Camera ActionCamera;
	public static GameManager instance;
	public List<string> UIDS = new List<string>();

	Quaternion lastQuatCam;	
	Quaternion lastQuatPlayer;

	bool wasMoving = false;
	bool tutorialInventory = false;
	public bool altPressed = false;
	GameObject lastHitWall;
	bool gameStarted = false;
	// Use this for initialization
	void Awake () {
	//	if (LevelSerializer.IsDeserializing)
	//		return;
		instance = this;
		Screen.showCursor = true;
		Application.targetFrameRate = 140;
		//ObjExporter.MeshToFile (MeshToFile.GetComponent<MeshFilter> (), "BIGTREEOBJ.obj");
		ShowCursor = true;
		Screen.lockCursor = false;
		if (!gameStarted)
		{
			CraftingHelper.Initialize ();
			GameHelper.Init ();

	//		BFProfiler.Clear ();

			gameStarted = true;
		}
//		UnityEngine.Debug.Log ("Hi! It's me from the previous scene! The SkinColor is now..." + CharManager.manager.character.SkinColor.Color);

	}

	void Start()
	{
		GameHelper.HideMenu (GlobalUIManager.instance.Skills);
	//	Paperdoll.instance.Hide ();
		Paperdoll.instance.Show ();
		GameHelper.HideMenu (GlobalUIManager.instance.Spellbook);
		StartCoroutine (delayedHide ());
	}

	IEnumerator delayedHide()
	{
		yield return new WaitForEndOfFrame ();
		Paperdoll.instance.Hide ();
		Console.instance.gameObject.SetActive(false);	
		
	}

	float _keyDownTimer = 0;
	public bool playerCanMove = true;
	Vector3 remPosition;
	bool stopped = false;
	public bool ShowItemNames = false;
	GameObject actionTag;
	// Update is called once per frame
	bool reEnabledPlayer = false;
	byte[] savedData;
	void Update () {
		UIDS = GameHelper.UIDS;
		/*if (Input.GetKeyDown(KeyCode.G) && !ConsoleOpen())
		{
			SaveLoad.Save(Application.loadedLevelName, CharManager.manager.character.Name);
		}
		if (Input.GetKeyDown (KeyCode.B) && !ConsoleOpen()) {
			Application.LoadLevel("LoadingScreen");
		}*/
		if (Input.GetKeyDown (KeyCode.F1) && !ConsoleOpen())
		{
			SpellBook s = GameObject.Find("SpellBook").GetComponent<SpellBook>();
			if (!s.Visible)
				s.Show( (GameHelper.GetPlayerComponent<EntitySpells>() as EntitySpells).CurrentKnownSpells);
			else
				s.Hide ();
		}
		if (Input.GetKeyDown(KeyCode.F12))
		{
			Console.instance.gameObject.SetActive(!Console.instance.gameObject.activeSelf);	
		}
		if (!playerCanMove)
		{
			if (!stopped)
			{
				stopped = true;
				remPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
			}
			GameObject.FindGameObjectWithTag("Player").transform.position = remPosition;
		}
		else
		{
			if (stopped)
				stopped = false;
		}

		if (Input.GetKey(KeyCode.LeftControl))
		{
			Time.timeScale = 0.2f;
		}
		else
			Time.timeScale = 1;


		// Menu Keys Handle

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PlayerCombat pc = (PlayerCombat)GameHelper.GetPlayerComponent<PlayerCombat>();
			PlayerInventory pi = (PlayerInventory)GameHelper.GetPlayerComponent<PlayerInventory>();
			if (pc.Target != null)
			{
				Destroy(pc.TargetMarker);
				Destroy(pc.markerParent);
				foreach(Canvas c in pc.Target.GetComponentsInChildren<Canvas>())
				{
					if (c.name == "HealthBar")
					{
						c.enabled = false;
					}
				}
				pc.Target = null;
				
			}

			if (pi.Selected != null)
			{
				BasicItem bi = pi.Selected.GetComponent<BasicItem>();
				bi.hasMarker = false;
				Destroy(bi.marker);
				pi.Selected = null;
			}
		}

		if (Input.GetKeyDown(KeyCode.P) && !ConsoleOpen())
		{
			Paperdoll paperdoll = GameObject.Find("Paperdoll").GetComponent<Paperdoll>();
			if (paperdoll.Visible)
				paperdoll.Hide();
			else
				paperdoll.Show();
		}

		if (Input.GetKeyDown(KeyCode.I) && !ConsoleOpen())
		{
			InventoryMenu inventory = GameObject.Find("Inventory").GetComponent<InventoryMenu>();
			if (inventory.Visible)
				GameHelper.HideMenu(inventory.gameObject);
			else
				GameHelper.ShowMenu(inventory.gameObject);
		}

		if (Input.GetKeyDown(KeyCode.K) && !ConsoleOpen())
		{
			SkillPanel skill = GameObject.Find("SkillsPanel").GetComponent<SkillPanel>();
			if (skill.Visible)
				GameHelper.HideMenu(skill.gameObject);
			else
				GameHelper.ShowMenu(skill.gameObject);
		}


		if (Input.GetKey(KeyCode.LeftShift))
		{
			ShowItemNames = true;
		}
		else
			ShowItemNames = false;

		if (Input.GetKey(KeyCode.Escape))
		{

		}
		else
		{
			_keyDownTimer = 0;
		}




		foreach(GameObject o in GameObject.FindGameObjectsWithTag("FloatText"))
		{
			if ( o.GetComponent<TextMesh>().renderer.enabled )
			{
				o.transform.LookAt(Camera.main.transform);
				Vector3 angles = o.transform.localEulerAngles;
				angles.y -= 180;
				angles.x = 0;
				o.transform.localEulerAngles = angles;
			}
		}

	

	}

	public void SaveCurrent()
	{
		SaveLoad.Save (GameHelper.GetDataManager ().currentGame.GameName);
	}

	public void Load(string name)
	{
		UnityEngine.Debug.Log ("Load start");
		SaveLoad.Load (name);
	}

	public void LoadLast()
	{

		SaveLoad.Load(GameHelper.GetDataManager().currentGame.GameName);
		
	}

	bool ConsoleOpen()
	{
		return (Console.instance.gameObject.activeSelf);
	}

	public void ExitGame()
	{
		Application.Quit ();
	}

	void OnGUI()
	{
		if (!ShowCursor)
			return;
		Texture toDraw = Cursor;
		if (GameHelper.GetLocalPlayer().GetComponent<PlayerCombat>().WarState)
			toDraw = WarCursor;
		if (GameHelper.GetLocalPlayer().GetComponent<ClickToMove>().Targeting)
			toDraw = TargetCursor;
		GUI.DrawTexture (new Rect (Input.mousePosition.x, Screen.height - Input.mousePosition.y, 32, 32), toDraw);
	}
}

public static class GameHelper {

	/// <summary>
	/// Restituisce il GameDataManager corrente
	/// </summary>
	/// <returns>L'istanza corrente</returns>
	public static GameDataManager GetDataManager()
	{
		if (GameObject.FindObjectOfType(typeof(GameDataManager)) != null)
		{
			return GameObject.FindObjectOfType(typeof(GameDataManager)) as GameDataManager;
		}
		GameObject o = new GameObject ();
		GameDataManager m = o.AddComponent<GameDataManager> ();
		m.currentGame = new Game ("Game1");
		m.currentGame.StartNewGame ();
		return m;
	}

	/// <summary>
	/// Mostra il testo text sopra al GameObject owner
	/// </summary>
	/// <param name="text">Il testo da visualizzare</param>
	/// <param name="owner">Se impostato null, il testo verrà visualizzato sul giocatore</param>
	public static void ShowNotice(string text, GameObject owner = null)
	{
//		GameObject.Find("Notice").GetComponent<NoticePanel>().Play(text, owner);
		if (owner == null)
		{
			SystemMessage("Io: " + text, Color.white);
		}
		else
			SystemMessage(owner.name + ": " + text, Color.white);
			
	}

	/// <summary>
	/// Restituisce un array di giocatori vicino ad un certo GameObject obj.
	/// </summary>
	/// <returns>Array di GameObject</returns>
	/// <param name="obj">Il centro del cerchio</param>
	/// <param name="distance">Il raggio del cerchio da controllare</param>
	public static GameObject[] AnyPlayerNearby(GameObject obj, float distance = 5)
	{
		List<GameObject> result = new List<GameObject> ();

		if (Vector3.Distance(obj.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < distance)
		{
			result.Add(GameObject.FindGameObjectWithTag("Player"));
		}
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("OnlinePlayer"))
		{
			if ( Vector3.Distance(o.transform.position, obj.transform.position) < distance)
			{
				result.Add(o);
			}
		}
		//result.Sort ((e1, e2) => Vector3.Distance (GetLocalPlayer ().transform.position, e1.transform.position) < 
		//				Vector3.Distance (GetLocalPlayer ().transform.position, e2.transform.position));

		return (from o in result orderby Vector3.Distance (GetLocalPlayer ().transform.position, o.transform.position) select o).ToArray ();

	}


	/// <summary>
	/// Restituisce il GameObject del personaggio locale, anche in una partita multiplayer.
	/// </summary>
	/// <returns>Il GameObject del personaggio</returns>
	public static GameObject GetLocalPlayer()
	{
		return GameObject.FindGameObjectWithTag ("Player");
	}

	public static List<GameObject> MenuStack;
	public static void Init()
	{
		MenuStack = new List<GameObject> ();
		SpeechBubbles = new List<GameObject> ();
		UIDS = new List<string> ();
	}
	
	public static void ShowTutorialMessage(string text)
	{
//		GameObject.Find ("TutorialPanel").GetComponent<TutorialPanel> ().ShowMessage (text);
	}

	public static void SetPlayerInput(bool enable = false)
	{
	}

	/// <summary>
	/// Imposta un oggetto nel QuickSlot col nome slot.
	/// </summary>
	/// <param name="slot">Slot.</param>
	/// <param name="item">Item.</param>
	public static void PutInQuickSlot(string slot, InventoryItem item)
	{
		GameObject.Find (slot).GetComponentInChildren<QuickSlotButton> ().Slot = item;
	}

	public static List<GameObject> SpeechBubbles;


	/// <summary>
	/// Una lista di ID unici assegnati. Questa lista si sincronizza con quella del GameManager all'avvio.
	/// </summary>
	public static List<string> UIDS;

	/// <summary>
	/// Genera un ID unico per l'oggetto o
	/// </summary>
	/// <returns>Un UID nel formato o.ToString() + "_xxx" </returns>
	/// <param name="o">L'oggetto da passare</param>
	public static string GetUIDForObject(object o)
	{
		int id = 0;
		string uid = o.ToString() + "_0";
		if (UIDS == null)
		{
			GameHelper.UIDS = new List<string>();
			UIDS = GameHelper.UIDS;
		}
		while(UIDS.Contains(uid))
		{
			id++;
			uid = o.ToString() + "_" + uid.ToString();
		}
		UIDS.Add (uid);
		UnityEngine.Debug.Log("UID for " + o + " " + uid);
		return uid;
	}

	/// <summary>
	/// Gets the player component of type T.
	/// </summary>
	/// <returns>The player component.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T GetPlayerComponent<T>()
	{
		if (GetLocalPlayer() != null)
			return (T)( (object) GetLocalPlayer ().GetComponent (typeof(T)) );
		else
			return default(T);
	}

	/// <summary>
	/// Gets the child of the Parent gameobject.
	/// </summary>
	/// <returns>The gameobject child of Parent with the name childName</returns>
	/// <param name="Parent">Parent.</param>
	/// <param name="childName">Child name.</param>
	/// <param name="useRectTransform">If set to <c>true</c> use rect transform.</param>
	public static GameObject GetChildOf(GameObject Parent, string childName, bool useRectTransform = false)
	{
		if (!useRectTransform)
		{
			foreach(Transform t in Parent.GetComponentsInChildren<Transform>())
			{
				if (t.name == childName)
					return t.gameObject;
			}
		}
		else
		{
			foreach(RectTransform t in Parent.GetComponentsInChildren<RectTransform>())
			{
				if (t.name == childName)
					return t.gameObject;
			}
		}

		return null;
	}

	/// <summary>
	/// Gets the component in child of parent called childName
	/// </summary>
	/// <returns>The component in child of.</returns>
	/// <param name="parent">Parent.</param>
	/// <param name="childName">Child name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static Component GetComponentInChildOf<T>(GameObject parent, string childName)
	{
		foreach(Component item in parent.GetComponentsInChildren(typeof(T)))
		{
			if (item.name == childName)
			{
				return item as Component;
			}
		}
		return null;
	}

	/// <summary>
	/// Scrive un messaggio nella chat in basso a sinistra dello schermo.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="color">Color.</param>
	/// <param name="outline">If set to <c>true</c> outline.</param>
	public static void SystemMessage(string text, Color color, bool outline = true)
	{
		GameObject.Find ("SystemMessages").GetComponent<SystemMessages> ().Write (text, color, outline);	
	}

	/// <summary>
	/// Shows the menu.
	/// </summary>
	/// <param name="menu">Menu.</param>
	public static void ShowMenu(GameObject menu)
	{
		foreach(Graphic e in menu.GetComponentsInChildren<Graphic>())
		{
			e.enabled = true;
		}

	}
	/// <summary>
	/// Hides the menu.
	/// </summary>
	/// <param name="menu">Menu.</param>
	public static void HideMenu(GameObject menu)
	{
		foreach(Graphic e in menu.GetComponentsInChildren<Graphic>())
		{
			e.enabled = false;
		}
	}
	/// <summary>
	/// Mostra un messaggio in alto nello schermo
	/// </summary>
	/// <param name="text">Text.</param>
	public static void WarningMessage(string text)
	{
		GameObject.Find ("WarningMessages").GetComponent<WarningMessages> ().Show (text);
	}

	/// <summary>
	/// Apre un menu per mostrare gli oggetti contenuti in un cadavere. Restituisce un oggetto Container creato.
	/// </summary>
	/// <returns>Container</returns>
	/// <param name="loot">Loot.</param>
	/// <param name="c">Cadavere</param>
	public static Container ShowCorpse(List<InventoryItem> loot, Corpse c)
	{
		GameObject corpseGump = GameObject.Instantiate (Resources.Load ("CorpseGump")) as GameObject;
		corpseGump.transform.position = new Vector3(UnityEngine.Random.Range(200, Screen.width - 200), UnityEngine.Random.Range(200,Screen.height-200), 0);
		corpseGump.GetComponent<Container> ().Corpse = c;
		corpseGump.transform.SetParent (GameObject.Find ("GlobalUI").transform,false);
		corpseGump.transform.localScale = Vector3.one;

		
		Transform corpseRect = GameHelper.GetComponentInChildOf<Transform> (corpseGump, "CorpseRect") as Transform;
		foreach(InventoryItem item in loot)
		{
			GameObject i = new GameObject();
			i.AddComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + item.ItemIcon) as Sprite;
			i.AddComponent<CorpseLootItem>().Item = item;
			i.transform.SetParent(corpseRect, true);
			i.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 1);
			i.GetComponent<RectTransform>().localPosition = new Vector3(UnityEngine.Random.Range(-80, 80), UnityEngine.Random.Range(-80,80), 0);
			i.GetComponent<CorpseLootItem>().Corpse = c;
		}
		return corpseGump.GetComponent<Container> ();
	}

	static List<GameObject> managers;
	static Vector3 spPoint;
	/// <summary>
	/// Loads the new scene.
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	/// <param name="spawnPoint">Spawn point.</param>
	public static void LoadNewScene(string sceneName, Vector3 spawnPoint)
	{
		spPoint = spawnPoint;
		managers = GameObject.FindGameObjectsWithTag ("Manager").ToList();
		managers.Add(GameObject.Find("ThirdPersonCamera"));
		foreach(GameObject o in managers)
		{
			o.SetActive(false);
		}
		SaveLoad.SaveCharacter (CharManager.manager.character.Name);
		CharManager.manager.character.SceneName = sceneName;
		CharManager.manager.character.LevelTransition = true;
		CharManager.manager.StartRoutine (WaitForScene (sceneName));
		Application.LoadLevel ("LoadingScreen");
	}
	static IEnumerator WaitForScene(string scene)
	{
		while(Application.loadedLevelName != scene)
		{
			yield return new WaitForSeconds(0.1f);
		}
		//managers = GameObject.FindGameObjectsWithTag ("Manager");
		foreach(GameObject o in managers)
		{
			o.SetActive(true);
		}
		NetworkManager.instance.InstantiatePlayer (spPoint);
		GameHelper.GetPlayerComponent<PlayerManager> ().ctm.enabled = true;
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="GameHelper"/> game is loading.
	/// </summary>
	/// <value><c>true</c> if game is loading; otherwise, <c>false</c>.</value>
	public static bool GameIsLoading {
				get {
					return GameObject.FindObjectOfType(typeof(LoadCoroutine)) != null;
				}
		}
	/// <summary>
	/// Gets a value indicating whether this <see cref="GameHelper"/> first time loading.
	/// </summary>
	/// <value><c>true</c> if first time loading; otherwise, <c>false</c>.</value>
	public static bool firstTimeLoading
	{
		get {
			return GameHelper.GetDataManager().currentGame.firstLoad;
		}
	}

	/// <summary>
	/// Gets the SpellDatabase
	/// </summary>
	/// <value>The spell Database</value>
	public static SpellsDatabase SpellDB { get { return GameObject.FindObjectOfType(typeof(SpellsDatabase)) as SpellsDatabase; } }

	public static void SetPlayerState(CharacterState state)
	{
		GetLocalPlayer ().GetComponent<Animator> ().SetInteger ("CharacterState", (int)state);
		foreach(Animator a in GetLocalPlayer().GetComponentsInChildren<Animator>())
		{
			a.SetInteger("CharacterState", (int)state);
			UnityEngine.Debug.Log(a.name);
		}
	}
	public static void PlayAnimationOnPlayer(string an)
	{
		GetLocalPlayer ().GetComponent<Animator> ().Play (Animator.StringToHash (an));
		foreach(Animator a in GetLocalPlayer().GetComponentsInChildren<Animator>())
		{
			a.Play (Animator.StringToHash (an));
			UnityEngine.Debug.Log(a.name);
		}
	}
	/// <summary>
	/// Gets the entities near point.
	/// </summary>
	/// <returns>The entities near point.</returns>
	/// <param name="point">Point.</param>
	/// <param name="radius">Radius.</param>
	public static List<BasicEntity> GetEntityNearPoint(Vector3 point, float radius)
	{
		List<BasicEntity> value = new List<BasicEntity> ();
		foreach(object o in (from o in GameObject.FindObjectsOfType (typeof(BasicEntity)) where (Vector3.Distance ((o as BasicEntity).transform.position, point) <= radius) select o))
		{
			value.Add(o as BasicEntity);
		}
		return value;
	}

}

[System.AttributeUsage(System.AttributeTargets.Class)]
public class BFCategory : System.Attribute
{
	public string path;
	
	public BFCategory(string p)
	{
		path = p;
	}	
}
public static class AttributeExtensions
{
	public static TValue GetAttributeValue<TAttribute, TValue>(
		this Type type, 
		Func<TAttribute, TValue> valueSelector) 
		where TAttribute : Attribute
	{
		var att = type.GetCustomAttributes(
			typeof(TAttribute), true
			).FirstOrDefault() as TAttribute;
		if (att != null)
		{
			return valueSelector(att);
		}
		return default(TValue);
	}
}
public interface IUIElement
{
	void Show();
	void Hide();
}