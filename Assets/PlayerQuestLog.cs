using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerQuestLog : MonoBehaviour {
	public List<BaseQuest> Quests = new List<BaseQuest>();
	public List<BaseQuest> Completed = new List<BaseQuest>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddQuest(BaseQuest q)
	{
		Quests.Add (q);

	}

	IEnumerator disableQuestAcceptedPanel()
	{
		yield return new WaitForSeconds (2);
	}

	public BaseQuest GetQuest(string name)
	{
		foreach(BaseQuest q in Quests)
		{
			if (q.QuestID == name)
				return q;
		}
		return null;
	}

	public void RemoveQuest(BaseQuest quest)
	{
		int i = 0;
		foreach(BaseQuest q in Quests)
		{
			if (q.QuestID == quest.QuestID)
			{
				break;
			}
			i++;
		}
		Quests.RemoveAt (i);
	}

	public bool QuestCompleted(BaseQuest q)
	{
		return q.QuestComplete ();
	}


}

public enum QuestTypes
{
	Gather = 0,
	KillMany,
	KillOne,
	TalkTo,
	Other
}
[System.Serializable]
public enum QuestState
{
	Start = 0,
	InProgress,
	Completed
}
[System.Serializable]
public class QuestPhase
{
	public QuestTypes Type;
	public string Name;
	public string Description;
	public string[] Text;
	public string[] CompletedText;
	public string[] UnfinishedText;
	public bool Complete = false;
	public QuestState State = QuestState.Start;
	// Gather, KillMany & KillOne
	public Dictionary<System.Type, int> Requested= new Dictionary<System.Type, int>();
	public Dictionary<System.Type, int> Killed = new Dictionary<System.Type, int> ();
	// typeof(InventoryItem) for Gather, typeof(BasicEntity) for KillMany & KillOne

	// TalkTo
	public string TalkToName;

	public Action<object[]> OnAccept;
	public Action<object[]> OnComplete;

	public List<QuestReward> Rewards = new List<QuestReward>();
	
	public void SetQuestComplete()
	{
		Complete = true;
	}

	 

	public virtual bool Completed()
	{
		switch(Type)
		{
		case QuestTypes.Gather:
			bool complete = true;
			foreach(KeyValuePair<System.Type, int> pair in Requested)
			{
				if (!GameHelper.GetLocalPlayer().GetComponent<PlayerInventory>().Has( ItemDatabase.Items[pair.Key], pair.Value))
				{
					complete = false;
					break;
				}
			}
			return complete;
		case QuestTypes.KillMany:
			foreach(KeyValuePair<System.Type, int> pair in Requested)
			{
				if (Killed.ContainsKey(pair.Key) && Killed[pair.Key] < pair.Value)
				{
					return false;
				}
				else if (!Killed.ContainsKey(pair.Key))
					return false;
			}
			return true;
		case QuestTypes.KillOne:
			break;
		case QuestTypes.TalkTo:
			break;
		case QuestTypes.Other:
			return Complete;
			break;
		}
		return true;
	}
	
	public QuestPhase(string name, string desc, QuestTypes type)
	{
		Name = name;
		Description = desc;
		Type = type;
	}

	public void CollectRewards()
	{
		foreach(QuestReward q in Rewards)
			q.Collect();
	}

}
[System.Serializable]
public class BaseQuest
{
	protected List<QuestPhase> Phases = new List<QuestPhase> ();
	public int currentPhase = 0;
	protected GameObject caller;
	public string QuestID;
	public BaseQuest(GameObject caller)
	{
		this.caller = caller;
	}

	public void AddPhase(QuestPhase phase)
	{
		Phases.Add (phase);
	}

	public bool PhaseComplete()
	{
		return Phases [currentPhase].Completed ();
	}

	public void AdvancePhase()
	{
		currentPhase++;
		if (currentPhase >= Phases.Count)
		{
			currentPhase = 0;
		}
	}

	public bool QuestComplete()
	{
		foreach(QuestPhase p in Phases)
		{
			if (!p.Completed())
				return false;
		}
		return true;
	}

	public void Show()
	{

		if (Phases[currentPhase].State == QuestState.InProgress)
		{
			if (Phases[currentPhase].Completed())
			{
				Phases[currentPhase].State = QuestState.Completed;
			}
		}

		Vector3 pos = Vector3.zero;
		foreach(Transform t in caller.GetComponentsInChildren<Transform>())
		{
			if (t.name == "CamPosition")
			{
				pos = t.position;
			}
		}
		iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("position", pos, "easeType", "easeInOutSine", "time", 1f, "lookTarget", pos - caller.transform.forward));

		switch(Phases[currentPhase].State)
		{
		case QuestState.Start:
			break;
		case QuestState.InProgress:
			
			break;
		case QuestState.Completed:
			
			break;
		}
	}

	public void AcceptDialog(object[] par)
	{
		if (Phases[currentPhase] != null)
		{
//			Camera.main.GetComponent<WowCamera>().ease = true;

			if (Phases[currentPhase].State != QuestState.Completed)
				Phases [currentPhase].OnAccept (par);
			else
				Phases[currentPhase].OnComplete(par);
		}
	}

	void StartDialog(string[] message, bool buttons, string acceptName = "Ok", string declineName = "Annulla")
	{
	}

	public QuestPhase GetCurrentPhase()
	{
		return Phases[currentPhase];
	}
}

[System.Serializable]
public enum RewardType
{
	Other = 0,
	SkillPoints,
	Item,
	StatPoint,
	Coins
}
[System.Serializable]
public class QuestReward
{
	public RewardType Type;
	public InventoryItem item;
	public float itemAmount;
	public string skillName;
	public float skillAmount;

	public QuestReward(RewardType type, InventoryItem item, float itemamount, string skillname, float skillamount)
	{
		Type = type;
		this.item = item;
		itemAmount = itemamount;
		skillName = skillname;
		skillAmount = skillamount;
	}

	public void Collect()
	{
		switch(Type)
		{
		case RewardType.Item:
			if (item != null)
			{
				GameHelper.GetLocalPlayer().GetComponent<PlayerInventory>().PickupObject(item, (int)itemAmount);
			}
			break;
		case RewardType.SkillPoints:
			GameHelper.GetLocalPlayer().GetComponent<EntitySkills>().Skills[skillName].Value += skillAmount;
			break;
		}
	}

}