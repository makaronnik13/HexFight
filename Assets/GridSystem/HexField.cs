using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexField : MonoBehaviour {

	private int raycastLayer = 9;

	public int size = 10;

	public Action<Cell> OnHexClicked;

    private float coef = 0.9f;

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

	public void GenerateCells(List<Vector3> cellsExistance)
    {
        foreach (Cell cell in cells)
        {
            Destroy(cell.gameObject);
        }
        cells.Clear();
        foreach (Vector3 c in cellsExistance)
        {


            Vector2 cell2DCoord = CellCoordToWorld(c);
            Vector3 cellPosition = new Vector3(cell2DCoord.x, transform.position.y , cell2DCoord.y);

				GameObject cellGo = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
				Cell cell = cellGo.GetComponent<Cell>();
			cell.GetComponentInChildren<Projector> ().orthographicSize = transform.localScale.x/2;
				cell.coord = c;
				cells.Add(cell);
        }
		if(cells.Count()>0)
		{
			Highlighter.defaultColor = cells[0].GetComponentInChildren<Projector>().material.color;
		}
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
				p.orthographicSize = transform.localScale.x/2;
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
        Vector2 cellCenter = CellCoordToWorld(c.coord);
		float _vert = transform.localScale.x * coef/2;
		float _hori = transform.localScale.x/2;


        
        float q2x = Math.Abs(point.x - cellCenter.x);         // transform the test point locally and to quadrant 2
        float q2y = Math.Abs(point.y - cellCenter.y);         // transform the test point locally and to quadrant 2
        if (q2x > _hori || q2y > _vert * 2) return false;           // bounding test (since q2 is in quadrant 2 only 2 tests are needed)
        return 2 * _vert * _hori - _vert * q2x - _hori * q2y >= 0;   // finally the dot product can be reduced to this due to the hexagon symmetry*
    
    }

	public Vector2 CellCoordToWorld(Vector2 cellCoord)
    {
		float x =  transform.localScale.x *  (float)Math.Sqrt(3) * (cellCoord.x + cellCoord.y/2);
		float y =  transform.localScale.x * 3 / 2 * -cellCoord.y;
        Vector3 pos = transform.position + new Vector3(x, 0, y) * 0.6f;//  lastScale * cellSize * (cellCoord.x + Mathf.Abs((cellCoord.y % 2 + .0f) / 2)) * Vector3.left - lastScale * coef * cellSize * Vector3.forward * cellCoord.y;
        return new Vector2(pos.x, pos.z);
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
}
