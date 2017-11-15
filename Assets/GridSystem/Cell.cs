using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Cell : MonoBehaviour {

	public int index;

    public bool[] binaryS;

    public Texture2D[] baseBorderTextures;
    public Vector2 coord;
    public bool enable = true;
	private CellVisual visual;
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
