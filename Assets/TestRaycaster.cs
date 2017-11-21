using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycaster : MonoBehaviour {

	public float minOffset, maxOffset; 
	public LayerMask recievingRaycastLayer;
	public LayerMask obstaclesLayers;

	// Use this for initialization
	void Start () {
		Raycast (minOffset, maxOffset, recievingRaycastLayer.value , obstaclesLayers.value);
	}
	
	// Update is called once per frame
	void Update () {
		Raycast (minOffset, maxOffset, recievingRaycastLayer.value , obstaclesLayers.value);
	}

	private bool Raycast(float minOffset, float maxOffset, LayerMask obstacleLayer, LayerMask raycastingLayer)
	{
		RaycastHit hit;

		if(Physics.Raycast(transform.position, Vector3.down, out hit, 1000, obstacleLayer.value|raycastingLayer.value))
		{
			Debug.DrawRay (transform.position, -transform.position+hit.point, Color.red);


			return !Contains (raycastingLayer, hit.collider.gameObject.layer);
		}

		return false;
	}

	private bool Contains(LayerMask mask, int layer)
	{
		return mask == (mask | (1 << layer));
	}
}
