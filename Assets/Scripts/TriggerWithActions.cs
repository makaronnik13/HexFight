using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerWithActions : MonoBehaviour {

	public UnityEvent onEnter;

	void OnTriggerEnter(Collider c)
	{
		if(onEnter!=null && c.GetComponent<BattleWarrior>() && c.GetComponent<BattleWarrior>().type == BattleWarrior.WarriorType.Player)
		{
			onEnter.Invoke();
		}
	}
}
