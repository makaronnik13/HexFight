using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellsHighlighter : MonoBehaviour {

    public enum HighlightType
    {
        Borders,
        Center,
        Both
    }

    public enum HighlightLayer
    {
        None = 0,
        Selection = 1,
        MovementArea = 2,
        Movement = 3,
        SkillArea = 4
    }

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

    private static Color[] layersColors = new Color[]{
    new Color(1,1,1,0.39f),
    Color.green,
    Color.green,
    Color.green,
    Color.magenta
    };

    private static HighlightType[] layersHighlightTypes = new HighlightType[]{
    HighlightType.Borders,
    HighlightType.Borders,
    HighlightType.Borders,
    HighlightType.Center,
    HighlightType.Borders
    };

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


    public void DehighlightLayer(HighlightLayer hlayer)
    {
        int layerId = (int)hlayer;

        if (highlightedLayers.ContainsKey(layerId))
        {
			foreach (Cell c in highlightedLayers[layerId].cells.Keys)
            {

                c.ShowCellBorders(new bool[] { false, false, false, false, false, false }, Color.black, false, Color.black);

                foreach (HighlightedLayer layer in highlightedLayers.OrderBy(kp => kp.Key).Select(kp => kp.Value).ToList())
                {
					if (layer.cells.Keys.Contains(c) && layer!= highlightedLayers[layerId])
                    {
                        /*
                        bool showBorder = true;
                        if (LayerType(highlightedLayers.ElementAt())== HighlightType.Borders)
                        {
                            showBorder = false;
                        }

						c.ShowCellBorders(layer.cells[c], layer.color, );
                        break;
                        */
                    }
                }
            }

            highlightedLayers.Remove(layerId);
        }
    }

    public void HighlightPath(Cell from, Cell to, int ap)
    {
        List<Cell> path = GetComponent<HexPathFinder>().GetPath(from, to);

        if (path!=null)
        {
            HighlightArea(from, path, HighlightLayer.MovementArea);
        }
        
    }

    public void HighlightCircle(Vector2 center, int rad)
    {

    }

    public void HighlightCell(Vector2 coord)
    {
        HighlightArea(coord, new List<Vector2>() {Vector2.zero}, HighlightLayer.Selection);
    }

    public void HighlightLine(Vector2 center, Vector2 aim, HighlightLayer layer)
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

        HighlightArea(center, offsets, layer);
    }


	public void HighlightArea(Cell center, List<Cell> cells,  HighlightLayer layer, int rotation = 0)
	{

		List<Vector2> offsets = new List<Vector2> ();
		foreach(Cell cell in cells)
		{
			offsets.Add (cell.coord - center.coord);
		}
		HighlightArea (center.coord, offsets, layer, rotation);
	}

    public void HighlightArea(Vector2 center, List<Vector2> offsets, HighlightLayer layer, int rotation = 0)
    {
		Dictionary<Cell, bool[]> newHighlightedCells = new Dictionary<Cell, bool[]>();

        DehighlightLayer(layer);

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
                bool border = (n == null) && LayerType(layer)!= HighlightType.Center;
				b.Add(border);
			}

            bool  showCenter = !(LayerType(layer) == HighlightType.Center);

			newHighlightedCells.ElementAt(j).Key.ShowCellBorders(b.ToArray(), LayerColor(layer), showCenter, LayerColor(layer));
			newHighlightedCells[newHighlightedCells.ElementAt(j).Key] = b.ToArray();	
		}

        highlightedLayers.Add((int)layer, new HighlightedLayer(newHighlightedCells, LayerColor(layer)));
    }

    private Color LayerColor(HighlightLayer layer)
    {
        return layersColors[(int)layer];
    }

    private HighlightType LayerType(HighlightLayer layer)
    {
        return layersHighlightTypes[(int)layer];
    }

    public void DehighLightAll ()
	{
		List<int> layersIds = highlightedLayers.Select (l => l.Key).ToList();
		foreach(int i in layersIds)
		{
			DehighlightLayer ((HighlightLayer)i);
		}

		highlightedLayers.Clear ();
	}
}
