using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBattleStateMachine : Singleton<HexBattleStateMachine> {

	private BattleWarrior warrior;
	private BattleState battleState = BattleState.SimpleSelect;
	private enum BattleState
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


}
