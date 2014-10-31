using UnityEngine;
using System.Collections;

public class SmotthFollow : MonoBehaviour {

	/*
This camera smoothes out rotation around the y-axis and height.
Horizontal Distance to the target is always fixed.

There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

For every of those smoothed values we calculate the wanted value and the current value.
Then we smooth it using the Lerp function.
Then we apply the smoothed values to the transform's position.
*/
	
	// The target we are following
	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 8.0f;
	// How much we 
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public bool rotating = false;
	public Vector3 facing = Vector3.forward;

	public bool Debugging = false;

	bool PlayerLost = false;
	Vector3 LastCameraPosition;
	// Place the script in the Camera-Control group in the component menu		
	bool CanSeePlayer {
		get {
			Ray r = new Ray(transform.position, (target.transform.position - transform.position).normalized);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit))
			{
				if (hit.collider.tag == "BlockView")
				{
					return false;
				}
			}
			return true;
		}
	}

	bool PlayerCanBeSeenFromDefaultPosition
	{
		get {
			Ray r = new Ray(CameraInDefaultPosition, (target.transform.position - CameraInDefaultPosition).normalized);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit))
			{
				if (hit.collider.tag == "BlockView")
				{
					return false;
				}
			}
			return true;
		}
	}

	void LateUpdate () {
		// Early out if we don't have a target
		if (!target)
			target = GameHelper.GetLocalPlayer().transform;

		if (!PlayerCanBeSeenFromDefaultPosition) {
			//if (!PlayerLost)
			//{
			///	LastCameraPosition = Camera.main.transform.position;
			//	PlayerLost = true;
				height = 16;
			//}

			Debug.Log("NON VEDO IL PLAYER");
		} 
		else
		{
			height = 10;			
		}

		// Calculate the current rotation angles
		var wantedRotationAngle = target.eulerAngles.y;
		var wantedHeight = target.position.y + height;
		
		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= facing * distance;
		
		// Set the height of the camera
		Vector3 tempPos = transform.position;
		tempPos.y = currentHeight;
		transform.position = tempPos;
		
		// Always look at the target
		if (!rotating && !Debugging)
		{
			transform.LookAt (target);
		}
	}

	Vector3 CameraInDefaultPosition
	{
		get
		{
			Vector3 pos = target.position;
			pos -= facing * distance;
			var wantedHeight = target.position.y + 10;
			// Set the height of the camera

			pos.y = wantedHeight;

			return pos;
		}
	}

	Vector3 tempFace = Vector3.zero;
	public void SetFacingDirection( Vector3 face, float speed )
	{
		StartCoroutine (rotateTo(face, speed));
	}

	IEnumerator rotateTo(Vector3 dir, float speed)
	{
		while(!facing.AlmostEquals(dir, 0.001f))
		{
			facing = Vector3.Slerp(facing, dir, 0.7f * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		facing = dir;
		yield return true;
	}
}
