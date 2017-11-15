using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeController : MonoBehaviour {

	public int raycastLayer = 9;

	private Vector3 aim = Vector3.one*Mathf.Infinity;
	private AnimatorController controller
	{
		get
		{
			return GetComponent<AnimatorController> ();
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

	// Use this for initialization
	void Start () {
		Raycaster.Instance.AddListener ((Vector3 point, GameObject rayHitObject)=>{
			Raycast(point, rayHitObject);
		}, ()=>{},raycastLayer);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			if(aim!= Vector3.one*Mathf.Infinity)
			{
				NavAgent.SetDestination (aim);
				NavMeshPath path = new NavMeshPath ();
				NavAgent.CalculatePath (aim, path);
				controller.Walk (true);
			}
		}

		if(Input.GetMouseButtonDown(1))
		{
			controller.Atack ();
		}

		if(Input.GetKeyDown(KeyCode.Q))
		{
			controller.SpellCast ();
		}

		if(Input.GetKeyDown(KeyCode.W))
		{
			controller.SpellCastSelf ();
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			controller.Die();
		}
		CheckEndOfPath ();
	}

	private void CheckEndOfPath()
	{
		if(NavAgent.hasPath && Vector3.Distance(transform.position, NavAgent.pathEndPosition)<0.1f)
		{
			controller.Walk (false);
		}
	}

	private void Raycast(Vector3 point, GameObject rayHitobject)
	{
		aim = point;
	}
}
