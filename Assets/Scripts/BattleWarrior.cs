using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class BattleWarrior :MonoBehaviour
{
	public Action<int, int> OnHpChanged, OnMpChanged, OnApChanged;
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

	[SerializeField]
	private int maxHp = 7;
	public int MaxHp
	{
		get
		{
			return maxHp;
		}
		set
		{
			maxHp = value;
			if (OnHpChanged!=null) 
			{
				OnHpChanged.Invoke(Hp, MaxHp);
			}
		}
	}
		
	private int hp = 100;
	public int Hp
	{
		get
		{
			return hp;
		}
		set
		{
			hp = value;
			if (OnHpChanged!=null) 
			{
				OnHpChanged.Invoke(Hp, MaxHp);
			}
		}
	}

	[SerializeField]
	private int maxMp = 7;
	public int MaxMp
	{
		get
		{
			return maxMp;
		}
		set
		{
			maxMp = value;
			if (OnMpChanged!=null) 
			{
				OnMpChanged.Invoke(Mp, MaxMp);
			}
		}
	}

	private int mp = 10;
	public int Mp
	{
		get
		{
			return mp;
		}
		set
		{
			mp = value;
			if (OnMpChanged!=null) 
			{
				OnMpChanged.Invoke(Mp, MaxMp);
			}
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

    public float WalkSpeed = 1;

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
			return Mathf.FloorToInt(Ap*WalkSpeed);
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
			Ap-=Mathf.CeilToInt(1/WalkSpeed);
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
		Hp = MaxHp;
		Ap = MaxAp;
		Mp = MaxMp;
	}
}

