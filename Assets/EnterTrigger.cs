using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnterTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public UnityEvent OnCursorEnter, OnCursorExit;

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (PointerEventData eventData)
	{
		if(OnCursorEnter!=null)
		{
			OnCursorEnter.Invoke ();
		}
	}

	#endregion

	#region IPointerExitHandler implementation

	public void OnPointerExit (PointerEventData eventData)
	{
		if(OnCursorExit!=null)
		{
			OnCursorExit.Invoke ();
		}
	}

	#endregion
}
