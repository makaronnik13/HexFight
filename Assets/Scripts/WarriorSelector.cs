using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class WarriorSelector : MonoBehaviour 
{

	public void Highlight(Color c)
	{
		foreach(Renderer mr in GetComponentsInChildren<Renderer>())
		{
			foreach(Material m in mr.materials)
			{
				if(m.shader == Shader.Find("Custom/Outline"))
				{
					m.SetColor("_OutlineColor", c);

				    m.SetFloat ("_Outline", c.a/50);
		
				}
			}
		}
	}

	public void DeHighlight()
	{
		Highlight (new Color(0,0,0,0));
	}
}
