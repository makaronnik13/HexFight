using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Raycaster : Singleton<Raycaster> {

	public LayerRaycaster[] raycasters;

	public void AddListener(Action<Vector3, GameObject> action, Action missAction, int layerId, float rate = 0.1f)
	{
		LayerRaycaster lr = raycasters.ToList ().Find (r => r.layer.LayerIndex == layerId);
		if (lr == null) {
			LayerRaycaster newRaycaster = gameObject.AddComponent<LayerRaycaster> ();
			newRaycaster.Init (rate, true, layerId); 

			Array.Resize (ref raycasters, raycasters.Count() + 1);

			raycasters [raycasters.Count() - 1] = newRaycaster;

			newRaycaster.OnRaycastHit += action;
			newRaycaster.OnRaycastMiss += missAction;
		} else {
			lr.OnRaycastHit += action;
			lr.OnRaycastMiss += missAction;
		}
	}

	// Use this for initialization
	void Start () {
		foreach(LayerRaycaster lr in raycasters)
		{
			lr.lastRate = lr.raycastRate;
			lr.lastEnable = lr.enableRaycast;
			if (lr.enableRaycast)
			{
				lr.StartRaycast ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach(LayerRaycaster lr in raycasters)
		{
			if (lr.raycastRate!=lr.lastRate || lr.lastEnable != lr.enableRaycast)
			{
				lr.lastRate = lr.raycastRate;
				lr.lastEnable = lr.enableRaycast;
				lr.StopRaycast ();
				lr.StartRaycast ();
			}	
		}
	}
}
