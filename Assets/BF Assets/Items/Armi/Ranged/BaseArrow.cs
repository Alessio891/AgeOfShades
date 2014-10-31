using UnityEngine;
using System.Collections;

public class BaseArrow : MonoBehaviour {

	public Vector3 direction = Vector3.one;
	public float speed = 10;

	bool flying = false;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 4);
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "SimpleEnemy")
		{
			c.GetComponent<BasicEntity>().Damage(1);
			Destroy(gameObject);
		}

	}

	// Update is called once per frame
	void Update () {
		if (flying)
		{
			transform.position += direction * speed * Time.deltaTime;
			transform.LookAt(transform.position + direction);
		}
	}

	//void OnTriggerEnter(

	public void Shoot()
	{
		flying = true;
	}
}
