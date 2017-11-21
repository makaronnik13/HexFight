using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParamSlider : MonoBehaviour
{
	public enum SliderType
	{
		Hp,
		Mp,
		Ap
	}

	private int aim;
	private int maxAim;

	public SliderType type;


	private Slider parameterSlider;
	public Slider ParameterSlider
	{
		get
		{
			if(!parameterSlider)
			{
				parameterSlider = GetComponent<Slider> ();
			}
			return parameterSlider;
		}
	}

	private Text text;
	public Text ParameterText
	{
		get
		{
			if(!text)
			{
				text = GetComponentInChildren<Text> ();
			}
			return text;
		}
	}
	private BattleWarrior currentWarrior;

	public void Init(BattleWarrior warrior)
	{
		if(currentWarrior)
		{
			switch(type)
			{
			case SliderType.Ap:
				currentWarrior.OnApChanged -= ValueChanged;
				break;
			case SliderType.Hp:
				currentWarrior.OnHpChanged -= ValueChanged;
				break;
			case SliderType.Mp:
				currentWarrior.OnMpChanged -= ValueChanged;
				break;
			}
		}
		currentWarrior = warrior;
		switch(type)
		{
		case SliderType.Ap:
			currentWarrior.OnApChanged += ValueChanged;
			SetValues (currentWarrior.Ap, currentWarrior.MaxAp);
			break;
		case SliderType.Hp:
			currentWarrior.OnHpChanged += ValueChanged;
			SetValues (currentWarrior.Hp, currentWarrior.MaxHp);
			break;
		case SliderType.Mp:
			currentWarrior.OnMpChanged += ValueChanged;
			SetValues (currentWarrior.Mp, currentWarrior.MaxMp);
			break;
		}
	}

	public void ShowText()
	{
		CancelInvoke ("HideText");
		ParameterText.enabled = true;
	}

	public void HideText()
	{
		ParameterText.enabled = false;
	}

	private void ValueChanged(int a, int aMax)
	{
		maxAim = aMax;
		aim = a;
		ParameterText.text = a + "/" + aMax;
		ShowText ();
		Invoke ("HideText", 1.5f);
	}

	private void SetValues(int a, int aMax)
	{
		ParameterSlider.maxValue = maxAim = aMax;
		ParameterSlider.value =  aim = a;
		ParameterText.text = a + "/" + aMax;
		ShowText ();
		Invoke ("HideText", 1.5f);
	}

	private void Update()
	{
		if(ParameterSlider.maxValue!=maxAim)
		{
			ParameterSlider.maxValue = Mathf.Lerp (ParameterSlider.maxValue, maxAim, Time.deltaTime);
		}
		if(ParameterSlider.value!=aim)
		{
			ParameterSlider.value = Mathf.Lerp (ParameterSlider.value, aim, Time.deltaTime);
		}
	}
}

