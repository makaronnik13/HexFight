using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

	public GameObject aim;
	public float height;
	[Range(0, 90)]
	public float angle = 45;
	private Vector3 aimPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (GameController.Instance.Mode)
		{
		case GameController.GameMode.Adventure:
			if(!aim)
			{
				return;
			}
			aimPosition = aim.transform.position + Vector3.up * height-Vector3.forward*(height/Mathf.Cos(angle*Mathf.Deg2Rad));
			transform.position = Vector3.Lerp (transform.position, aimPosition, Time.deltaTime*3);
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(new Vector3(angle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)), Time.deltaTime);
			break;
		case GameController.GameMode.Battle:
			GetComponent<Camera> ().fieldOfView -= Input.mouseScrollDelta.y;
			GetComponent<Camera> ().fieldOfView = Mathf.Clamp (GetComponent<Camera> ().fieldOfView, 3, 45);
			if (Input.GetMouseButton (1)) {
				transform.RotateAround (transform.position + transform.forward * transform.position.y, Vector3.up, Input.GetAxis ("Mouse X"));
			}
			Vector3 forward = new Vector3 (transform.forward.x, 0, transform.forward.z);
			transform.position += forward * Input.GetAxis ("Vertical");
			transform.position += Quaternion.Euler(Vector3.up*90) * forward * Input.GetAxis ("Horizontal");
			break;
		}
	}

	public void SetAim(MonoBehaviour aim)
	{
		if(aim)
		{
			this.aim = aim.gameObject;
		}
	}

	void OnEnable()
	{
		GameController.Instance.warriorSelected += SetAim;
	}

	void OnDisable()
	{
		if (GameController.Instance) 
		{
			GameController.Instance.warriorSelected -= SetAim;	
		}
	}
}
