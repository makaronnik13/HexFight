using System;
using UnityEngine;

[System.Serializable]
public class LayerRaycaster :MonoBehaviour
{
	private const float doubleClickTreshold = 0.3f;
	private const float holdTreshold = 0.3f;

	public enum InputType
	{
		none,
		mouseUp,
		mouseDown,
		mouseHold,
		mouseDoubleClick
	}

	public int mouseButtonId;

	private bool doubleClicked = false;
	public bool DoubleClicked
	{
		get
		{
			return doubleClicked;
		}
		set
		{
			doubleClicked = value;
			Invoke ("ResetDoubleClicked", doubleClickTreshold);
		}
	}

	private bool hold = false;
	public bool Hold
	{
		get
		{
			return hold;
		}
		set
		{
			hold = value;
			if(!hold)
			{
				CancelInvoke ("ResetHold");
				InputTriggered = false;
			}
			Invoke ("ResetHold", holdTreshold);
		}
	}

	public InputType inputType = InputType.none;

	public Action<Vector3, GameObject> OnRaycastHit;
	public Action OnRaycastMiss;

	public float raycastRate = 0.5f;
	public bool enableRaycast = true;

	public bool InputTriggered = false;

	public SingleUnityLayer layer;


	[NonSerialized]
	public float lastRate;
	[NonSerialized]
	public bool lastEnable;

	public void Init(float rate, bool enable, int layer, InputType inputType = InputType.none, int buttonId = 0)
	{
		this.mouseButtonId = buttonId;
		this.inputType = inputType;
		if(inputType == InputType.none)
		{
			InputTriggered = true;
		}
		Init (rate, enable, layer);
	}


	public void Init(float rate, bool enable, int layer)
	{
		raycastRate = rate;
		lastRate = rate;
		this.enableRaycast = enable;
		lastEnable = enable;
		this.layer = new SingleUnityLayer();
		this.layer.Set(layer);
	}

	private void ResetDoubleClicked()
	{
		doubleClicked = false;
	}

	private void ResetHold()
	{
		hold = false;
		InputTriggered = true;
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



			if (InputTriggered || inputType == InputType.none) {
				if (Physics.Raycast (ray, out hit, 10000)) 
				{
					if (hit.collider.gameObject.layer == layer.LayerIndex) {
						OnRaycastHit.Invoke (hit.point, hit.collider.gameObject);
					} else {
						OnRaycastMiss.Invoke ();
					}
				} else {
					OnRaycastMiss.Invoke ();
				}
				InputTriggered = false;
			}
			}
	}
}
	