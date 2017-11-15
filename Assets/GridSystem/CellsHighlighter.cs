using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsHighlighter : MonoBehaviour {

    public Color avalibleColor = Color.green,
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

    public void HilightPath(Vector2 start, Vector2 aim)
    {
        List<Vector2> path = PathFinder.GetPath(start, aim);

        for (int i = 0; i<path.Count; i++)
        {
            path[i] -= start;
        }

        if (path!=null)
        {
            HighlightArea(start, path, avalibleColor);
        }
    }
    public void HighlightCircle(Vector2 center, int rad)
    {

    }

    public void HighlightCell(Vector2 coord)
    {
        HighlightArea(coord, new List<Vector2>() {Vector2.zero}, avalibleColor);
    }

    public void HighlightLine(Vector2 center, Vector2 aim)
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

        HighlightArea(center, offsets, wrongColor);
    }

    public void HighlightArea(Vector2 center, List<Vector2> offsets, Color c, int rotation = 0)
    {
        foreach (Vector2 offset in offsets)
        {
            HexManager.GetCellByCoord(center+offset).ShowCellBorders(new bool[] {true, true, true, true, true, true}, c);
        }
    }
}
