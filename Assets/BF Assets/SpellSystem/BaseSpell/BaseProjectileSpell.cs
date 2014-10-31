using UnityEngine;
using System.Collections;

public class BaseProjectileSpell : CastTimeSpell {

	public GameObject Projectile;
	GameObject p;
	Vector3 dir;
	public float speed;
	void Start()
	{
		IsProjectile = true;
	}

	public virtual void OnProjectileHit(Collider c)
	{
		if (c.GetComponent<BasicEntity>() != null)
		{
			c.GetComponent<BasicEntity>().Damage(5);
			Destroy(p);
			Destroy(gameObject);
		}
	}

	public override void Cast (ICaster caster)
	{
		base.Cast (caster);
		if (!weCanCast)
			return;
		p = Instantiate (Projectile) as GameObject;
		p.GetComponent<SpellProjectile> ().spell = this;
		p.transform.position = caster.Transform.position + caster.Transform.up;
		flying = true;
		Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		Vector3 point = Vector3.zero;
		if (Physics.Raycast(r, out hit))
		{
			point = hit.point;
		}
		dir = (point - Caster.Transform.position).normalized;
		caster.Transform.forward = dir;
		Destroy (p, 2);
		Destroy (gameObject, 2);
	}

	bool flying = false;
	void Update()
	{
		if (flying)
		{
			if (p == null)
			{
				flying = false;
				return;
			}
			p.transform.position += dir * speed * Time.deltaTime;
		}
	}
}
