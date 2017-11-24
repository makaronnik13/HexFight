using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureMovementManager : MonoBehaviour {

	public int raycastLayer = 9;
	public FormationType formationType;
	public float formationScale;
	private FakeController warrior;

	private void SelectWarrior(BattleWarrior warrior)
	{
		if(this.warrior)
		{
			RemoveRaycasters ();
			foreach(PlayerFollower pf in FindObjectsOfType<PlayerFollower>())
			{
				Destroy (pf);
			}
		}

		if (warrior) 
		{
			this.warrior = warrior.GetComponent<FakeController> ();
		} else {
			this.warrior = null;
		}


		if(this.warrior)
		{
			AddRaycasters ();
			int i = 0;

			foreach(BattleWarrior bw in FindObjectsOfType<BattleWarrior>())
			{
				
				if(bw!=this.warrior.GetComponent<BattleWarrior>() && bw.type == BattleWarrior.WarriorType.Player)
				{
					bw.gameObject.AddComponent<PlayerFollower> ().Init(i, FindObjectsOfType<BattleWarrior>().Length, formationType, formationScale);
					i++;
				}
			}

		}
	}

	private void OnEnable()
	{
		GameController.Instance.warriorSelected += SelectWarrior;
		GameController.Instance.onModeChanged += ModeChanged;
	}

	private void OnDisable()
	{
		GameController.Instance.warriorSelected -= SelectWarrior;
		GameController.Instance.onModeChanged -= ModeChanged;
	}

	private void ModeChanged(GameController.GameMode mode)
	{
		GameController.Instance.warriorSelected -= SelectWarrior;
		if (mode == GameController.GameMode.Adventure) {
			
			GameController.Instance.warriorSelected += SelectWarrior;
		}
	}

	private void AddRaycasters()
	{
		Raycaster.Instance.AddListener (RunAction, () => {}, raycastLayer, 0.1f, LayerRaycaster.InputType.mouseHold, 1, gameObject);
		Raycaster.Instance.AddListener (GoAction, () => {}, raycastLayer, 0.1f, LayerRaycaster.InputType.mouseDown, 1, gameObject);
	}

	private void RemoveRaycasters()
	{
		Raycaster.Instance.RemoveRaycaster (raycastLayer, LayerRaycaster.InputType.mouseHold, 1, gameObject);
		Raycaster.Instance.RemoveRaycaster (raycastLayer, LayerRaycaster.InputType.mouseDown, 1, gameObject);
	}

    private void RunAction(Vector3 point, GameObject go)
    {
        warrior.GoTo(point, true);
    }

    private void GoAction(Vector3 point, GameObject go)
    {
        warrior.GoTo(point, false);
    }
}
