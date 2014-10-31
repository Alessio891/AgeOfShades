using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// TEMP
[System.Serializable]
public class Reagent
{
	public string name;
}

public interface ISpell
{
	string Name { get; set; }
	string Description { get; set; }
	GameObject Effect { get; set; }
	GameObject GO { get; }
	Sprite Icon { get; }
	List<Reagent> ReagentsNeeded { get; }
	Elements Element { get; set; }
	int Circle { get; }
	float RequiredMagery { get; set; }
	string PowerWords { get; set; }
	bool CanCastWhileWalking { get; set; }

	void Cast(ICaster caster);
	void Fizzle();

	void FinalizeCasting();

	void InterruptCast();

	bool CanCast();

}

public interface ICaster
{
	Transform Transform { get; }

	bool SkillCheckSuccessful(string skillName, float checkValue);

	bool isPlayer { get; }
	bool IsCasting { get; set; }

	Image CastingBar { get; }

	void ConsumeMP(float mp);
	bool HasEnoughMP(float mp);

}

public interface ITargetSpell
{
	bool WaitingForTarget { get; }
	void OnTargetSelect(object target);
	void WaitForTarget();
}