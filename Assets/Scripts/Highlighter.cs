using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour {

	private void OnEnable()
	{
		GameController.Instance.warriorDepointed += Depointed;
		GameController.Instance.warriorPointed += Pointed;
	}

	private void OnDisable()
	{
		GameController.Instance.warriorDepointed -= Depointed;
		GameController.Instance.warriorPointed -= Pointed;
	}

	private void Depointed(BattleWarrior warrior)
	{
		if (warrior) 
		{
		    warrior.GetComponent<WarriorSelector> ().DeHighlight ();
		}
	}

	private void Pointed(BattleWarrior warrior)
	{
		if (warrior) 
		{
            Color color = Color.white;
            switch (warrior.type)
            {
                case BattleWarrior.WarriorType.Default:
                    color = Color.white;
                    break;
                case BattleWarrior.WarriorType.Player:
                    color = Color.green;
                    break;
                case BattleWarrior.WarriorType.Enemy:
                    color = Color.red;
                    break;
            }

            warrior.GetComponent<WarriorSelector> ().Highlight (color);
		}
	}
}
