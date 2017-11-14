using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Cell : MonoBehaviour {

	public string binaryS;

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

	[ContextMenu ("show cell test")]
	public void TestShowCell()
	{
		Visual.ShowBorder (new bool[]{
			binaryS[0] == '1',
			binaryS[1] == '1',
			binaryS[2] == '1',
			binaryS[3] == '1',
			binaryS[4] == '1',
			binaryS[5] == '1'
		}, Color.green);
	}
		
}
