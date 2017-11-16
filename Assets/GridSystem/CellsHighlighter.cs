using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellsHighlighter : MonoBehaviour {

    private class HighlightedLayer
    {
        public List<Cell> cells = new List<Cell>();
        public Color color;

        public HighlightedLayer(List<Cell> cells, Color color)
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

        for (int i = 0; i<path.Count; i++)
        {
            path[i] -= start;
        }

        if (path!=null)
        {
            HighlightArea(start, path, avalibleColor, layerId);
        }
    }

    public void DehighlightLayer(int layerId)
    {
        if (highlightedLayers.ContainsKey(layerId))
        {
            foreach (Cell c in highlightedLayers[layerId].cells)
            {

                c.ShowCellBorders(new bool[] { true, true, true, true, true, true }, defaultColor);

                foreach (HighlightedLayer layer in highlightedLayers.OrderBy(kp => kp.Key).Select(kp => kp.Value).ToList())
                {
                    if (layer.cells.Contains(c) && layer!= highlightedLayers[layerId])
                    {
                        c.ShowCellBorders(new bool[] { true, true, true, true, true, true }, layer.color);
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

    public void HighlightArea(Vector2 center, List<Vector2> offsets, Color c, int layerId, int rotation = 0)
    {
        List<Cell> newHighlightedCells = new List<Cell>();

        DehighlightLayer(layerId);

        foreach (Vector2 offset in offsets)
        {
            Cell hc = HexManager.GetCellByCoord(center + offset);
            if(hc)
            {
                newHighlightedCells.Add(hc);
                hc.ShowCellBorders(new bool[] { true, true, true, true, true, true }, c);
            }
        }

        highlightedLayers.Add(layerId, new HighlightedLayer(newHighlightedCells, c));
    }
}
