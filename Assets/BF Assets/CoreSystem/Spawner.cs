using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public GameObject Spawn;
	public float Timer = 2;
	public int Quantita = 1;
	public int QuantitaMassima = 3;

	public bool SpawnOnStart = false;
	public bool AreaSpawn = false;
	public Vector2 minRect = Vector2.zero;
	public Vector2 maxRect = Vector2.zero;

	Vector2 MinRect { get { return (new Vector2 (transform.position.x - minRect.x, transform.position.z + minRect.y)); } }
	Vector2 MaxRect { get { return (new Vector2 (transform.position.x + maxRect.x, transform.position.z - maxRect.y)); } }

	public bool SpawnaEdElimina = false;

	public List<GameObject> SpawnedHere = new List<GameObject>();



	float _timer = 0;

	// Use this for initialization
	void Start () {
		if (SpawnOnStart)
		{
			for(int j = 0; j < QuantitaMassima; j++)
			{
				for(int i = 0; i < Quantita; i++)
				{
					Vector3 pos = new Vector3();
					if (AreaSpawn)
					{
						pos.x = Random.Range(MinRect.x, MaxRect.x);
						pos.z = Random.Range(MinRect.y, MaxRect.y);
						pos.y = Terrain.activeTerrain.SampleHeight( pos );
					}
					else
					{
						pos = transform.position;
					}
					GameObject newSpawn = PhotonNetwork.Instantiate(Spawn.name, pos, Quaternion.identity, 0 );
					SpawnedHere.Add(newSpawn);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.inRoom)
		{
			if (!PhotonNetwork.isMasterClient)
			{
				return;
			}
		}

		if (!PhotonNetwork.inRoom)
			return;
		Vector3 center = new Vector3 (MinRect.x + (MaxRect.x / 2), 10, MinRect.y + (MaxRect.y / 2));

		if (Spawn == null)
			return;

		for(int j = 0; j < SpawnedHere.Count; j++)
		{
			if (SpawnedHere[j] == null)
			{
				SpawnedHere.Remove(SpawnedHere[j]);
				j--;
			}
		}

		if (SpawnedHere.Count >= QuantitaMassima)
			return;

		_timer += Time.deltaTime;
		if (_timer > Timer)
		{
			for(int i = 0; i < Quantita; i++)
			{
				Vector3 pos = new Vector3();
				if (AreaSpawn)
				{
					pos.x = Random.Range(MinRect.x, MaxRect.x);
					pos.z = Random.Range(MinRect.y, MaxRect.y);
					pos.y = Terrain.activeTerrain.SampleHeight( pos );
				}
				else
				{
					pos = transform.position;
				}
				GameObject newSpawn = PhotonNetwork.Instantiate(Spawn.name, pos, Quaternion.identity, 0) as GameObject;

//				newSpawn.transform.position = pos;
				SpawnedHere.Add(newSpawn);
			}
			_timer = 0;
			if (SpawnaEdElimina)
				Destroy(gameObject);
		}
	
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (new Vector3(MinRect.x, 0.1f, MinRect.y),new Vector3( MinRect.x, 0.1f, MaxRect.y));
		Gizmos.DrawLine (new Vector3(MinRect.x, 0.1f, MinRect.y),new Vector3( MaxRect.x, 0.1f, MinRect.y));
		Gizmos.DrawLine (new Vector3(MaxRect.x, 0.1f, MinRect.y),new Vector3( MaxRect.x, 0.1f, MaxRect.y));
		Gizmos.DrawLine (new Vector3(MaxRect.x, 0.1f, MaxRect.y),new Vector3( MinRect.x, 0.1f, MaxRect.y));
	}
}
