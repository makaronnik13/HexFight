using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : Singleton<Raycaster> {

    public float raycastRate = 0.5f;
    public bool enableRaycast = true;

    public SingleUnityLayer layer;
    private float lastRate;
    private bool lastEnable;

    public Action<Vector3, GameObject> OnRaycastHit;

	// Use this for initialization
	void Start () {
        lastRate = raycastRate;
        lastEnable = enableRaycast;
        if (enableRaycast)
        {
            InvokeRepeating("Raycast", 0, raycastRate);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (raycastRate!=lastRate || lastEnable != enableRaycast)
        {
            lastRate = raycastRate;
            lastEnable = enableRaycast;
            CancelInvoke("Raycast");
            InvokeRepeating("Raycast", 0, raycastRate);
        }
	}

    private void Raycast()
    {
        if (OnRaycastHit!=null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10000, layer.Mask))
            {
                OnRaycastHit.Invoke(hit.point, hit.collider.gameObject);
            }
        }
        
    }
}
