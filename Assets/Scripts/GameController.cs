using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : Singleton<GameController> {

	public enum GameMode
	{
		Adventure,
		Battle
	}

	public Action<BattleWarrior> warriorPointed, warriorDepointed, warriorSelected;

	private BattleWarrior warrior;
	public BattleWarrior Warrior
	{
		get
		{
			return warrior;
		}
		set
		{
			if(warriorDepointed!=null)
			{
				warriorDepointed.Invoke (warrior);
			}
			warrior = value;

			warriorSelected.Invoke (warrior);

			if(warriorPointed!=null)
			{
				warriorPointed.Invoke (warrior);
			}
		}
	}

	private BattleWarrior highlightedWarrior;
	public BattleWarrior HighlightedWarrior
	{
		get
		{
			return highlightedWarrior;
		}
		set
		{
			if(warriorDepointed!=null)
			{
				if (Warrior != highlightedWarrior) 
				{
					warriorDepointed.Invoke (highlightedWarrior);
				}
			}
			highlightedWarrior = value;
			if(warriorPointed!=null)
			{
				warriorPointed.Invoke (highlightedWarrior);
			}
		}
	}

	private GameMode mode = GameMode.Adventure;
	public GameMode Mode
	{
		get
		{
			return mode;
		}
		set
		{
			mode = value;
		}
	}

	private void PointWarrior(BattleWarrior warrior)
	{
		HighlightedWarrior = warrior;
	}

	private void SelectWarrior(BattleWarrior warrior)
	{
		Warrior = warrior;
	}

	private void DePointWarrior(BattleWarrior warrior)
	{
		HighlightedWarrior = null;
	}

	private void OnEnable()
	{
		Raycaster.Instance.AddListener ((Vector3 v, GameObject go)=>{
			HighlightedWarrior = go.GetComponent<BattleWarrior>();
		}, ()=>{HighlightedWarrior = null;}, 10, 0.1f, LayerRaycaster.InputType.none, 0);
		Raycaster.Instance.AddListener ((Vector3 v, GameObject go)=>{
			Warrior = go.GetComponent<BattleWarrior>();
		}, ()=>{Warrior = null;}, 10, 0.1f, LayerRaycaster.InputType.mouseDown, 0);
	}

	private void OnDisable()
	{
		if (Raycaster.Instance) 
		{
			Raycaster.Instance.RemoveRaycaster (10, LayerRaycaster.InputType.none, 0);
			Raycaster.Instance.RemoveRaycaster (10, LayerRaycaster.InputType.mouseDown, 0);
		}
	}
}
