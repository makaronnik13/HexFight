using System;
using UnityEngine;

[System.Serializable]
public class LayerRaycaster :MonoBehaviour
{
	public Action<Vector3, GameObject> OnRaycastHit;
	public Action OnRaycastMiss;

	public float raycastRate = 0.5f;
	public bool enableRaycast = true;

	public SingleUnityLayer layer;

	[NonSerialized]
	public float lastRate;
	[NonSerialized]
	public bool lastEnable;

	public void Init(float rate, bool enable, int layer)
	{
		raycastRate = rate;
		lastRate = rate;
		this.enableRaycast = enable;
		lastEnable = enable;
		this.layer = new SingleUnityLayer();
		this.layer.Set(layer);
	}

	public void StartRaycast()
	{
		InvokeRepeating("Raycast", 0, raycastRate);
	}

	public void StopRaycast()
	{
		CancelInvoke("Raycast");
	}

	private void Raycast()
	{
		if (OnRaycastHit!=null)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 10000, layer.Mask)) {
				OnRaycastHit.Invoke (hit.point, hit.collider.gameObject);
			} else {
				OnRaycastMiss.Invoke ();
			}
		}
	}
}