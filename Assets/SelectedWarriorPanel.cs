using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedWarriorPanel : MonoBehaviour {
	public int WarriorsLayer = 10;

	private BattleWarrior selectedWarrior = null;
	private BattleWarrior lastWarrior = null;
	private BattleButtonsController buttonsController;

	public Image WarriorPortrait;
	public ParamSlider HpSlider, MpSlider, ApSlider;

	void Start()
	{
		buttonsController = GetComponentInChildren<BattleButtonsController> ();
		Hide ();
		lastWarrior = selectedWarrior;
		foreach (WarriorSelector ws in FindObjectsOfType<WarriorSelector>()) {
			ws.onPointerEnter += ShowWarrior;
			ws.onPointerClick += SelectWarrior;
			ws.onPointerExit += DeShowWarrior;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(selectedWarrior!=lastWarrior)
		{
			lastWarrior = selectedWarrior;
		}
	}

	private void ShowWarrior(BattleWarrior warrior)
	{
		buttonsController.Hide ();
		foreach(Transform t in transform){
			t.gameObject.SetActive (true);
		}
		WarriorPortrait.sprite = warrior.portrait;
		HpSlider.Init (warrior);
		MpSlider.Init (warrior);
		ApSlider.Init (warrior);
	}

	private void SelectWarrior(BattleWarrior warrior)
	{
		selectedWarrior = warrior;
	}

	private void DeShowWarrior(BattleWarrior warrior)
	{
		if (selectedWarrior) {
			ShowWarrior (selectedWarrior);
		} else {
			Hide ();
		}
	}

	private void Hide()
	{
		buttonsController.Hide ();
		foreach(Transform t in transform){
			t.gameObject.SetActive (false);
		}
	}

	public void ShowButtons()
	{
		if(!selectedWarrior.Enemy)
		{
			buttonsController.Show ();
		}
	}
}
