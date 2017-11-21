using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Cell : MonoBehaviour, IHasNeighbours<Cell> {

	public int index;

    public bool[] binaryS;

    public Texture2D[] baseBorderTextures;
    public Vector2 coord;
    public bool enable = true;
	private CellVisual visual;

	public bool Passable
	{
		get
		{
			return GetComponentInParent<HexField> ().IsPassable (this);
		}
	}
	public IEnumerable<Cell> AllNeighbours 
	{ 
		get
		{ 
			return GetComponentInParent<HexField> ().AdjustedHexes(this);
		}
	}
    public IEnumerable<Cell> Neighbours
    {
        get
        {
          return AllNeighbours.Where(o => o.Passable);
        }
    }

    [ContextMenu("neighbours")]
    public void NeighboursCount()
    {
        Debug.Log(Neighbours.Count()+"");
    }

    private CellVisual Visual
	{
		get
		{
			if(!visual)
			{
				visual = GetComponentInChildren<CellVisual> ();
				visual.SetBorderTextures (baseBorderTextures.ToList());
			}
			return visual;
		}
	}

	public void ShowCellBorders(bool[] binaryS, Color c)
	{
		Visual.ShowBorder (binaryS, c);
	}
		
}
