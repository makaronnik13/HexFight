using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWarrior :MonoBehaviour
{
	public float Hp = 100;
	public float Mp = 10;
	public float Initiative = 3;
	public int ActionPoints = 7;

    public float WalkSpeed = 1;

	public bool Enemy = false;

	public Sprite portrait;

	private List<Vector2> cellsSize = new List<Vector2>(){new Vector2(0,0)};
	public List<Cell> cells = new List<Cell>();

    public int CurrentWalkRange
    {
        get
        {
            return Mathf.FloorToInt(ActionPoints*WalkSpeed);
        }
    }
}

