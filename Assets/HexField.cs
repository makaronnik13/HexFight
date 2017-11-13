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

    [ContextMenu("GenerateCells")]
    public void GenerateFakeCells()
    {
        List<Vector2> positions = new List<Vector2>();

        for (int i = Mathf.CeilToInt(-(width) / 2); i < Mathf.FloorToInt((width) / 2); i++)
        {
            for (int j = Mathf.CeilToInt(-(height) / 2); j < Mathf.FloorToInt((height) / 2); j++)
            {
                if (Random.Range(0,10)>3)
                {
                    positions.Add(new Vector2(i,j));
                }
            }
        }
        GenerateCells(positions);
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
                    Vector3 cellPosition = transform.position + cellSize * (c.x + Mathf.Abs((c.y % 2 + .0f) / 2)) * Vector3.left + coef * cellSize * Vector3.forward * c.y;
                    GameObject cellGo = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
                    Cell cell = cellGo.GetComponent<Cell>();
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
}
