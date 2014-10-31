using UnityEngine;
using System.Collections;

public class PlayerFriend : BasicNPC {

	protected override void Init ()
	{
		
		if (GameHelper.GameIsLoading)
			return;

		base.Init ();


		Behaviours.Add (new ScreamOnHostile (gameObject));	
	}

	
	public override void OnLoadingFinished()
	{
		base.OnLoadingFinished ();
		//GetComponent<NavMeshAgent> ().Stop ();
		Behaviours.Add (new FollowPlayer (gameObject));
	}

	public override object[] Serialize()
	{
		object[] data = SerializeBasicAttributes ().ToArray ();

		return data;
	}

	public override void DeSerialize(object[] data)
	{
		base.DeSerialize (data);
	}


}
