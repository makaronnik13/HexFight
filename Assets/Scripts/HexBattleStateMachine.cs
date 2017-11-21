using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class HexBattleStateMachine : Singleton<HexBattleStateMachine> {

	private BattleWarrior warrior;
	[HideInInspector]
	public List<Vector2> SelectionArea = new List<Vector2> ();

	private List<Cell> AvaliableMoveCell = new List<Cell> ();

	private BattleState _battleState = BattleState.SimpleSelect;
    public BattleState battleState
    {
        get
        {
            return _battleState;
        }

        set
        {
			if(value == BattleState.None)
			{
				FindObjectOfType<CellsHighlighter> ().DehighLightAll ();
			}
            _battleState = value;
			GetComponent<HexField>().AimedCell = GetComponent<HexField>().AimedCell;
        }
    }
	public enum BattleState
	{
		SimpleSelect,
		MoveAtackSelect,
		AreaSelect,
		None
	}

	public void SelectWarrior(BattleWarrior warrior)
	{
		FindObjectOfType<HexField> ().OnHexClicked -= HexClicked;
		this.warrior = warrior;
		if(warrior)
		{
			FindObjectOfType<HexField> ().OnHexClicked += HexClicked;
			SelectPointToAtack ();
		}
	}

    public void SelectPointToAtack()
    {
        battleState = BattleState.MoveAtackSelect;
        Vector2 warriorPosition = new Vector2(warrior.gameObject.transform.position.x, warrior.gameObject.transform.position.z);
        Cell warriorCell = GetComponent<HexField>().GetCellByWorldCoord(warriorPosition);
        

		AvaliableMoveCell = GetComponent<HexPathFinder>().GetAwaliableCells(warriorCell, warrior.CurrentWalkRange);

		FindObjectOfType<CellsHighlighter> ().HighlightArea (warriorCell, AvaliableMoveCell, FindObjectOfType<CellsHighlighter> ().avalibleColor, 4);
		//FindObjectOfType<CellsHighlighter> ().HilightPath (warriorCell);
    }

	public void SetSelectionArea(List<Vector2> offsets)
	{
		SelectionArea = offsets;
		battleState = BattleState.AreaSelect;
	}

	public void HexClicked(Cell cell)
	{
		if(battleState == BattleState.MoveAtackSelect && AvaliableMoveCell.Contains(cell))
		{
			battleState = BattleState.None;
			List<Vector3> pointsPath = new List<Vector3> ();

			foreach(Cell c in FindObjectOfType<HexPathFinder>().GetPath(FindObjectOfType<HexField>().WarriorCell(warrior), cell))
			{
				Vector2 wc2d = FindObjectOfType<HexField> ().CellCoordToWorld (c.coord);
				Vector3 wc = new Vector3 (wc2d.x, 50, wc2d.y);
				RaycastHit hit;
				Ray ray = new Ray (wc, Vector3.down);

				if (Physics.Raycast (ray, out hit, 10000,  1 << 9)) {
					pointsPath.Add (hit.point);		
				} 
			}
			warrior.GoByPath (pointsPath);
		}
	}
}
