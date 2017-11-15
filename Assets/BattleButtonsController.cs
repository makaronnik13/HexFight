using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleButtonsController : MonoBehaviour {

	private Animator animator
	{
		get
		{
			return GetComponent<Animator> ();
		}
	}

	public void Show()
	{
		CancelInvoke ("Close");
		animator.SetBool ("Open", true);
	}

	public void Hide()
	{
		Invoke ("Close", 1);
	}

	public void Close()
	{
		animator.SetBool ("Open", false);
	}
}
