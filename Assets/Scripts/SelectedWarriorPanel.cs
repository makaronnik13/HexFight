using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedWarriorPanel : MonoBehaviour {
	public int WarriorsLayer = 10;

	private BattleWarrior lastWarrior = null;
	private BattleButtonsController buttonsController;

	public Image WarriorPortrait;
	public ParamSlider ApSlider;

	void OnEnable(){
		if (GameController.Instance) {
			GameController.Instance.warriorPointed += PointWarrior;
			GameController.Instance.warriorDepointed += DepointWarrior;
			GameController.Instance.warriorSelected += SelectWarrior;
		}

	}

	void OnDisable(){
		if(GameController.Instance)
        {
		    GameController.Instance.warriorPointed -= PointWarrior;
		    GameController.Instance.warriorDepointed -= DepointWarrior;
		    GameController.Instance.warriorSelected -= SelectWarrior;
		}
	}

	void Start()
	{
		buttonsController = GetComponentInChildren<BattleButtonsController> ();
		Hide();
	}
	
	// Update is called once per frame
	void Update () {
		if (lastWarrior != GameController.Instance.HighlightedWarrior && GameController.Instance.Mode == GameController.GameMode.Battle) {
			ShowWarrior (GameController.Instance.HighlightedWarrior);
		} else {
			Hide ();
		}
	}

	private void ShowWarrior(BattleWarrior warrior)
	{
		
		Hide ();
        if (GameController.Instance.Mode == GameController.GameMode.Battle)
        {
            if (warrior)
            {
                foreach (Transform t in transform)
                {
                    t.gameObject.SetActive(true);
                }
                WarriorPortrait.sprite = warrior.portrait;
                ApSlider.Init(warrior);
            }
        }
	}



	private void PointWarrior(BattleWarrior warrior)
	{
		ShowWarrior (warrior);
	}


	private void DepointWarrior(BattleWarrior warrior)
	{
		ShowWarrior (lastWarrior);
	}


	private void SelectWarrior(BattleWarrior warrior)
	{
		lastWarrior = warrior;
		ShowWarrior (lastWarrior);
	}


	private void ShowButtons()
	{
		if(!lastWarrior.Enemy)
		{
			buttonsController.Show ();
		}
	}

	private void Hide()
	{
		buttonsController.Hide ();
		foreach(Transform t in transform){
			t.gameObject.SetActive (false);
		}

        if (lastWarrior)
        {
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(true);
            }
            WarriorPortrait.sprite = lastWarrior.portrait;
            ApSlider.Init(lastWarrior);
        }
    }
}
