using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeToken : MonoBehaviour 
{
	
	private static float raundTime = 10;
	public static float speedCoef = 0;
	private RectTransform parentTransform;
	private float value = 0;
	private float visualValue = 0;
	private static float startValue = 0.25f;

	private static Color playerColor = new Color(113f/255, 188f/255, 40f/255);
	private static Color enemyColor = new Color(188f/255, 40f/255, 40f/255);
    private static Color neutralColor = new Color(200f / 255, 200f / 255, 200f / 255);

    private bool selected = false;
	private float randomInitiativeModificator = 0;

	public BattleWarrior warrior;

	private RectTransform line
	{
		get
		{
			return transform.GetChild (0).GetComponent<RectTransform> ();
		}
	}

	private Image circle
	{
		get
		{
			return transform.GetChild (1).GetComponent<Image> ();
		}
	}

	private Image portrait
	{
		get
		{
			return transform.GetChild (2).GetChild(0).GetComponent<Image> ();
		}
	}

	private RectTransform portraitTransform
	{
		get
		{
			return transform.GetChild (2).GetComponent<RectTransform> ();
		}
	}

	private float speed
	{
		get
		{
			return Mathf.Clamp(warrior.Initiative+randomInitiativeModificator,1,10) /10;
		}
	}

	private RectTransform rectTransform
	{
		get
		{
			return GetComponent<RectTransform> ();
		}
	}

	//public BattleWarrior warrior;

	public void ChangeValue(float changeAmount)
	{
		SetValue (value+changeAmount);
	}

	public void SetValue(float v)
	{
		value = Mathf.Clamp (v, 0, 1);
	}

	public void Init(BattleWarrior warrior)
	{
		this.warrior = warrior;
		value = startValue;
		parentTransform = transform.parent.GetComponent<RectTransform> ();
		InvokeRepeating ("MoveForward", 0, 0.01f);

        switch (warrior.type)
        {
            case BattleWarrior.WarriorType.Default:
                line.GetComponent<Image>().color = neutralColor;
                circle.color = neutralColor;
                break;
            case BattleWarrior.WarriorType.Player:
                line.GetComponent<Image>().color = playerColor;
                circle.color = playerColor;
                break;
            case BattleWarrior.WarriorType.Enemy:
                line.GetComponent<Image>().color = enemyColor;
                circle.color = enemyColor;
                break;
        }

		portrait.sprite = warrior.smallPortrait;
		randomInitiativeModificator = Random.Range (-0.5f, 0.5f);
		visualValue = (value - 0.5f) * parentTransform.rect.height;

		transform.localPosition = new Vector3(transform.localPosition.x, visualValue ,transform.localPosition.z);
	}

	void Update () 
	{
        Color aimColor = neutralColor;
        float xPos = 0;

        switch (warrior.type)
        {
            case BattleWarrior.WarriorType.Default:
                aimColor = neutralColor;
                xPos = 0;
                break;
            case BattleWarrior.WarriorType.Player:
                aimColor = playerColor;
                xPos = -rectTransform.rect.width;
                break;
            case BattleWarrior.WarriorType.Enemy:
                aimColor = enemyColor;
                xPos = rectTransform.rect.width;
                break;
        }

        line.GetComponent<Image>().color = Color.Lerp(line.GetComponent<Image>().color, aimColor, Time.deltaTime);
        circle.color = Color.Lerp(circle.color, aimColor, Time.deltaTime);

        rectTransform.localPosition = new Vector2(Mathf.Lerp(rectTransform.localPosition.x, xPos*0.8f, Time.deltaTime), rectTransform.localPosition.y);
        line.localPosition = new Vector2(Mathf.Lerp(line.localPosition.x, -xPos / 2, Time.deltaTime), line.localPosition.y);

     
		visualValue = Mathf.Lerp (visualValue, (value-0.5f)*parentTransform.rect.height, Time.deltaTime*8*speedCoef);
		transform.localPosition = new Vector3(transform.localPosition.x, visualValue,transform.localPosition.z);
		if (selected) {
			portraitTransform.localScale = Vector3.Lerp (portraitTransform.localScale, Vector3.one * 1.5f, Time.deltaTime*5);
			circle.transform.localScale = Vector3.Lerp (portraitTransform.localScale, Vector3.one * 1.5f, Time.deltaTime*5);
		} else {
			portraitTransform.localScale = Vector3.Lerp (portraitTransform.localScale, Vector3.one, Time.deltaTime*5);
			circle.transform.localScale = Vector3.Lerp (portraitTransform.localScale, Vector3.one, Time.deltaTime*5);
		}
	}

	private void MoveForward()
	{
		value += (speedCoef* speed * 0.01f / raundTime);
		if(value>=1)
		{
			speedCoef = 0;
			value = startValue;
			randomInitiativeModificator = Random.Range (-0.5f, 0.5f);
			
            GameController.Instance.CurrentField.SelectWarrior(warrior);
		}
	}

	public void ResumeInitiative()
	{
		speedCoef = 1;
	}

	public void ShowInfo()
	{
		transform.SetSiblingIndex (transform.parent.childCount-1);
		GameController.Instance.HighlightedWarrior = warrior;
		selected = true;
	}

	public void HideInfo()
	{
		GameController.Instance.HighlightedWarrior = null;
		selected = false;
	}
}
