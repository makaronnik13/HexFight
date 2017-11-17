﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarriorSelector : MonoBehaviour 
{	
	public Action<BattleWarrior> onPointerEnter, onPointerExit, onPointerClick;

	private bool mouseOn = false;

	private bool selected = false;
	public bool Selected
	{
		get
		{
			return selected;
		}
		set
		{
			selected = value;
			if (value) 
			{
				FindObjectOfType<FakeCellsTester> ().SelectWarrior (GetComponent<BattleWarrior> ());
			} else 
			{
				if(!mouseOn)
				{
					DeHighlight ();
				}
			}
		}
	}

	void OnMouseEnter() 
	{
		mouseOn = true;
		if (GetComponent<BattleWarrior> ().Enemy) 
		{
			Highlight (Color.red);
		} else 
		{
			Highlight (Color.green);		
		}

		if (onPointerEnter!=null) 
		{
			onPointerEnter.Invoke (GetComponent<BattleWarrior>());
		}
	}

	void OnMouseExit() {
		mouseOn = false;
		if(!Selected)
		{
			DeHighlight ();
		}

		if(onPointerExit!=null)
		{
			onPointerExit.Invoke (GetComponent<BattleWarrior>());
		}
	}

	void OnMouseDown()
	{
		foreach(WarriorSelector bw in FindObjectsOfType<WarriorSelector>())
		{
			bw.Selected = false;
			bw.DeHighlight ();
		}

		if (GetComponent<BattleWarrior> ().Enemy) 
		{
			Highlight (Color.red);
		} else 
		{
			Highlight (Color.green);		
		}

		Selected = true;

		if (onPointerClick != null) 
		{
			onPointerClick.Invoke (GetComponent<BattleWarrior>());
		}
	}

	public void Highlight(Color c)
	{
		foreach(Renderer mr in GetComponentsInChildren<Renderer>())
		{
			foreach(Material m in mr.materials)
			{
				if(m.shader == Shader.Find("Custom/Outline"))
				{
					m.SetColor("_OutlineColor", c);
				}
			}
		}
	}

	public void DeHighlight()
	{
		Highlight (new Color(0,0,0,0));
	}
}
