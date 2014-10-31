using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Console : MonoBehaviour {

	public Dictionary<string, ConsoleCommand> Commands = new Dictionary<string, ConsoleCommand>();
	public List<string> lastMessages = new List<string> ();
	public GameObject consoleText;
	public static Console instance;
	// Use this for initialization
	void Awake()
	{
		instance = this;
	}

	void Start () {
	//	GameHelper.HideMenu (gameObject);
		Commands.Add ("AddItem", new AddItem());
		Commands.Add ("Spawn", new Spawn ());
		Commands.Add ("GoTo", new GoTo ());
		Commands.Add ("FillInventory", new FillInventory ());
		Commands.Add ("SetStat", new SetStat ());
		Commands.Add ("SetSkill", new SetSkill ());
		Commands.Add ("StartServer", new StartServer ());
		Commands.Add ("DeveloperMode", new DeveloperMode ());
		Commands.Add ("TestDialog", new TestDialog ());
		GetComponent<InputField> ().onSubmit.AddListener ((value) => OnSubmit (value));
	}

	void OnSelect(bool isSelected)
	{
	}
	void OnDeselectEvent()
	{
		Debug.Log("Deselezionato");
	}


	void OnSubmit(string m)
	{
		string clean = m.Replace ("|", "");
		string[] fetched = clean.Split (new string[] { " " }, StringSplitOptions.None);
		Debug.Log (fetched [0]);
		string[] paramS = new string[fetched.Length - 1];
		for(int i = 1; i < fetched.Length; i++)
			paramS[i-1] = fetched[i];
		if (Commands.ContainsKey(fetched[0]))
		{
			Debug.Log(fetched[0]);
			Commands[fetched[0]].OnCommand( paramS );
		}
		else
		{
			AddMessage("Il comando " + fetched[0] + " non esiste.");
		}
		AddMessage(clean);


	}

	public void AddMessage(string mess)
	{

		GameHelper.SystemMessage ("Console: " + mess, Color.green);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class ConsoleCommand
{
	public string Command = "set";
	public Action DoWhat;
	public virtual void OnCommand(object[] parameters)
	{
	}

	public virtual void InvalidUse(string mess)
	{
		Console.instance.AddMessage (mess);
	}
}

public class AddItem : ConsoleCommand
{
	public AddItem()
	{
		Command = "AddItem";
	}

	public override void OnCommand (object[] parameters)
	{
		if (parameters.Length < 2)
		{
			InvalidUse ("Uso: AddItem <TipoOggetto> <Quantita>");
			return;
		}
		InventoryItem i;// = ItemDatabase.Items [Type.GetType (parameters [0])];
		if (ItemDatabase.Items.TryGetValue (Type.GetType (parameters [0].ToString()), out i))
		{
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerInventory> ().PickupObject (i,
		                                                                                        Convert.ToInt32(parameters [1]));
		}
		else
		{
			InvalidUse("Impossibile trovare il tipo " + parameters[0]);
		}

	}
	
}

public class GoTo : ConsoleCommand
{
	public GoTo()
	{
		Command = "GoTo";
	}
	public override void OnCommand(object[] parameters)
	{
		if (parameters.Length < 3)
			return;
		Debug.Log (parameters[0] + "," + parameters[1] + "," + parameters[2]);
		GameHelper.GetLocalPlayer ().transform.position = new Vector3 (System.Convert.ToSingle(parameters [0]), System.Convert.ToSingle(parameters [1]), System.Convert.ToSingle(parameters [2]));
	}
}

public class Spawn : ConsoleCommand
{
	public Spawn()
	{
		Command = "Spawn";
	}

	public override void OnCommand(object[] parameters)
	{
		if (parameters.Length < 1)
			return;
		GameObject o = GameObject.Instantiate( Resources.Load ((string)parameters[0]) ) as GameObject;
		o.transform.position = GameObject.FindGameObjectWithTag ("Player").transform.position;
		Debug.Log (o);
	}
} 

public class FillInventory : ConsoleCommand
{
	public FillInventory()
	{
		Command = "FillInventory";
	}
	public override void OnCommand (object[] parameters)
	{
		foreach(GameObject o in GameHelper.GetLocalPlayer().GetComponent<PlayerInventory>().InventorySlots)
		{
			o.GetComponent<InventorySlot>().Item = new Ascia();
			o.GetComponent<InventorySlot>().stack = 1;
		}
	}
}

public class SetStat : ConsoleCommand
{
	public SetStat()
	{
		Command = "SetStat";
	}

	public override void OnCommand (object[] parameters)
	{
		// 0 = attribute
		// 1 = value
		if (parameters.Length < 2)
		{
			InvalidUse("Uso: SetStat <Stat> <Valore>");
			return;
		}
		typeof(EntityStatus).GetField (parameters [0] as string).SetValue ((EntityStatus)GameHelper.GetPlayerComponent<EntityStatus> (), Convert.ToSingle(((string)parameters [1])));
	}
}

public class SetSkill : ConsoleCommand
{
	public SetSkill()
	{
		Command = "SetSkill";
	}
	
	public override void OnCommand (object[] parameters)
	{
		// 0 = attribute
		// 1 = value
		if (parameters.Length < 2)
		{
			InvalidUse("Uso: SetSkill <Skill> <Valore>");
			return;
		}
		//typeof(EntitySkills).GetField (parameters [0] as string).SetValue ((EntitySkills)GameHelper.GetPlayerComponent<EntitySkills> (), Convert.ToSingle(((string)parameters [1])));
		GameHelper.GetPlayerComponent<EntitySkills> ().Skills [parameters [0] as string].Value = Convert.ToSingle (parameters [1] as String);
	}
}

public class StartServer : ConsoleCommand
{
	public StartServer()
	{ Command = "StartServer"; }

	public override void OnCommand (object[] parameters)
	{
		NetworkManager.instance.StartCoroutine (ConnectAndCreateRoom ("TestRoom"));
	}

	IEnumerator ConnectAndCreateRoom(string roomName)
	{
		PhotonNetwork.LeaveRoom ();
		PhotonNetwork.offlineMode = false;
		PhotonNetwork.ConnectUsingSettings ("1.0");
		while(!PhotonNetwork.insideLobby)
		{
			yield return new WaitForSeconds(0.1f);
		}
		NetworkManager.instance.CreateNewRoom (roomName);

	}
}

public class DeveloperMode : ConsoleCommand
{
	public DeveloperMode() { Command = "DeveloperMode"; }

	public override void OnCommand (object[] parameters)
	{
		GameHelper.GetPlayerComponent<EntityStatus>().ApplyBuff(new GodMode());
	}
}

public class TestDialog : ConsoleCommand
{
	public TestDialog() { Command = "TestDialog"; }

	public override void OnCommand (object[] parameters)
	{
		string file = (string)parameters [0];
		GameHelper.ShowMenu (DialogueManager.instance.gameObject);
		DialogueManager.instance.ShowDialogue (file, "Root", true);
	}
}