using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFollower : MonoBehaviour {

	private FormationType formationType;
	private int formationIndex;
	private int formationSize;
	private float formationScaleFactor; 
	private FakeController controller;
	public FakeController Controller
	{
		get
		{
			if(!controller)
			{
				controller = GetComponent<FakeController> ();
			}
			return controller;
		}
	}

	private Vector3 AimPosition
	{
		get
		{
			Vector3 formationOffset = Vector3.zero;

			switch(formationType)
			{
			case FormationType.Grid:
				formationOffset = - new Vector3(0,0,formationIndex+1); //Mathf.RoundToInt( Mathf.Sqrt(formationSize)
				break;
			case FormationType.Line:
				formationOffset =  -new Vector3(0,0,formationIndex+1);
				break;
			case FormationType.Pyramide:
				float raw = Mathf.FloorToInt (Mathf.Sqrt (2 * (formationIndex + 1) + 0.25f) - 0.5f);
				float column = (formationIndex+1) - raw*(raw+1)/2;
				formationOffset = -new Vector3 (-0.5f*raw+column,0, raw);
				break;
			}

			Vector3 p = GameController.Instance.Warrior.transform.position + formationOffset * formationScaleFactor;

			return RotatePointAroundPivot (p, GameController.Instance.Warrior.transform.position, Vector3.up*GameController.Instance.Warrior.transform.rotation.eulerAngles.y);//RotatePointAroundPivot(formationOffset*formationScaleFactor, GameController.Instance.Warrior.transform.position, (GameController.Instance.Warrior.transform.rotation.y+90)*Vector3.forward);
		}
	}

	private void Update()
	{
		float distance = Vector3.Distance (transform.position, AimPosition);
		if(distance>Controller.Controller.NavAgent.stoppingDistance)
		{
			NavMeshHit nmh;
			Controller.Controller.NavAgent.Raycast (AimPosition, out nmh);

			if(nmh.hit)
			{
				GetComponent<FakeController> ().GoTo (AimPosition, distance>formationScaleFactor/2);
			}
			else
			{
				GetComponent<FakeController> ().GoTo (nmh.position, distance>formationScaleFactor);
			}
		}
	}

	public void Init(int formationIndex, int formationSize, FormationType formationType, float formationScaleFactor = 1)
	{
		this.formationIndex = formationIndex;
		this.formationType = formationType;
		this.formationScaleFactor = formationScaleFactor;
		this.formationSize = formationSize;
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}
}
