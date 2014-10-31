using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Draggable : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler */{
	public Transform target;
	private bool isMouseDown = false;
	private Vector3 startMousePosition;
	private Vector3 startPosition;
	public bool shouldReturn;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void OnPointerDown(PointerEventData dt) {
		isMouseDown = true;
		
		Debug.Log ("Draggable Mouse Down");
		Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
		Vector3 pos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle (GetComponent<RectTransform> (), Input.mousePosition, c, out pos);

		startPosition = target.position;
		startMousePosition = pos;
	}
	
	public void OnPointerUp(PointerEventData dt) {
		Debug.Log ("Draggable mouse up");
		
		isMouseDown = false;
		
		if (shouldReturn) {
			target.position = startPosition;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isMouseDown) {
			Camera c = GameObject.Find ("GlobalUICamera").GetComponent<Camera> ();
			Vector3 mpos;
			RectTransformUtility.ScreenPointToWorldPointInRectangle (GetComponent<RectTransform> (), Input.mousePosition, c, out mpos);
			Vector3 currentPosition = mpos;
			
			Vector3 diff = currentPosition - startMousePosition;
			
			Vector3 pos = startPosition + diff;
			
			target.position = pos;
		}
	}
}