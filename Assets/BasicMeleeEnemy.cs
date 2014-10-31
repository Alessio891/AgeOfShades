using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicMeleeEnemy : BasicEntity, IHostile {

	protected override void Init ()
	{
		base.Init ();
		gameObject.tag = "SimpleEnemy";
		BasicMeleeBehaviour standBehavior = new BasicMeleeBehaviour (gameObject);
		Behaviours.Add (standBehavior);
		Status.Strength = 25;
		Status.Life = 25;
		Status.MaxLife = 25;
		GetComponent<NavMeshAgent> ().speed = 2f;
		if (!UseMecanim)
			animation [Animations.WalkAnimation.name].speed = 1;

		RightHand = new Spada_lunga ();
		RightHand.WeaponDamage.owner = gameObject;

		EntitySkills s = GetComponent<EntitySkills> ();
		s.Skills ["Corpo A Corpo"] = new CorpoACorpo ();
		s.Skills ["Corpo A Corpo"].Value = 50;

		Loot.AddLootItem (typeof(Spada_lunga), 1, 0.4f);
	}


	void OnDrawGizmos()
	{
	

	}

	protected override void Die ()
	{
		if (!UseMecanim)
			animation.CrossFade (Animations.DieAnimation.name);
		GetComponent<NavMeshAgent> ().Stop ();
		Behaviours.Clear ();

		CheckForQuestRequirement ();

		AddCorpse ();

		base.Die ();
	}

	[RPC]
	public override void Damage (float damage)
	{
		if (Sounds.GetHitClip != null)
		{
			AudioSource s = gameObject.GetComponent<AudioSource>();
			bool found = false;
			foreach(AudioSource aus in GetComponents<AudioSource>())
			{
				if (aus.clip == Sounds.GetHitClip)
				{
					s = aus;
					found = true;
				}
			}
			if (!found)
				s = gameObject.AddComponent<AudioSource>();
			s.clip = GetComponent<BasicEntity>().Sounds.GetHitClip;
			s.priority = 200;
			s.minDistance = 20;
			s.Play();
			GameObject.Destroy(s, s.clip.length);
		}
		if (!UseMecanim)
			animation.Blend (Animations.GetHitAnimation.name, 1, 0.1f);
		Debug.Log ("Ouch! " + damage);
		base.Damage (damage);

	}

	public override void OnSelect ()
	{
		base.OnSelect ();

	}

	protected override void UpdateLogic ()
	{

	}
}
