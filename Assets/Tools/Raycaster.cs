using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Raycaster : Singleton<Raycaster>
{

	public LayerRaycaster[] raycasters;

	public void RemoveRaycaster(int layerId,  LayerRaycaster.InputType inputType, int buttonId)
	{
		LayerRaycaster lr = raycasters.ToList ().Find (r => r.layer.LayerIndex == layerId && r.inputType == inputType && r.mouseButtonId == buttonId);
		if(lr!=null)
		{
			raycasters = raycasters.Where(val => val != lr).ToArray();
			Destroy (lr);
		}
	}

	public void AddListener (Action<Vector3, GameObject> action, Action missAction, int layerId, float rate = 0.1f, LayerRaycaster.InputType inputType = LayerRaycaster.InputType.none, int buttonId = 0)
	{
		LayerRaycaster lr = raycasters.ToList ().Find (r => r.layer.LayerIndex == layerId && r.inputType == inputType && r.mouseButtonId == buttonId);
		if (lr == null) {
			LayerRaycaster newRaycaster = gameObject.AddComponent<LayerRaycaster> ();
			newRaycaster.Init (rate, true, layerId, inputType, buttonId); 

			Array.Resize (ref raycasters, raycasters.Count () + 1);

			raycasters [raycasters.Count () - 1] = newRaycaster;

			newRaycaster.OnRaycastHit += action;
			newRaycaster.OnRaycastMiss += missAction;

			newRaycaster.lastRate = newRaycaster.raycastRate;
			newRaycaster.lastEnable = newRaycaster.enableRaycast;
			if (newRaycaster.enableRaycast) {
				newRaycaster.StartRaycast ();
			}

		} else {
			lr.OnRaycastHit += action;
			lr.OnRaycastMiss += missAction;
		}
	}

	// Use this for initialization
	void Start ()
	{
		foreach (LayerRaycaster lr in raycasters) {
			lr.lastRate = lr.raycastRate;
			lr.lastEnable = lr.enableRaycast;
			if (lr.enableRaycast) {
				lr.StartRaycast ();
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (LayerRaycaster lr in raycasters) {
			if (lr.raycastRate != lr.lastRate || lr.lastEnable != lr.enableRaycast) {
				lr.lastRate = lr.raycastRate;
				lr.lastEnable = lr.enableRaycast;
				lr.StopRaycast ();
				lr.StartRaycast ();
			}	

			if (lr.inputType == LayerRaycaster.InputType.mouseDown && Input.GetMouseButtonDown (lr.mouseButtonId)) 
			{
				lr.InputTriggered = true;
			}

			if (lr.inputType == LayerRaycaster.InputType.mouseUp && Input.GetMouseButtonUp (lr.mouseButtonId)) {
				lr.InputTriggered = true;
			}

			if (lr.inputType == LayerRaycaster.InputType.mouseHold)
			{
				lr.Hold = Input.GetMouseButton (lr.mouseButtonId);
			}

			
			if(lr.inputType == LayerRaycaster.InputType.mouseDoubleClick && Input.GetMouseButtonDown (lr.mouseButtonId))
			{
				if (lr.DoubleClicked) {
					lr.InputTriggered = true;
					lr.DoubleClicked = false;
				} else 
				{
					lr.InputTriggered = false;
					lr.DoubleClicked = true;
				}
			}
			
		}
	}
}
