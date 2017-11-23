using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

	public GameObject aim;
	public float height;
	[Range(0, 90)]
	public float angle = 45;
	private Vector3 aimPosition;
    public float Boundary = 5;
    public float Speed = 5;

    private float aimFow = 0;

	// Use this for initialization
	void Start () {
        aimFow = Camera.main.fieldOfView;
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
                aimFow -= Input.mouseScrollDelta.y * Speed * Time.deltaTime * 25;
                aimFow = Mathf.Clamp(aimFow, 3, 45);
                GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, aimFow, Time.deltaTime * 2);
                Vector3 beforePosition = transform.position;

                if (Input.GetMouseButton(1))
                {
                    transform.RotateAround(transform.position + transform.forward * transform.position.y, Vector3.up, Input.GetAxis("Mouse X") * Speed * Time.deltaTime * 20);
                }
                Vector3 forward = new Vector3 (transform.forward.x, 0, transform.forward.z);
			    transform.position += forward * Input.GetAxis ("Vertical") * Speed * Time.deltaTime;
			    transform.position += Quaternion.Euler(Vector3.up*90) * forward * Input.GetAxis ("Horizontal") * Speed * Time.deltaTime;

                if (Input.GetMouseButton(2))
                {
                    transform.position -= forward * Input.GetAxis("Mouse Y") * Speed * Time.deltaTime * 5;
                    transform.position -= Quaternion.Euler(Vector3.up * 90) * forward * Input.GetAxis("Mouse X") * Speed * Time.deltaTime * 5;
                }

                if (Input.mousePosition.x > Screen.width - Boundary)
                {
                    transform.position +=  Quaternion.Euler(Vector3.up * 90) * forward * Time.deltaTime * Speed * 2;
                }

                if (Input.mousePosition.x < 0 + Boundary)
                {
                    transform.position -= Quaternion.Euler(Vector3.up * 90) * forward * Time.deltaTime * Speed * 2;
                }

                if (Input.mousePosition.y > Screen.height - Boundary)
                {
                    transform.position += forward * Time.deltaTime * Speed * 2;
                }

                if (Input.mousePosition.y < 0 + Boundary)
                {
                    transform.position -= forward * Time.deltaTime * Speed * 2;
                }

                if (Vector3.Distance(GameController.Instance.CurrentField.transform.position, transform.position) > GameController.Instance.CurrentField.Radius)
                {
                    transform.position = beforePosition;
                }
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
