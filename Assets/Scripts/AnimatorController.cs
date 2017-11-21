using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatorController : MonoBehaviour {

	private float speedMultiplyer = 0;

	public NavMeshAgent navAgent;
	public NavMeshAgent NavAgent
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

	private float speed = 1;
	private float Speed
	{
		get
		{
			return speed;
		}
		set
		{
			time = 0;
			speed = value;
		}
	}

	private float time;
	private Animator controller;
	private Animator Controller
	{
		get
		{
			if(!controller)
			{
				controller = GetComponentInChildren<Animator> ();
			}
			return controller;
		}
	}

	public void Atack()
	{
		Controller.SetTrigger ("Atack");
		Speed = 1;
	}

	public void Walk(bool value)
	{
		Controller.SetBool ("Walking", value);
		Speed = 1;
	}

	public void SpellCast()
	{
		Controller.SetTrigger ("SpellCast");
		Speed = 1;
	}

	public void SpellCastSelf()
	{
		Controller.SetTrigger ("SpellCastSelf");
		Speed = 1;
	}

	public void Die()
	{
		Controller.SetBool("Diying",true);
		Speed = 1;
	}

	public void SetSpeed(float speed)
	{
		Speed = speed;
	}

	void Update()
	{
		time += Time.deltaTime*2f;
		Controller.speed = Mathf.Lerp (Controller.speed, speed, time);
		Controller.SetFloat ("Speed", Controller.speed);

		float angle = Vector3.Angle(NavAgent.velocity.normalized, this.transform.forward);

		if (angle != 90) {
			if (NavAgent.velocity.normalized.x < this.transform.forward.x) {
				angle *= -1;
			}

			angle = (angle + 180.0f) % 360.0f;

			if (angle < 5) {
				//speedMultiplyer = 1;	
			} else {
				//speedMultiplyer = 0;
			}
			Controller.SetFloat ("Rotation", (180-angle)/90);
		} else {
			speedMultiplyer = 1;
			//Controller.SetFloat ("Rotation", 0);
		}

		if(NavAgent.hasPath && NavAgent.remainingDistance<=NavAgent.stoppingDistance)
		{
			speedMultiplyer = 1;
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation(NavAgent.pathEndPosition-transform.position), Time.deltaTime*NavAgent.angularSpeed/90);
		}

		//Controller.SetFloat ("Rotation", Quaternion.Angle(transform.rotation, Quaternion.LookRotation(transform.position - NavAgent.pathEndPosition, Vector3.up)));

		NavAgent.speed = Controller.speed*speedMultiplyer;
	}
}
