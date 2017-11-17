using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCellsTester : MonoBehaviour {


	public void SelectWarrior(BattleWarrior bw)
	{
		HexBattleStateMachine.Instance.SelectWarrior(bw);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			foreach(WarriorSelector ws in FindObjectsOfType<WarriorSelector>())
			{
				ws.Selected = false;
			}
            HexBattleStateMachine.Instance.battleState = HexBattleStateMachine.BattleState.SimpleSelect;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HexBattleStateMachine.Instance.SelectPointToAtack();
        }
	}
}
