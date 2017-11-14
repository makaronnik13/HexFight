using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellVisual : MonoBehaviour {

	public Texture2D[] borderTextures;
	private Material projectionMaterial;
	private Material ProjectionMaterial
	{
		get
		{
			if(!projectionMaterial)
			{
				projectionMaterial = new Material (Shader.Find("Projector/Light"));
				projectionMaterial.CopyPropertiesFromMaterial(GetComponent<Projector> ().material);
				projector.material = projectionMaterial;
			}
			return projectionMaterial;
		}
	}
	private Projector _projector;
	private Projector projector
	{
		get
		{
			if(!_projector)
			{
				_projector = GetComponent<Projector> ();
			}
			return _projector;
		}
	}

	public void ShowBorder(bool[] borders, Color color)
	{
		Debug.Log (BordersToInt(borders));
		int bordersInt = BordersToInt (borders);

		Texture2D tex = null;
		int rotationAngle = 60;
		projector.enabled = true;

		switch(borders.ToList().FindAll(b=>b==true).Count)
		{
		default:
			projector.enabled = false;
			break;
		case 1:
			tex = borderTextures [11];
			transform.rotation = Quaternion.Euler (90, 90+rotationAngle*borders.ToList().IndexOf(true), 0);
			break;
		case 2:
			if(bordersInt == 3 || bordersInt == 6 || bordersInt == 12 || bordersInt == 24 || bordersInt == 48||bordersInt == 33)
			{
				tex = borderTextures [10];	
			}
			if(bordersInt == 72 || bordersInt == 36 || bordersInt == 18)
			{
				tex = borderTextures [9];	
			}
			if(bordersInt == 5 || bordersInt == 10 || bordersInt == 20 || bordersInt == 40 || bordersInt == 17 || bordersInt == 34)
			{
				tex = borderTextures [8];	
			}
			break;
		case 3:
			tex = borderTextures [7];
			tex = borderTextures [5];
			tex = borderTextures [6];
			break;
		case 4:
			tex = borderTextures [2];
			tex = borderTextures [3];
			tex = borderTextures [4];
			break;
		case 5:
			tex = borderTextures [1];
			transform.rotation = Quaternion.Euler(90, 30+rotationAngle*borders.ToList().IndexOf(false), 0);
			break;
		case 6:
			tex = borderTextures [0];
			break;
		}

		ProjectionMaterial.SetTexture("_ShadowTex", tex);
		ProjectionMaterial.color = color;
	}

	public void SetBorderTextures(List<Texture2D> newTextures)
	{
		borderTextures = newTextures.ToArray ();
	}

	public int BordersToInt(bool[] b)
	{
		int result = 0;

		for(int i = 0; i<b.Length;i++)
		{
			int p = 0;
			if(b[i])
			{
				p = 1;
			}
			result += p * (int)Mathf.Pow (2, i);	
		}

		return result;
	}
}
