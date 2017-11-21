using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellsHighlighter : MonoBehaviour {

    private class HighlightedLayer
    {
		public Dictionary<Cell, bool[]> cells = new Dictionary<Cell, bool[]>();
        public Color color;

		public HighlightedLayer(Dictionary<Cell, bool[]> cells, Color color)
        {
            this.cells = cells;
            this.color = color;
        }
    }

    public Color defaultColor = new Color(1,1,1,0.39f),
                 avalibleColor = Color.green,
                 wrongColor = Color.red,
                 specialColor = Color.magenta;

    private HexPathFinder pathFinder;
    private HexPathFinder PathFinder
    {
        get
        {
            if (pathFinder == null)
            {
                pathFinder = GetComponent<HexPathFinder>();
            }
            return pathFinder;
        }
    }

    private HexField hexManager;
    public HexField HexManager {
        get
        {
            if (!hexManager)
            {
                hexManager = GetComponent<HexField>();
            }
            return hexManager;
        }
    }

    private Dictionary<int, HighlightedLayer> highlightedLayers = new Dictionary<int, HighlightedLayer>();

    public void HilightPath(Cell start, Cell aim, int layerId)
    {
        List<Cell> path = PathFinder.GetPath(start, aim);


        if (path!=null)
        {
			HighlightArea(start, path, avalibleColor, layerId);
        }
    }

    public void DehighlightLayer(int layerId)
    {
        if (highlightedLayers.ContainsKey(layerId))
        {
			foreach (Cell c in highlightedLayers[layerId].cells.Keys)
            {

                c.ShowCellBorders(new bool[] { true, true, true, true, true, true }, defaultColor);

                foreach (HighlightedLayer layer in highlightedLayers.OrderBy(kp => kp.Key).Select(kp => kp.Value).ToList())
                {
					if (layer.cells.Keys.Contains(c) && layer!= highlightedLayers[layerId])
                    {
						c.ShowCellBorders(layer.cells[c], layer.color);
                        break;
                    }
                }
            }

            highlightedLayers.Remove(layerId);
        }
    }

    public void HighlightCircle(Vector2 center, int rad)
    {

    }

    public void HighlightCell(Vector2 coord, int layer)
    {
        HighlightArea(coord, new List<Vector2>() {Vector2.zero}, avalibleColor, layer);
    }

    public void HighlightLine(Vector2 center, Vector2 aim, int layerId)
    {
        List<Vector2> offsets = new List<Vector2>();

        if (center.x == aim.x)
        {
            if (center.x>aim.x)
            {
                for (int i = 0; i<=(int)center.x - (int)aim.x; i++)
                {
                    offsets.Add(new Vector2(-i,0));
                }
            }
            else
            {
                for (int i = 0; i <= (int)aim.x - (int)center.x; i++)
                {
                    offsets.Add(new Vector2(i, 0));
                }
            }
        }

        if (center.y == aim.y)
        {
            if (center.x > aim.x)
            {
                for (int i = 0; i <= (int)center.y - (int)aim.y; i++)
                {
                    offsets.Add(new Vector2(0, -i));
                }
            }
            else
            {
                for (int i = 0; i <= (int)aim.y - (int)center.y; i++)
                {
                    offsets.Add(new Vector2(0, i));
                }
            }
        }

        if (aim.x - center.x == aim.y - center.y)
        {
            if (center.x > aim.x)
            {
                for (int i = 0; i <= (int)center.y - (int)aim.y; i++)
                {
                    offsets.Add(new Vector2(-i, -i));
                }
            }
            else
            {
                for (int i = 0; i <= (int)aim.y - (int)center.y; i++)
                {
                    offsets.Add(new Vector2(i, i));
                }
            }
        }

        HighlightArea(center, offsets, wrongColor, layerId);
    }


	public void HighlightArea(Cell center, List<Cell> cells, Color c, int layerId, int rotation = 0)
	{
		List<Vector2> offsets = new List<Vector2> ();
		foreach(Cell cell in cells)
		{
			offsets.Add (cell.coord - center.coord);
		}
		HighlightArea (center.coord, offsets, c, layerId, rotation);
	}

    public void HighlightArea(Vector2 center, List<Vector2> offsets, Color c, int layerId, int rotation = 0)
    {
		Dictionary<Cell, bool[]> newHighlightedCells = new Dictionary<Cell, bool[]>();

        DehighlightLayer(layerId);

        foreach (Vector2 offset in offsets)
        {
            Cell hc = HexManager.GetCellByCoord(center + offset);
            if(hc)
            {
				newHighlightedCells.Add(hc, new bool[0]);
            }
        }

		for(int j= 0;j<newHighlightedCells.Count;j++)
		{
			List<bool> b = new List<bool> ();
			for(int i =0; i<6;i++)
			{
				Cell n = HexManager.Neighbour(newHighlightedCells.ElementAt(j).Key.coord, i, newHighlightedCells.Keys.ToList());
				b.Add (n==null);
			}

			newHighlightedCells.ElementAt(j).Key.ShowCellBorders(b.ToArray(), c);



			newHighlightedCells[newHighlightedCells.ElementAt(j).Key] = b.ToArray();
			
		}



        highlightedLayers.Add(layerId, new HighlightedLayer(newHighlightedCells, c));
    }

	public void DehighLightAll ()
	{
		List<int> layersIds = highlightedLayers.Select (l => l.Key).ToList();
		foreach(int i in layersIds)
		{
			DehighlightLayer (i);
		}

		highlightedLayers.Clear ();
	}
}
