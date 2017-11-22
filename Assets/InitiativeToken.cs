using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeToken : MonoBehaviour {

	private static float raundTime = 10;
	private static float speedCoef = 1;
	private RectTransform parentTransform;
	private float value = 0;
	private float visualValue = 0;



	public float speed = 1;
	//public BattleWarrior warrior;


	void Start () {
		parentTransform = transform.parent.GetComponent<RectTransform> ();
		InvokeRepeating ("MoveForward", 0, 0.01f);
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			speedCoef = 1;
		}
		
		visualValue = Mathf.Lerp (visualValue, (value-0.5f)*parentTransform.rect.height, Time.deltaTime*20*speedCoef);
		transform.localPosition = new Vector3(transform.localPosition.x, visualValue,transform.localPosition.z);
	}

	private void MoveForward()
	{
		value += speedCoef* speed * 0.01f / raundTime;
		if(value>=1)
		{
			speedCoef = 0;
			value = 0;
		}
	}

	public void ResumeInitiative()
	{
		speedCoef = 1;
	}
}
