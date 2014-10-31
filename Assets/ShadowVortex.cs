using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowVortex : MonoBehaviour {

	public List<GameObject> Spawnable;
	public float Rate;

	float timer = 0;

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystem> ().Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M))
		{
			SpawnCreature();
		}
	}

	public void SpawnCreature()
	{
		if (Spawnable.Count <= 0)
		{
			Debug.LogError("ShadowVortex: La lista dei possibili spawn è vuota.");
			return;
		}

		// Mostra particelle prima di spawnare
		
		GetComponentInChildren<ParticleSystem> ().Play ();
		GetComponent<ParticleSystem> ().Stop (false);
		GameObject spawned = Instantiate (Spawnable [Random.Range (0, Spawnable.Count - 1)]) as GameObject;
		spawned.GetComponent<NavMeshAgent> ().enabled = false;
		spawned.transform.position = transform.position;
		iTween.FadeFrom (spawned, iTween.Hash ("amount", 0, "time", 2));
		//StartCoroutine (WaitForParticlesToEnd (GetComponentInChildren<ParticleSystem> ().duration));
	}

	IEnumerator WaitForParticlesToEnd(float time)
	{
		yield return new WaitForSeconds (time);

		


	}

}
