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
		if(!aim)
		{
			return;
		}
		aimPosition = aim.transform.position + Vector3.up * height-Vector3.forward*(height/Mathf.Cos(angle*Mathf.Deg2Rad));
		transform.position = Vector3.Lerp (transform.position, aimPosition, Time.deltaTime*3);
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(new Vector3(angle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)), Time.deltaTime);
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
