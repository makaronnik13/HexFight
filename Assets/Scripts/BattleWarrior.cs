using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BattleWarrior :MonoBehaviour
{
	public Action<int, int> OnApChanged;
	private AnimatorController controller;
	private AnimatorController Controller
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

	private NavMeshAgent navAgent;
	private NavMeshAgent NavAgent
	{
		get
		{
			if(!navAgent)
			{
				navAgent = GetComponent<NavMeshAgent> ();
			}
			return navAgent;
		}
	}

	public float Initiative = 3;

	[SerializeField]
	private int maxAp = 7;
	public int MaxAp
	{
		get
		{
			return maxAp;
		}
		set
		{
			maxAp = value;
			if (OnApChanged!=null) 
			{
				OnApChanged.Invoke(Ap, MaxAp);
			}
		}
	}

	private int ap = 7;
	public int Ap
	{
		get
		{
			return ap;
		}
		set
		{
			ap = value;
			if (OnApChanged!=null) 
			{
				OnApChanged.Invoke(Ap, MaxAp);
			}
		}
	}

	public bool Enemy = false;

	public Sprite portrait;
	public Sprite smallPortrait;

	private List<Vector2> cellsSize = new List<Vector2>(){new Vector2(0,0)};
	public List<Cell> cells = new List<Cell>();

	private Stack<Vector3> pathQueue = new Stack<Vector3> ();


    public int CurrentWalkRange
    {
        get
        {
			return Mathf.FloorToInt(Ap);
        }
    }

	public void GoByPath(List<Vector3> path)
	{
		if (path.Count == 1) {
			return;
		} else {
			path.RemoveAt (path.Count-1);
		}
		pathQueue = new Stack<Vector3> ();
		foreach(Vector3 p in path)
		{
			pathQueue.Push (p);
		}

		GoToNextPoint ();
	}

	void Update()
	{
		CheckEndOfPath ();
	}

	private void CheckEndOfPath()
	{
		if (NavAgent.hasPath && Vector3.Distance (transform.position, NavAgent.pathEndPosition) < 0.1f) {
			GoToNextPoint ();
		}
	}

	private void GoToNextPoint()
	{
		if (pathQueue.Count > 0) {
			Ap-=1;
			Vector3 aim = pathQueue.Pop ();
			NavAgent.SetDestination (aim);
			NavMeshPath path = new NavMeshPath ();
			NavAgent.CalculatePath (aim, path);
			Controller.Walk (true);
		} else {
			NavAgent.ResetPath ();
			Controller.Walk (false);
		}
	}

	private void Start()
	{
		Ap = MaxAp;
	}
}

