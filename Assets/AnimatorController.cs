using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour {

	private Animator Controller
	{
		get
		{
			return GetComponent<Animator>();
		}
	}

	public void Atack()
	{
		Controller.SetTrigger ("Atack");
	}

	public void Walk(bool value)
	{
		Controller.SetBool ("Walking", value);
	}

	public void SpellCast()
	{
		Controller.SetTrigger ("SpellCast");
	}

	public void SpellCastSelf()
	{
		Controller.SetTrigger ("SpellCastSelf");
	}

	public void Die()
	{
		Controller.SetBool("Diying",true);
	}
}
