using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamSlider : MonoBehaviour
{
	public Text text;

	public void Init(BattleWarrior warrior)
	{
		text.enabled = false;
	}

	public void ShowText()
	{
		text.enabled = true;
	}

	public void HideText()
	{
		text.enabled = false;
	}
}

