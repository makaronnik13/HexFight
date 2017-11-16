using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBattleStateMachine : Singleton<HexBattleStateMachine> {

	private BattleWarrior warrior;
	private BattleState _battleState = BattleState.SimpleSelect;
    public BattleState battleState
    {
        get
        {
            return _battleState;
        }

        set
        {
            _battleState = value;

        }
    }
	public enum BattleState
	{
		SimpleSelect,
		MoveAtackSelect,
		AreaSelect
	}

	public void SelectWarrior(BattleWarrior warrior)
	{
		this.warrior = warrior;
		if(warrior)
		{
			battleState = BattleState.MoveAtackSelect;
		}
	}

    public void SelectPointToAtack(BattleWarrior warrior)
    {
        battleState = BattleState.MoveAtackSelect;
        Vector2 warriorPosition = new Vector2(warrior.gameObject.transform.position.x, warrior.gameObject.transform.position.z);
        Cell warriorCell = GetComponent<HexField>().GetCellByCoord(warriorPosition);
        List<Cell> awaliableCells = GetComponent<HexPathFinder>().GetAwaliableCells(warriorCell.coord, warrior.CurrentWalkRange);
    }
}
