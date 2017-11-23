using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CellVisual : MonoBehaviour {

    private class BordersAndRotation
    {
        public int rotation;
        public int textureIndex;

        public BordersAndRotation(int rotation, int textureIndex)
        {
            this.rotation = rotation;
            this.textureIndex = textureIndex;
        }
    }

    private static Dictionary<int, BordersAndRotation> bordersDictionary = new Dictionary<int, BordersAndRotation>()
    {
        //zero
        {0, new BordersAndRotation(0, 0)},

        //one
        {1, new BordersAndRotation(1, 1)},
        {2, new BordersAndRotation(2, 1)},
        {4, new BordersAndRotation(3, 1)},
        {8, new BordersAndRotation(4, 1)},
        {16, new BordersAndRotation(5, 1)},
        {32, new BordersAndRotation(0, 1)},

        //two
        {3, new BordersAndRotation(2, 2)},
        {6, new BordersAndRotation(3, 2)},
        {12, new BordersAndRotation(4, 2)},
        {24, new BordersAndRotation(5, 2)},
        {48, new BordersAndRotation(0, 2)},
        {33, new BordersAndRotation(1, 2)},

        {9, new BordersAndRotation(2, 3)},
        {18, new BordersAndRotation(3, 3)},
        {36, new BordersAndRotation(4, 3)},

        {5, new BordersAndRotation(3, 4)},
        {10, new BordersAndRotation(4, 4)},
        {20, new BordersAndRotation(5, 4)},
        {40, new BordersAndRotation(0, 4)},
        {17, new BordersAndRotation(1, 4)},
        {34, new BordersAndRotation(2, 4)},
        //three
        {7, new BordersAndRotation(3, 5)},
        {14, new BordersAndRotation(4, 5)},
        {28, new BordersAndRotation(5, 5)},
        {56, new BordersAndRotation(0, 5)},
        {49, new BordersAndRotation(1, 5)},
        {35, new BordersAndRotation(2, 5)},

        {21, new BordersAndRotation(2, 6)},
        {42, new BordersAndRotation(3, 6)},

        {11, new BordersAndRotation(2, 7)},
        {22, new BordersAndRotation(3, 7)},
        {44, new BordersAndRotation(4, 7)},
        {25, new BordersAndRotation(5, 7)},
        {50, new BordersAndRotation(0, 7)},
        {37, new BordersAndRotation(1, 7)},

        {13, new BordersAndRotation(4, 8)},
        {26, new BordersAndRotation(5, 8)},
        {52, new BordersAndRotation(0, 8)},
        {41, new BordersAndRotation(1, 8)},
        {19, new BordersAndRotation(2, 8)},
        {38, new BordersAndRotation(3, 8)},

        //four

        {60, new BordersAndRotation(0, 11)},
        {57, new BordersAndRotation(1, 11)},
        {51, new BordersAndRotation(2, 11)},
        {39, new BordersAndRotation(3, 11)},
        {15, new BordersAndRotation(4, 11)},
        {30, new BordersAndRotation(5, 11)},

        {54, new BordersAndRotation(3, 10)},
        {45, new BordersAndRotation(4, 10)},
        {27, new BordersAndRotation(5, 10)},

        {58, new BordersAndRotation(0, 9)},
        {53, new BordersAndRotation(1, 9)},
        {43, new BordersAndRotation(2, 9)},
        {23, new BordersAndRotation(3, 9)},
        {46, new BordersAndRotation(4, 9)},
        {29, new BordersAndRotation(5, 9)},

        //five
        {31, new BordersAndRotation(5, 12)},
        {47, new BordersAndRotation(4, 12)},
        {55, new BordersAndRotation(3, 12)},
        {59, new BordersAndRotation(2, 12)},
        {61, new BordersAndRotation(1, 12)},
        {62, new BordersAndRotation(0, 12)},

        //six
        {63, new BordersAndRotation(0, 13)}
    };

    public void ShowCenter(bool center, Color centerColor)
    {
        if (!center)
        {
            centerColor.a = 0;
        }
        ProjectionMaterial.SetColor("_CenterColor", centerColor);
    }

    private Texture2D[] borderTextures;
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
		int bordersInt = BordersToInt (borders);

        if (borders.ToList().FindAll(b => b == true).Count > 0)
        {
            projector.enabled = true;
            ProjectionMaterial.SetTexture("_ShadowTex", borderTextures[bordersDictionary[bordersInt].textureIndex]);
            ProjectionMaterial.color = color;
            transform.rotation = Quaternion.Euler(90,30+ bordersDictionary[bordersInt].rotation*60, 0);
        }
        else
        {
            projector.enabled = false;
        }
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
