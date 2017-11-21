using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour {

	private void OnEnable()
	{
		GameController.Instance.warriorDepointed += Depointed;
		GameController.Instance.warriorSelected += Selected;
		GameController.Instance.warriorPointed += Pointed;
	}

	private void OnDisable()
	{
		GameController.Instance.warriorDepointed -= Depointed;
		GameController.Instance.warriorSelected -= Selected;
		GameController.Instance.warriorPointed -= Pointed;
	}

	private void Depointed(BattleWarrior warrior)
	{
		if (warrior) 
		{
				warrior.GetComponent<WarriorSelector> ().DeHighlight ();
		}
	}

	private void Selected(BattleWarrior warrior)
	{
		if (warrior) {
			Color color = Color.green;
			if (warrior.Enemy) {
				color = Color.red;
			}
		
			warrior.GetComponent<WarriorSelector> ().Highlight (color);
		}
	}

	private void Pointed(BattleWarrior warrior)
	{
		if (warrior) 
		{
			Color color = Color.green;
			if (warrior.Enemy) {
				color = Color.red;
			}
			warrior.GetComponent<WarriorSelector> ().Highlight (color);
		}
	}
}
