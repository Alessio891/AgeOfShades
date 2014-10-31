using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerLockSystem : MonoBehaviour {

	public GameObject LockOn;
	public GameObject Target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (LockOn != null)
		{
		
			Vector3 relPos = LockOn.transform.position - transform.position;
			Quaternion lookRot = Quaternion.LookRotation(relPos);
			Vector3 eulerAngles = lookRot.eulerAngles;
			eulerAngles = new Vector3(0, eulerAngles.y, 0);
			lookRot.eulerAngles = eulerAngles;
			//transform.rotation = Quaternion.Slerp(transform.rotation, , 3f * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 4 * Time.deltaTime);
			//GameHelper.GetLocalPlayer().transform.LookAt(LockOn.transform.position);
		}
		else
		{
		
		}

		RaycastHit hit;
		Ray r = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2));
		bool target = false;
		if (Physics.Raycast(r, out hit))
		{
			if (hit.collider.GetComponent<BasicEntity>() != null)
			{
				Target = hit.collider.gameObject;
				target = true;
			}
		}
		if (!target)
			Target = null;

	}

	void Update()
	{


		if (LockOn == null && GameObject.Find("TargetIcon") != null && GameObject.Find("TargetIcon").renderer.enabled)
		{
			GameObject.Find("TargetIcon").renderer.enabled = false;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			LockOn = null;
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			List<GameObject> enemies = (from o in GameObject.FindGameObjectsWithTag("SimpleEnemy") where isInFront(o) orderby Vector3.Distance(transform.position, o.transform.position) select o).ToList();
			enemies.AddRange ( (from o in GameObject.FindGameObjectsWithTag("Animale") where isInFront(o) orderby Vector3.Distance(transform.position, o.transform.position) select o).ToList());
			if (enemies.Count <= 0)
				return;
			int index = 0;
			if (LockOn == null)
			{
				LockOn = enemies[0];
				PlaceTargetIcon();

			}
			else
			{
				while(enemies[index].GetInstanceID() == LockOn.GetInstanceID())
				{
					index++;
					if (index >= enemies.Count - 1)
						break;
				}
				if (index >= enemies.Count)
					index = enemies.Count-1;
				LockOn = enemies[index];
				PlaceTargetIcon();
			}
		}
	}

	void PlaceTargetIcon()
	{
		GameObject targetIcon = GameObject.Find("TargetIcon");
		if (!targetIcon.renderer.enabled)
			targetIcon.renderer.enabled = true;
		targetIcon.transform.parent = LockOn.transform;
		targetIcon.transform.localPosition = Vector3.zero;
		Vector3 pos = targetIcon.transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight (pos) + 0.5f;
		targetIcon.transform.position = pos;

	}

	bool isInFront(GameObject obj){
		return Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(obj.transform.position)) > 0;
	}
}
