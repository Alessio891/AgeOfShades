using UnityEngine;
using System.Collections;

public interface IBuff
{
	void OnApply();
	void OnCleanse();
	void OnDecay();
	void OnTick();

	string Name { get; set; }
	string Description { get; set; }

	float Duration { get; set; }

	EntityStatus Status { get; set; }

	float TickEvery { get; set; }

	bool ToBeRemoved { get; set; }
}

public class Buff : IBuff
{
	public Buff() {
		Duration = 5;
		Name = "Base Debuff";
		Description = "Buff!";
	}

	// Interface Implementations
	public virtual void OnApply() {
		GameHelper.SystemMessage (Name + " è stato applicato!", Color.red);
	}
	public virtual void OnCleanse() {}
	public virtual void OnDecay() {
		ToBeRemoved = true;
	}
	public virtual void OnTick() {
		GameHelper.SystemMessage ("Sei buffato!", Color.gray);
		Status.Damage (2);
	}

	string _name;
	public string Name { get { return _name; } set { _name = value; } }
	public string _description;
	public string Description { get { return _description; } set { _description = value; } }
	float _duration;
	public float Duration { get { return _duration; } set { _duration = value; } } 
	EntityStatus _status;
	public EntityStatus Status { get { return _status; } set { _status = value; } }
	float _tickEvery;
	public float TickEvery { get { return _tickEvery; } set { _tickEvery = value; } }
	bool _toBeRemoved;
	public bool ToBeRemoved { get { return _toBeRemoved; } set { _toBeRemoved = value; } }
}

public class HiddenBuff : Buff
{
	public HiddenBuff()
	{
		Duration = 10;
		TickEvery = 0.5f;
		Name = "Nascosto";
		Description = "Shh...";
	}

	Vector3 lastPos;
	public override void OnTick ()
	{


		if (lastPos != Status.transform.position)
		{
			GameHelper.SystemMessage("Sei stato scoperto!", Color.red);
			ToBeRemoved = true;
		}
		lastPos = Status.transform.position;
	}

	public override void OnApply ()
	{
		lastPos = Status.transform.position;
		if (Status.AttachedToPlayer)
		{
			Status.GetComponent<PlayerMeshManager>().ChangeAllColors(Color.grey);
		}
	}

	public override void OnDecay ()
	{
		if (Status.AttachedToPlayer)
		{
			Status.GetComponent<PlayerMeshManager>().RestoreOldColors();
		}
	}
}

public class HealOverTime : Buff
{
	public HealOverTime()
	{
		Name = "Ringiovanimento";
		Description = "Sei in fase di cura.";
		Duration = 5;
		TickEvery = 0.5f;
	}

	public override void OnTick ()
	{
		Status.Life += 0.2f;
	}
}

public class PassoSveltoBuff : Buff
{

	public float SpeedIncrease = 4;
	public PassoSveltoBuff(float speed)
	{
		SpeedIncrease = speed;
		Name = "Passo Svelto";
		Description = "Agile come una lucertola!";
		Duration = 20;
		TickEvery = 20;
	}

	public override void OnTick ()
	{

	}

	public override void OnApply ()
	{
		ClickToMove cm = Status.GetComponent<ClickToMove> ();
		if (cm != null)
		{
			cm.SpeedModifier += SpeedIncrease;
		}
		GameHelper.SystemMessage ("Veloce come il vento!", Color.blue);
	}

	public override void OnDecay ()
	{
		ClickToMove cm = Status.GetComponent<ClickToMove> ();
		if (cm != null)
		{
			cm.SpeedModifier -= SpeedIncrease;
		}
		GameHelper.SystemMessage ("Lento come una tartaruga :(", Color.blue);
	}
}

public class GodMode : Buff
{
	public GodMode() : base() {}

	public override void OnDecay ()
	{
	}
	public override void OnTick ()
	{

	}
}