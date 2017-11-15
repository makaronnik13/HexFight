using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexField : MonoBehaviour {

    public float height = 10;
    public float width = 10;

    private float cellSize = 0.7f;
    private float coef = 0.9f;

    public GameObject cellPrefab;
    private List<Cell> cells = new List<Cell>();

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
            aimedCell = value;
            if (aimedCell)
            {
                Debug.Log(value.coord);
                Highlighter.HighlightCell(value.coord);
            }
        }
    }

	public float lastScale = 1;

    [ContextMenu("GenerateCells")]
    public void GenerateFakeCells()
    {
        List<Vector2> positions = new List<Vector2>();

        for (int i = Mathf.CeilToInt(-(width) / 2); i < Mathf.FloorToInt((width) / 2); i++)
        {
            for (int j = Mathf.CeilToInt(-(height) / 2); j < Mathf.FloorToInt((height) / 2); j++)
            {
                if (UnityEngine.Random.Range(0,10)>3)
                {
                    positions.Add(new Vector2(i,j));
                }
            }
        }
        GenerateCells(positions);
    }

	void Start()
	{
		lastScale = transform.localScale.x;
        Raycaster.Instance.OnRaycastHit += RaycatHit;
	}

    private void RaycatHit(Vector3 point, GameObject hitObject)
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

    public void GenerateCells(List<Vector2> cellsExistance)
    {
        foreach (Cell cell in cells)
        {
            Destroy(cell.gameObject);
        }
        cells.Clear();

        foreach (Vector2 c in cellsExistance)
        {
            Vector2 cell2DCoord = CellCoordToWorld(c);
            Vector3 cellPosition = new Vector3(cell2DCoord.x, transform.position.y , cell2DCoord.y);
            GameObject cellGo = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
            Cell cell = cellGo.GetComponent<Cell>();
			cell.GetComponentInChildren<Projector> ().orthographicSize = lastScale/2;
            cell.coord = c;
            cells.Add(cell);
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
		if(transform.localScale.x!=lastScale || transform.localScale.y!=lastScale)
		{
			transform.localScale = Vector3.one * transform.localScale.x;
			lastScale = transform.localScale.x;
			foreach(Projector p in GetComponentsInChildren<Projector>())
			{
				p.orthographicSize = lastScale/2;
			}
		}
	}

    public Cell GetCellByCoord(Vector2 coord)
    {
        return cells.Find(c=>c.coord == coord);
    }

    private bool IsPointInsideHex(Vector2 point, Cell c)
    {
        Vector2 cellCenter = CellCoordToWorld(c.coord);
        float _vert = lastScale * coef * cellSize/2;
        float _hori = lastScale * cellSize/2;


        
        float q2x = Math.Abs(point.x - cellCenter.x);         // transform the test point locally and to quadrant 2
        float q2y = Math.Abs(point.y - cellCenter.y);         // transform the test point locally and to quadrant 2
        if (q2x > _hori || q2y > _vert * 2) return false;           // bounding test (since q2 is in quadrant 2 only 2 tests are needed)
        return 2 * _vert * _hori - _vert * q2x - _hori * q2y >= 0;   // finally the dot product can be reduced to this due to the hexagon symmetry*
    
    }

    private Vector2 CellCoordToWorld(Vector2 cellCoord)
    {
        Vector3 pos = transform.position + lastScale * cellSize * (cellCoord.x + Mathf.Abs((cellCoord.y % 2 + .0f) / 2)) * Vector3.left + lastScale * coef * cellSize * Vector3.forward * cellCoord.y;
        return new Vector2(pos.x, pos.z);
    }

}
