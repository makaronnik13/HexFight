using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointerTrigger : MonoBehaviour {

	public UnityEvent onEner, onExit;

	void OnMouseEnter() {
		Debug.Log ("enter");
		if(onEner!=null)
		{
			onEner.Invoke ();
		}
	}

	void OnMouseExit() {
		if(onExit!=null)
		{
			onExit.Invoke ();
		}
	}
}
