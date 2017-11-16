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

    private bool Passable;
    public IEnumerable<Cell> AllNeighbours { get; set; }
    public IEnumerable<Cell> Neighbours
    {
        get
        {
          return AllNeighbours.Where(o => o.Passable);
        }
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
