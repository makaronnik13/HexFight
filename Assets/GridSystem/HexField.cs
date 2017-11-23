using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexField : MonoBehaviour {

	private int raycastLayer = 9;

	public int size = 10;

	public Action<Cell> OnHexClicked;


	private List<Vector3> cellsPositions = new List<Vector3>();

	public List<BattleWarrior> Warriors = new List<BattleWarrior>();

    public GameObject cellPrefab;
    private List<Cell> cells = new List<Cell>();
	public List<Cell> Cells
	{
		get
		{
			return cells;
		}
	}

    private CellsHighlighter highlighter;
    private CellsHighlighter Highlighter
    {
        get
        {
            if (!highlighter)
            {
                highlighter = GetComponent<CellsHighlighter>();
            }
            return highlighter;
        }
    }

    private Cell aimedCell = null;
    public Cell AimedCell
    {
        get
        {
            return aimedCell;
        }
        set
        {
            if (value)
            {
				if(aimedCell && CellWarrior(value))
				{
					GameController.Instance.HighlightedWarrior = CellWarrior (value);
				}
					
				if (HexBattleStateMachine.Instance.battleState == HexBattleStateMachine.BattleState.AreaSelect) {
					Highlighter.HighlightArea (value.coord, HexBattleStateMachine.Instance.SelectionArea, Highlighter.specialColor, 4);
				} else {
					Highlighter.HighlightCell (value.coord, 1);
				}      
            }
            else
            {
                Highlighter.HighlightCell(Vector2.one*Mathf.Infinity, 1);
            }
            aimedCell = value;
        }
    }

	public BattleWarrior CellWarrior(Cell cell)
	{
		foreach(BattleWarrior bw in FindObjectsOfType<BattleWarrior>())
		{
			if(IsPointInsideHex(new Vector2(bw.transform.position.x, bw.transform.position.z), cell))
			{
				return bw;
			}
		}
		return null;
	}

	public Cell WarriorCell(BattleWarrior warrior)
	{
		return GetCellByWorldCoord (warrior.transform.position);
	}

	void Start()
	{
		Raycaster.Instance.AddListener ((Vector3 point, GameObject rayHitObject)=>{
			Raycast(point, rayHitObject);
		}, ()=>{}, raycastLayer);
	}

    private void Raycast(Vector3 point, GameObject hitObject)
    {
        Cell newAimedCell = null;


        foreach (Cell cell in cells)
        {
            if (IsPointInsideHex(new Vector3(point.x, point.z), cell))
            {
                newAimedCell = cell;
            }
        }


        if (newAimedCell!=AimedCell)
        {
            AimedCell = newAimedCell;
        }
    }

	public void GenerateCells(List<Vector3> cellsPositions, List<Vector2> cellsCoordinates)
    {
        foreach (Cell cell in cells)
        {
            Destroy(cell.gameObject);
        }
        cells.Clear();

	

		Debug.Log (cellsPositions.Count()+"/"+cellsCoordinates.Count());

		for (int i = 0; i<cellsPositions.Count()-1;i++)
        {
			GameObject cellGo = Instantiate(cellPrefab, new Vector3(cellsPositions[i].x, transform.position.y,cellsPositions[i].z), Quaternion.identity, transform);
			Cell cell = cellGo.GetComponent<Cell>();
			cell.GetComponentInChildren<Projector> ().orthographicSize = transform.localScale.x/1.4f;

			Debug.Log (cellsCoordinates.Count()+"/"+i);

			cell.coord = cellsCoordinates[i];
			cells.Add(cell);
        }

		if(cells.Count()>0)
		{
			Highlighter.defaultColor = cells[0].GetComponentInChildren<Projector>().material.color;
		}

		this.cellsPositions = cellsPositions;
    }
		
    public bool CellEnable(Vector2 position)
    {
        Cell cell = cells.Find(c => c.coord == position);
        if (cell)
        {
            return cell.enable;
        }
        return false;
    }

	private void Update()
	{
		if(transform.hasChanged)
		{
			foreach(Projector p in GetComponentsInChildren<Projector>())
			{
				p.orthographicSize = transform.localScale.x/1.4f;
			}
		}

		if(Input.GetMouseButtonDown(0))
		{
			if (AimedCell && OnHexClicked!=null)
			{
				OnHexClicked (AimedCell);
			}

			if(AimedCell && CellWarrior(AimedCell))
			{
				GameController.Instance.Warrior = CellWarrior (AimedCell);
			}
		}
	}

    public Cell GetCellByCoord(Vector2 coord)
    {
        return cells.Find(c=>c.coord == coord);
    }

	public Cell GetCellByWorldCoord(Vector3 coord)
	{
		return GetCellByWorldCoord (new Vector2(coord.x, coord.z));
	}

    public Cell GetCellByWorldCoord(Vector2 coord)
    {
        foreach (Cell c in cells)
        {
            if (IsPointInsideHex(coord, c))
            {
                return c;
            }
        }
        return null;
    }


    private bool IsPointInsideHex(Vector2 point, Cell c)
    {
		Vector3 cc = CellCoordToWorld(c);
		Vector2 cellCenter = new Vector2 (cc.x, cc.z);

		/*
		float _vert = transform.localScale.x * coef/2;
		float _hori = transform.localScale.x/2;
        
        float q2x = Math.Abs(point.x - cellCenter.x);         // transform the test point locally and to quadrant 2
        float q2y = Math.Abs(point.y - cellCenter.y);         // transform the test point locally and to quadrant 2
        if (q2x > _hori || q2y > _vert * 2) return false;           // bounding test (since q2 is in quadrant 2 only 2 tests are needed)
        return 2 * _vert * _hori - _vert * q2x - _hori * q2y >= 0;   // finally the dot product can be reduced to this due to the hexagon symmetry*
    	*/
		return Vector2.Distance (point, cellCenter)<=transform.localScale.x;
    }

	public Vector3 CellCoordToWorld(Cell c)
    {
		/*
		float x =  transform.localScale.x *  (float)Math.Sqrt(3) * (cellCoord.x + cellCoord.y/2);
		float y =  transform.localScale.x * 3 / 2 * -cellCoord.y;
        Vector3 pos = transform.position + new Vector3(x, 0, y) * 0.6f;//  lastScale * cellSize * (cellCoord.x + Mathf.Abs((cellCoord.y % 2 + .0f) / 2)) * Vector3.left - lastScale * coef * cellSize * Vector3.forward * cellCoord.y;
		*/
		return cellsPositions[cells.IndexOf(c)];
    }

	public List<Cell> AdjustedHexes(Cell center)
	{
		List<Cell> adjusted = new List<Cell> ();
		foreach(Cell c in cells)
		{
			for(int i = 0; i<6; i++)
			{
				Cell n = Neighbour (center.coord, i);
				if(n && c.coord == n.coord)
				{
					adjusted.Add (c);
				}		
			}
		}
		return adjusted;
	}

	List<Vector2> directions = new List<Vector2>{
		new Vector2(-1,  1), new Vector2(-1, 0), new Vector2( 0, -1),
		new Vector2(1,  -1), new Vector2(1, 0),new Vector2( 0, 1)
	};
		
	public Cell Neighbour(Vector2 hex, int direction, List<Cell> fromCells = null)
	{
		if(fromCells==null)
		{
			fromCells = cells;
		}
		var dir = directions [direction];
		return fromCells.Find(c=>c.coord == new Vector2 (hex.x + dir.x, hex.y + dir.y));
	}
			
	public bool IsPassable (Cell c)
	{
		return CellWarrior(c)==null;
	}

	private bool Raycast(Vector3 fromPosition, float minOffset, float maxOffset, LayerMask obstacleLayer, LayerMask raycastingLayer)
	{
		RaycastHit hit;
		if (Physics.Raycast (fromPosition, Vector3.down, out hit, 1000, obstacleLayer.value | raycastingLayer.value)) {
			return false;
		} 

		return true;
	}

	private bool Contains(LayerMask mask, int layer)
	{
		return mask == (mask | (1 << layer));
	}

	public Cell GetClosestCell(Vector3 v, bool onlyEmpty = false)
	{
		List<Cell> selectCells = cells.Where (c=>c.enable).ToList();
	
		if(onlyEmpty)
		{
			selectCells = selectCells.Where (c=>c.Passable).ToList();
		}
		return selectCells.Aggregate((curMin, c) => curMin == null || Vector3.Distance(v, CellCoordToWorld(c)) < Vector3.Distance(v, CellCoordToWorld(curMin)) ? c : curMin);
	}
}
