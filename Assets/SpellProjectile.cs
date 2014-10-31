using UnityEngine;
using System.Collections;

public class SpellProjectile : MonoBehaviour {

	public BaseProjectileSpell spell;

	void OnTriggerEnter(Collider c)
	{
		if (spell != null)
			spell.OnProjectileHit (c);
	}
	
}
