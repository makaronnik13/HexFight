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

    public enum BattleAction
    {
        None,
        Default
    }

    public BattleAction actionType = BattleAction.Default;

	public Action<GameMode> onModeChanged;

	public Action<BattleWarrior> warriorPointed, warriorDepointed, warriorSelected;


	private HexField currentField;
	public HexField CurrentField
	{
		get
		{
			return currentField;
		}
		set
		{
			currentField = value;
		}
	}

	private BattleWarrior warrior;
	public BattleWarrior Warrior
	{
		get
		{
			return warrior;
		}
		set
		{
            if (warrior != value)
            {
                if (warriorDepointed != null)
                {
                    warriorDepointed.Invoke(warrior);
                }
                warrior = value;

                warriorSelected.Invoke(warrior);

                if (warriorPointed != null)
                {
                    warriorPointed.Invoke(warrior);
                }

                if (Mode == GameMode.Battle)
                {
                    CurrentField.SelectWarrior(warrior);
                }
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

	[SerializeField]
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
			if(onModeChanged!=null)
			{
				onModeChanged.Invoke (mode);
			}
		}
	}

	
	private void OnEnable()
	{
		Raycaster.Instance.AddListener ((Vector3 v, GameObject go)=>{
			if(GameController.Instance.Mode == GameMode.Adventure){
			HighlightedWarrior = go.GetComponent<BattleWarrior>();
			}
		}, ()=>{
			if(GameController.Instance.Mode == GameMode.Adventure){
			HighlightedWarrior = null;
			}
		}, 10, 0.1f, LayerRaycaster.InputType.none, 0);
		Raycaster.Instance.AddListener ((Vector3 v, GameObject go)=>{
			if(GameController.Instance.Mode == GameMode.Adventure){
			Warrior = go.GetComponent<BattleWarrior>();
			}
		}, ()=>{if(GameController.Instance.Mode == GameMode.Adventure){Warrior = null;}}, 10, 0.1f, LayerRaycaster.InputType.mouseDown, 0);
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
