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
				GameController.Instance.warriorSelected = null;
			}
            HexBattleStateMachine.Instance.battleState = HexBattleStateMachine.BattleState.SimpleSelect;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
			HexBattleStateMachine.Instance.SetSelectionArea(new List<Vector2>(){
				new Vector2(-1,1),
				new Vector2(1,-1),
				new Vector2(0,0),
				new Vector2(1,0),
				new Vector2(-1,0),
				new Vector2(0,1),
				new Vector2(0,-1),
			});
        }
	}
}
