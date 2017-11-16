using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCellsTester : MonoBehaviour {

    public BattleWarrior warrior;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HexBattleStateMachine.Instance.battleState = HexBattleStateMachine.BattleState.SimpleSelect;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HexBattleStateMachine.Instance.SelectPointToAtack(warrior);
        }
	}
}
