using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeController : MonoBehaviour {

	public float WalkSpeed = 1;
	public float RunSpeed = 3;

	public bool inputEnabled = false;

	public bool InputEnabled
	{
		get
		{
			return inputEnabled;
		}
		set
		{
			inputEnabled = value;

		}
	}
		
	private AnimatorController controller;
	public AnimatorController Controller
	{
		get
		{
			if(!controller)
			{
				controller = GetComponent<AnimatorController> ();
			}
			return controller;
		}
	}

	void Update () 
	{
		CheckEndOfPath ();
	}


	public void CheckEndOfPath()
	{
		if(Controller.NavAgent.hasPath && Controller.NavAgent.remainingDistance<=Controller.NavAgent.stoppingDistance)
		{
			Controller.SetSpeed (1);
		}
	}

	public void GoTo(Vector3 point, bool run = false)
	{
		NavMeshPath path = new NavMeshPath ();
		Controller.NavAgent.CalculatePath (point, path);
		if (path.status != NavMeshPathStatus.PathPartial) 
		{
			Controller.NavAgent.SetDestination (point);
			if (run) {
				Run ();
			} else {
				Walk ();
			}
		}

	}
		
	public void Run()
	{
		if(Controller.NavAgent.path!=null && Controller.NavAgent.path.status != NavMeshPathStatus.PathPartial)
		{
			Controller.SetSpeed (RunSpeed);
		}
	}
	public void Walk()
	{
		if(Controller.NavAgent.path!=null && Controller.NavAgent.path.status != NavMeshPathStatus.PathPartial)
		{
			Controller.SetSpeed (WalkSpeed);
		}
	}
}
