using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarriorSelector : MonoBehaviour 
{	
	public Action<BattleWarrior> onPointerEnter, onPointerExit, onPointerClick;

	public bool Selected = false;

	void OnMouseEnter() {
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
